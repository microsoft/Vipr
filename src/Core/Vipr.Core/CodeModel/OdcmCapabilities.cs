using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel
{
    public class OdcmCapabilities
    {
        public bool Insertable { get; set; }
        public bool Updateable { get; set; }
        public bool Deleteable { get; set; }
        public bool Expandable { get; set; }
        
        public OdcmCapabilities()
        {
            Insertable = true;
            Updateable = true;
            Deleteable = true;
            Expandable = true;
        }
    }
}
