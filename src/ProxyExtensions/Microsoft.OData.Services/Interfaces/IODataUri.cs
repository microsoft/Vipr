using System;
using System.Collections.Generic;

namespace Microsoft.OData.Services.Interfaces
{
    public interface IODataUri
    {
        String BaseUri { get; set; }

        List<String> PathComponents { get; }

        Dictionary<String, String> QueryStringParameters { get; }
    }
}
