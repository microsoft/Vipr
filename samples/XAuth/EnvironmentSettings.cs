using System;

namespace XAuth
{
    public class EnvironmentSettings
    {
        public Uri EndpointUri { get; private set; }

        public string ResourceId { get; private set; }

        public EnvironmentSettings(Uri endpointUri, string resourceId)
        {
            ResourceId = resourceId;
            EndpointUri = endpointUri;
        }

        public static EnvironmentSettings SharePointPrd { get { return new EnvironmentSettings(new Uri(""),""); } }

        public static EnvironmentSettings ExchangePrd { get { return new EnvironmentSettings(new Uri("https://outlook.office365.com/ews/odata"), "https://outlook.office365.com/"); } }
    }
}
