using System;
using Newtonsoft.Json;

namespace XAuth
{
    public class AuthSettings
    {
        public Uri Authority { get; set; }
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthSettings() { }

        public AuthSettings(Uri authority, string clientId, string username, string password)
        {
            Authority = authority;
            ClientId = clientId;
            Username = username;
            Password = password;
        }

        public static AuthSettings Prd
        {
            get
            {
                return new AuthSettings(
                    new Uri("https://login.windows.net/common/oauth2/token"),
                    "YourClientId",
                    "YourUserName",
                    "YourPassword");
            }
        }

        public static string GetJsonTemplate()
        {
            return JsonConvert.SerializeObject(Prd);
        }
    }
}
