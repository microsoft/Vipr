using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace TestReaderWriter
{
    public class TestReaderWriter : IOdcmReader, IOdcmWriter
    {
        public OdcmModel GenerateOdcmModel(IEnumerable<TextFile> serviceMetadata)
        {
            return new OdcmModel(new List<TextFile> { new TextFile("$metadata", "") });
        }

        public IEnumerable<TextFile> GenerateProxy(OdcmModel model)
        {
            return Enumerable.Empty<TextFile>();
        }
    }
}
