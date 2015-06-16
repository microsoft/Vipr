using System;
using System.Net.Http;

namespace Microsoft.OData.Services.Interfaces
{
    public interface IDependencyResolver
    {
        ILogger Logger { get; }

        IJsonSerializer JsonSerializer { get; }

        IODataUri CreateODataUri();

        HttpRequestMessage CreateRequest();

        String GetPlatformUserAgent(String productName);

        ICredentialProvider CredentialProvider { get; }
    }
}
