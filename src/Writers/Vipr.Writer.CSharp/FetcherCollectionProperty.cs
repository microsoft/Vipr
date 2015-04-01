using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpWriter
{
    class FetcherCollectionProperty : NavigationFetcherProperty
    {
        public string CollectionType { get; private set; }
    }
}
