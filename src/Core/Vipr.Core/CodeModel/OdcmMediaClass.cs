using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel
{
    public class OdcmMediaClass : OdcmEntityClass
    {
        public OdcmMediaClass(string name, OdcmNamespace @namespace)
            : base(name, @namespace)
        {
            Kind = OdcmClassKind.MediaEntity;
        }
    }
}
