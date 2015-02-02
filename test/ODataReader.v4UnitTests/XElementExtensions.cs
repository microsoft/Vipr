using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ODataReader.v4UnitTests
{
    public static class XElementExtensions
    {
        public static void AddAttribute(this XElement element, string name, object value)
        {
            element.Add(new XAttribute(name, value));
        }
    }
}
