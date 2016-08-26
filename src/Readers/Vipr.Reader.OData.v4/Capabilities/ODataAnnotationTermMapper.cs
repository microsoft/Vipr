using System.Collections.Generic;
using System.Linq;

using OdcmTerms = Vipr.Core.CodeModel.Vocabularies.Capabilities.TermNames;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public class ODataAnnotationTermMapper : Core.CodeModel.Vocabularies.Capabilities.INameMapper
    {
        private static IDictionary<string, string> ODataMap = new Dictionary<string, string>
        {
            { OdcmTerms.Update, ODataTermNames.Updatable },
            { OdcmTerms.UpdateLink, ODataTermNames.NonUpdatableNavigationProperties },
            { OdcmTerms.Delete, ODataTermNames.Deletable },
            { OdcmTerms.DeleteLink, ODataTermNames.NonDeletableNavigationProperties },
            { OdcmTerms.Insert, ODataTermNames.Insertable },
            { OdcmTerms.Expand, ODataTermNames.Expandable },
        };

        public string ToExternal(string annotationTerm)
        {
            string odcmTerm;

            if (!ODataMap.TryGetValue(annotationTerm, out odcmTerm))
            {
                odcmTerm = annotationTerm;
            }
            return odcmTerm;
        }

        public string ToInternal(string name)
        {
            foreach (var pair in ODataMap)
            {
                if (pair.Value == name)
                {
                    return pair.Key;
                }
            }

            return name;
        }
    }
}
