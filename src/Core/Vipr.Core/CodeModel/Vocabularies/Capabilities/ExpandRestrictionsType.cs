using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    /*      <ComplexType Name="ExpandRestrictionsType">
        <Property Name="Expandable" Type="Edm.Boolean" DefaultValue="true">
          <Annotation Term="Core.Description" String="$expand is supported" />
        </Property>
        <Property Name="NonExpandableProperties" Type="Collection(Edm.NavigationPropertyPath)">
          <Annotation Term="Core.Description" String="These properties cannot be used in $expand expressions" />
        </Property>
      </ComplexType>
*/
    /// <summary>
    /// 
    /// </summary>
    public class ExpandRestrictionsType
    {
        /// <summary>
        /// $expand is supported
        /// </summary>
        public bool Expandable { get; set; }

        /// <summary>
        /// These properties cannot be used in $expand expressions
        /// </summary>
        public List<string> NonExpandableProperties { get; set; }

        public ExpandRestrictionsType()
        {
            Expandable = true;
            NonExpandableProperties = new List<string>();
        }
    }
}
