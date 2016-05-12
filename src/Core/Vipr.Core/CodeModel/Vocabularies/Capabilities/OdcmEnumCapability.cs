using System.Collections.Generic;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmEnumCapability : OdcmCapability<IEnumerable<string>>
    {
        public OdcmEnumCapability(IEnumerable<string> value, string termName) 
            : base(value, termName)
        {
        }
    }
}
