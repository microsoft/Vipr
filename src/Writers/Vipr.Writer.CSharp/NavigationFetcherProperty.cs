using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpWriter
{
    public class NavigationFetcherProperty : NavigationProperty
    {
        public Identifier InstanceType { get; internal set; }
    }
}
