using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpWriterUnitTests
{
    public class EnumType
    {
        private readonly Type _instance;

        internal EnumType(Type instance)
        {
            _instance = instance;
        }

        public string Name { get { return _instance.Name; } }

        public string Namespace { get { return _instance.Namespace; } }

        public IDictionary<string, object> Members
        {
            get
            {
                return _instance.GetEnumValues()
                    .Cast<object>()
                    .ToDictionary(
                        v => _instance.GetEnumName(v),
                        v => Convert.ChangeType(v, UnderlyingType()));
            }
        }

        public Type UnderlyingType()
        {
            return _instance.GetEnumUnderlyingType();
        }
    }
}