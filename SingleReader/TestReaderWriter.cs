using System.Collections.Generic;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace TestReaderWriter
{
    public class TestReaderWriter:IOdcmReader, IOdcmWriter
    {
        public OdcmModel GenerateOdcmModel(IDictionary<string, string> serviceMetadata)
        {
            return new OdcmModel(new Dictionary<string, string> {{"$metadata", ""}});
        }

        public IDictionary<string, string> GenerateProxy(OdcmModel model)
        {
            return new Dictionary<string, string>();
        }
    }
}
