using System.Collections.Generic;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace TestReaderWriter
{
    public class TestReaderWriter:IOdcmReader, IOdcmWriter
    {
        public OdcmModel GenerateOdcmModel(FileGroup serviceMetadata)
        {
            return new OdcmModel(new Dictionary<string, string> {{"$metadata", ""}});
        }

        public FileGroup GenerateProxy(OdcmModel model)
        {
            return new FileGroup();
        }
    }
}
