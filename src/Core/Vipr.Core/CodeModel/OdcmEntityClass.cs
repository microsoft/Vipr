using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel
{
    public class OdcmEntityClass : OdcmClass
    {
        public List<OdcmProperty> Key { get; private set; }

        public OdcmEntityClass(string name, string @namespace) :
            base(name, @namespace)
        {
            Kind = OdcmClassKind.Entity;
            Key = new List<OdcmProperty>();
        }
    }
}
