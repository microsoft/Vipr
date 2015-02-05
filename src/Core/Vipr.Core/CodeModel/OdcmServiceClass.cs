using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel
{
    public class OdcmServiceClass : OdcmClass
    {
        public OdcmServiceClass(string name, string @namespace)
            : base(name, @namespace)
        {
            Kind = OdcmClassKind.Service;
        }
    }
}
