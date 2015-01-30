using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpWriter
{
    class FetcherProperty : Property
    {
        public string InstanceType { get; internal set; }
        public string ModelName { get; internal set; }
    }
}
