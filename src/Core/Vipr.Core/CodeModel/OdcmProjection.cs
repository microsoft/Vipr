using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel.Vocabularies;

namespace Vipr.Core.CodeModel
{
    public class OdcmProjection
    {
        public OdcmType Type { get; set; }

        public OdcmCapabilities Capabilities { get; set; }

        public OdcmCapabilities EnsureCapabilities()
        {
            if (Capabilities == null)
            {
                Capabilities = new OdcmCapabilities();
            }
            return Capabilities;
        }
    }
}
