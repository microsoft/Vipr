using System;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IConcreteTypeAccessor
    {
        Type ConcreteType { get; }
        Type ElementType { get; }
    }
}