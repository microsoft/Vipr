using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ObsoletedProperty : Property
    {
        public string UpdatedName { get; private set; }

        public ObsoletedProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            Type = odcmProperty.IsCollection
                ? new Type(new Identifier("global::System.Collections.Generic", "IList"),
                    TypeService.GetPropertyType(odcmProperty))
                : TypeService.GetPropertyType(odcmProperty);
            UpdatedName = Name;
            Name = NamesService.GetModelPropertyName(odcmProperty);
        }
    }
}