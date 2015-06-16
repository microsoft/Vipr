using Microsoft.OData.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.OData.Services.OData
{
    public class ODataUri:IODataUri
    {
        public string BaseUri { get; set; }
        public List<string> PathComponents { get; private set; }
        public Dictionary<string, string> QueryStringParameters { get; private set; }

        public ODataUri()
        {
            PathComponents = new List<string>();
            QueryStringParameters = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(AppendTrailingSlash(BaseUri));

            foreach (var pathComponent in PathComponents)
            {
                builder.Append(AppendTrailingSlash(pathComponent));
            }

            builder.Remove(builder.Length - 1, 1);

            if (QueryStringParameters.Any())
            {
                builder.Append("?");

                foreach (var queryStringParameter in QueryStringParameters)
                {
                    builder.AppendFormat("{0}={1}&", Uri.EscapeUriString(queryStringParameter.Key), Uri.EscapeUriString(queryStringParameter.Value));
                }

                builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();
        }

        private string AppendTrailingSlash(string baseUrl)
        {
            if (!baseUrl.EndsWith("/"))
            {
                return baseUrl + "/";
            }

            return baseUrl;
        }
    }
}
