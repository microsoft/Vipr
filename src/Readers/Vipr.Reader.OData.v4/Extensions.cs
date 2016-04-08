using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Vipr.Reader.OData.v4.Capabilities;

namespace Vipr.Reader.OData.v4
{
    public static class Extensions
    {
#if false
        public static void AddCapability(this OdcmObject odcmObject, OdcmCapability capability)
        {
            var capabilities = PropertyCapabilitiesCache.Instance.GetCapabilities(odcmObject);
            capabilities.Add(capability);
        }

        public static void InitializeCapabilities(this OdcmObject odcmObject, ICollection<OdcmCapability> capabilities)
        {
            PropertyCapabilitiesCache.Instance.Add(odcmObject, capabilities);
        }
#endif
    }
}
