using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmCollectionCapability : OdcmCapability<IList<object>>
    {
        public OdcmCollectionCapability(IEnumerable<object> value, string termName)
            : base(value.ToList(), termName)
        {
        }
    }
}
