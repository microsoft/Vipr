using System.Collections.Generic;

using OdcmTerms = Vipr.Core.CodeModel.Vocabularies.Capabilities.TermNames;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public static class ODataTermNames
    {
        public const string Updatable = "Org.OData.Capabilities.V1.UpdateRestrictions/Updatable";
        public const string NonUpdatableNavigationProperties = "Org.OData.Capabilities.V1.UpdateRestrictions/NonUpdatableNavigationProperties";
        public const string Deletable = "Org.OData.Capabilities.V1.DeleteRestrictions/Deletable";
        public const string NonDeletableNavigationProperties = "Org.OData.Capabilities.V1.DeleteRestrictions/NonDeletableNavigationProperties";
        public const string Insertable = "Org.OData.Capabilities.V1.InsertRestrictions/Insertable";
        public const string NonInsertableNavigationProperties = "Org.OData.Capabilities.V1.InsertRestrictions/NonInsertableNavigationProperties";
        public const string Expandable = "Org.OData.Capabilities.V1.ExpandRestrictions/Expandable";
        public const string NonExpandableProperties = "Org.OData.Capabilities.V1.ExpandRestrictions/NonExpandableProperties";
    }
}
