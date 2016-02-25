using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace CSharpWriter
{
    public class InitializeMethod : Method
    {
        public Identifier FetchedTypeInterface { get; set; }
        public Identifier FetchedType { get; set; }

        public override string Name
        {
            get { return "Initialize"; }
        }

        public override string Type
        {
            get { return string.Format("{0}<{1}>", NamesService.GetExtensionTypeFullName("IReadOnlyQueryableSet"), FetchedTypeInterface); }
        }
    }
}
