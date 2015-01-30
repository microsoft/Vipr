using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace XAuth
{
    public class Auth
    {
        const string passwordTemplate = "grant_type=password&resource={0}&client_id={1}&username={2}&password={3}&scope=openid";
        const string refreshTokenTemplate = "grant_type=refresh_token&resource={0}&client_id={1}&refresh_token={2}";

        AuthSettings CurrentSettings { get; set; }

        string refreshToken;
        Dictionary<string, Tuple<string, DateTimeOffset>> cache = new Dictionary<string, Tuple<string, DateTimeOffset>>();

        public Auth()
            : this(AuthSettings.Prd)
        {
        }

        public Auth(AuthSettings settings)
        {
            CurrentSettings = settings;
        }

        private static DateTimeOffset ParseExpiresOn(string expiresIn)
        {
            try
            {
                // calculate expiration time from number of seconds until expiry then take off two minutes just to be safe.
                return DateTimeOffset.UtcNow + TimeSpan.FromSeconds(int.Parse(expiresIn, CultureInfo.InvariantCulture)) - TimeSpan.FromMinutes(2);
            }
            catch
            {
                // something went wrong, we'll just assume the token is expired and try again next time.
                return DateTimeOffset.MinValue;
            }
        }

        public async Task<string> GetAccessToken(string resource)
        {
            Tuple<string, DateTimeOffset> cacheEntry;
            if (cache.TryGetValue(resource, out cacheEntry))
            {
                if (DateTimeOffset.UtcNow < cacheEntry.Item2)
                {
                    return cacheEntry.Item1;
                }
            }

            JObject bearerToken = await GetAccessTokenFromRefreshTokenOrPassword(resource);

            refreshToken = bearerToken["refresh_token"].ToObject<string>();

            cache[resource] = new Tuple<string, DateTimeOffset>(bearerToken["access_token"].ToObject<string>(), ParseExpiresOn(bearerToken["expires_in"].ToObject<string>()));

            return bearerToken["access_token"].ToObject<string>();
        }

        private async Task<JObject> GetAccessTokenFromRefreshTokenOrPassword(string resource)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    return await GetAccessTokenFromRefreshToken(resource);
                }
                catch
                {
                    // if the refresh token is expired, we might get the boot.
                    // we'll drop through and try to log back in using the password.
                }
            }

            return await GetAccessTokenFromPassword(resource);
        }

        private async Task<JObject> GetAccessTokenFromRefreshToken(string resource)
        {
            return await GetAccessTokenFromContent(new StringContent(string.Format(refreshTokenTemplate, Uri.EscapeDataString(resource), Uri.EscapeDataString(CurrentSettings.ClientId), Uri.EscapeDataString(refreshToken)), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"));
        }

        private async Task<JObject> GetAccessTokenFromPassword(string resource)
        {
            return await GetAccessTokenFromContent(new StringContent(string.Format(passwordTemplate, Uri.EscapeDataString(resource), Uri.EscapeDataString(CurrentSettings.ClientId), Uri.EscapeDataString(CurrentSettings.Username), Uri.EscapeDataString(CurrentSettings.Password)), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"));
        }

        private async Task<JObject> GetAccessTokenFromContent(HttpContent requestContent)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, CurrentSettings.Authority))
                {
                    request.Content = requestContent;

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        JObject content = JObject.Parse(await response.Content.ReadAsStringAsync());

                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new Exception(content["error_description"].ToObject<string>());
                        }

                        return content;
                    }
                }
            }
        }
    }
}