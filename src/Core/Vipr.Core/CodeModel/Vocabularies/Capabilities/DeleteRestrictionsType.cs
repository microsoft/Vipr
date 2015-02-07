using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    /*      <Term Name="DeleteRestrictions" Type="Capabilities.DeleteRestrictionsType" AppliesTo="EntitySet">
        <Annotation Term="Core.Description" String="Restrictions on delete operations" />
      </Term>
      <ComplexType Name="DeleteRestrictionsType">
        <Property Name="Deletable" Type="Edm.Boolean" DefaultValue="true">
          <Annotation Term="Core.Description" String="Entities can be deleted" />
        </Property>
        <Property Name="NonDeletableNavigationProperties" Type="Collection(Edm.NavigationPropertyPath)">
          <Annotation Term="Core.Description" String="These navigation properties do not allow DeleteLink requests" />
        </Property>
      </ComplexType>
*/
    /// <summary>
    /// Restrictions on delete operations
    /// </summary>
    public class DeleteRestrictionsType
    {
        /// <summary>
        /// Entities can be deleted
        /// </summary>
        public bool Deletable { get; set; }

        /// <summary>
        /// These navigation properties do not allow DeleteLink requests
        /// </summary>
        public List<string> NonDeletableNavigationProperties { get; set; }

        public DeleteRestrictionsType()
        {
            Deletable = true;
            NonDeletableNavigationProperties = new List<string>();
        }
    }
}
