using System.Net.Http.Headers;
using Microsoft.OData.Services.Interfaces;
using System.Net.Http;

namespace Microsoft.OData.Services.Auth
{
    public class OAuthCredential: ICredentialProvider
    {
        private readonly string _credential;

        public OAuthCredential(string token)
        {
            _credential = token;
        }

        public void PrepareRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _credential);
        }
    }
}
