using System.Net.Http;

namespace Microsoft.OData.Services.Interfaces
{
    public interface ICredentialProvider
    {
        void PrepareRequest(HttpRequestMessage request);
    }
}
