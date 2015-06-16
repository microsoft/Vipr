using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.OData.Services.Interfaces;
using Microsoft.OData.Services.Utility;

namespace Microsoft.OData.Services.Auth
{
    public class BasicCredential: ICredentialProvider
    {
        private readonly string _credential;

        public BasicCredential(string user, string password)
        {
            _credential = (user + ":" + password).Base64Encode();
        }

        public void PrepareRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _credential);
        }
    }
}
