// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.ProxyExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxyExtensionsUnitTests
{
    public class ProxyExtensionsUnitTestsBase
    {
        private static string s_Edmx = "";

        public static global::Microsoft.OData.Edm.IEdmModel LoadModelFromString()
        {
            global::System.Xml.XmlReader reader = global::System.Xml.XmlReader.Create(new global::System.IO.StringReader(s_Edmx));
            try
            {
                return global::Microsoft.OData.Edm.Csdl.EdmxReader.Parse(reader);
            }
            finally
            {
                ((global::System.IDisposable)(reader)).Dispose();
            }
        }



        public static void GetEdmxStringFromMetadataPath(string metadataUri)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(metadataUri);
            using (var webResponse = webRequest.GetResponse())
            {
                var metadataStream = webResponse.GetResponseStream();
                using (var reader = new StreamReader(metadataStream))
                {
                    s_Edmx = reader.ReadToEnd();
                }
            }
        }

        public static DataServiceContextWrapper GetDataServiceContext(string uri)
        {
            GetEdmxStringFromMetadataPath(uri + "$metadata");

            var context = new DataServiceContextWrapper(new Uri(uri), 0, () => Task.FromResult(""));
            context.Format.LoadServiceModel = LoadModelFromString;
            context.Format.UseJson();

            return context;
        }
    }
}
