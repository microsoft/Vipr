using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

using FluentAssertions;

using ODataReader.v4;

using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Vipr.Core;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_with_annotations
    {
        // TODO: At the moment, the sample edmx does not have annotations on the sites of all items which can be annotated.
        // It may be expedient to automatically generate these sorts of documents for different test cases. 
        [Fact]
        public void InsertRestrictions_should_be_properly_parsed()
        {
            OdcmReader odata4Reader = new OdcmReader();
            OdcmModel model = odata4Reader.GenerateOdcmModel(GetOneNoteEdmModel());

            model.EntityContainer.Properties.Should().Contain(prop => prop.Name == "notebooks");

            var notebookProperty = model.EntityContainer.Properties.First(prop => prop.Name == "notebooks");

            var insertRestriction = notebookProperty.Annotations.Should()
                .Contain(an => an.Name == "InsertRestrictions" && an.Namespace == "Org.OData.Capabilities.V1").Which;

            var annotationValue = insertRestriction.Value.Should().BeOfType<InsertRestrictionsType>().Which;

            annotationValue.Insertable.Should().BeTrue();
            annotationValue.NonInsertableNavigationProperties.Should().HaveCount(1).And.Contain("sectionGroups");
        }

        [Fact]
        public void UpdateRestrictions_should_be_properly_parsed()
        {
            OdcmReader odata4Reader = new OdcmReader();
            OdcmModel model = odata4Reader.GenerateOdcmModel(GetOneNoteEdmModel());

            model.EntityContainer.Properties.Should().Contain(prop => prop.Name == "notebooks");

            var notebookProperty = model.EntityContainer.Properties.First(prop => prop.Name == "notebooks");

            var updateRestriction = notebookProperty.Annotations.Should()
                .Contain(an => an.Name == "UpdateRestrictions" && an.Namespace == "Org.OData.Capabilities.V1").Which;

            var annotationValue = updateRestriction.Value.Should().BeOfType<UpdateRestrictionsType>().Which;

            annotationValue.Updatable.Should().BeFalse();
            annotationValue.NonUpdatableNavigationProperties.Should().HaveCount(2).And.Contain("sectionGroups").And.Contain("sections");
        }

        [Fact]
        public void DeleteRestrictions_should_be_properly_parsed()
        {
            OdcmReader odata4Reader = new OdcmReader();
            OdcmModel model = odata4Reader.GenerateOdcmModel(GetOneNoteEdmModel());

            model.EntityContainer.Properties.Should().Contain(prop => prop.Name == "notebooks");

            var notebookProperty = model.EntityContainer.Properties.First(prop => prop.Name == "notebooks");

            var deleteRestriction = notebookProperty.Annotations.Should()
                .Contain(an => an.Name == "DeleteRestrictions" && an.Namespace == "Org.OData.Capabilities.V1").Which;

            var annotationValue = deleteRestriction.Value.Should().BeOfType<DeleteRestrictionsType>().Which;

            annotationValue.Deletable.Should().BeFalse();
            annotationValue.NonDeletableNavigationProperties.Should().HaveCount(2).And.Contain("sectionGroups").And.Contain("sections");
        }

        private TextFileCollection GetOneNoteEdmModel()
        {
            return new TextFileCollection
            {
                new TextFile("$metadata", ODataReader.v4UnitTests.Properties.Resources.OneNoteExampleEdmx)
            };            
        }
    }
}
