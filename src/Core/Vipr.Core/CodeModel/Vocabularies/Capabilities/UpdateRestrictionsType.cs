using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    /*      <Term Name="UpdateRestrictions" Type="Capabilities.UpdateRestrictionsType" AppliesTo="EntitySet">
        <Annotation Term="Core.Description" String="Restrictions on update operations" />
      </Term>
      <ComplexType Name="UpdateRestrictionsType">
        <Property Name="Updatable" Type="Edm.Boolean" DefaultValue="true">
          <Annotation Term="Core.Description" String="Entities can be updated" />
        </Property>
        <Property Name="NonUpdatableNavigationProperties" Type="Collection(Edm.NavigationPropertyPath)">
          <Annotation Term="Core.Description" String="These navigation properties do not allow rebinding" />
        </Property>
      </ComplexType>
*/

    /// <summary>
    /// Restrictions on update operations
    /// </summary>
    public class UpdateRestrictionsType
    {
        /// <summary>
        /// Entities can be updated
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// These navigation properties do not allow rebinding
        /// </summary>
        public List<string> NonUpdatableNavigationProperties { get; set; }

        public UpdateRestrictionsType()
        {
            Updatable = true;
            NonUpdatableNavigationProperties = new List<string>();
        }
    }
}
