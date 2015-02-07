using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ObsoletedNavigationProperty : ObsoletedProperty
    {
        public ObsoletedNavigationProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            Type = odcmProperty.IsCollection
                ? new Type(new Identifier("global::System.Collections.Generic", "IList"),
                    new Type(NamesService.GetConcreteTypeName(odcmProperty.Type)))
                : new Type(NamesService.GetConcreteTypeName(odcmProperty.Type));
        }
    }
}