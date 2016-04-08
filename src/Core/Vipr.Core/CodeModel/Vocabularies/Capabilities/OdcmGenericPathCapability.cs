using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public class OdcmGenericPathCapability : OdcmCapability
    {
        private readonly string _termName;
        public override string TermName => _termName;

        public OdcmGenericPathCapability(string termName)
        {
            _termName = termName;
        }

        public override bool Equals(OdcmCapability otherCapability)
        {
            var other = otherCapability as OdcmGenericPathCapability;
            if (other == null)
            {
                return false;
            }

            return other.GetType() == this.GetType() && other.TermName == this.TermName;
        }

        public override int GetHashCode()
        {
            int hash = this.GetType().GetHashCode();
            return hash * 31 + TermName.GetHashCode();
        }
    }
}
