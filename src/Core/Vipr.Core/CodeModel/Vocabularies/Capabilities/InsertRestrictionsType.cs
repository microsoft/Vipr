using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    // TODO: These classes shouldn't need to be handwritten. Make sure VIPR can output them in a nice way. 
    // This was generated from this XML: 
    /* <ComplexType Name="InsertRestrictionsType">
        <Property Name="Insertable" Type="Edm.Boolean" DefaultValue="true">
        <Annotation Term="Core.Description" String="Entities can be inserted"/>
        </Property>
        <Property Name="NonInsertableNavigationProperties" Type="Collection(Edm.NavigationPropertyPath)">
        <Annotation Term="Core.Description" String="These navigation properties do not allow deep inserts"/>
        </Property>
        </ComplexType>
    */
    
    public class InsertRestrictionsType
    {
        /// <summary>
        /// Entities can be inserted
        /// </summary>
        public bool Insertable { get; set; }

        //TODO: Is there a more correct mapping to Edm.NavigationPropertyPath? 
        /// <summary>
        /// These navigation properties do not allow deep inserts
        /// </summary>
        public List<string> NonInsertableNavigationProperties { get; set;}

        public InsertRestrictionsType()
        {
            // TODO: Code gen for default value semantics
            Insertable = true;
            NonInsertableNavigationProperties = new List<string>();
        }

    }
}
