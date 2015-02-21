using System.Collections.Generic;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace TestReaderWriter
{
    public class TestReaderWriter:IOdcmReader, IOdcmWriter
    {
        public OdcmModel GenerateOdcmModel(TextFileCollection serviceMetadata)
        {
            return new OdcmModel(new TextFileCollection {new TextFile("$metadata", "")});
        }

        public TextFileCollection GenerateProxy(OdcmModel model)
        {
            return new TextFileCollection();
        }
    }
}
