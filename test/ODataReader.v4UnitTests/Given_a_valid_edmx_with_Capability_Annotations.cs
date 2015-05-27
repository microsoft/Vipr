// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Vipr.Reader.OData.v4;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;
using Xunit;

namespace ODataReader.v4UnitTests
{
    public class Given_a_valid_edmx_with_Capability_Annotations
    {
        [Fact]
        public void When_EntitySet_has_no_Capability_Annotation_Then_Its_OdcmProperty_has_all_the_default_OdcmCapabilities()
        {
            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.RandomElement();
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultEntitySetCapabilities,
                "because an entity set without any capability annotation should have default capabilities");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");

            odcmEntityType.DefaultProjection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultOdcmCapabilities,
                    "because every OdcmType should have a default Projection with default capabilities");
        }

        [Fact]
        public void When_Singleton_has_no_Capability_Annotation_Then_Its_OdcmProperty_has_all_the_default_OdcmCapabilities()
        {
            var entityTypeElement = _entityTypeElements.RandomElement();
            var singletonElement = Any.Csdl.Singleton();
            var singletonName = singletonElement.GetAttribute("Name");
            singletonElement.AddAttribute("Type",
                string.Format("{0}.{1}", _schemaNamespace, entityTypeElement.GetAttribute("Name")));
            _entityContainerElement.Add(singletonElement);

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmSingleton = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == singletonName);
            OdcmType odcmEntityType = odcmSingleton.Projection.Type;

            odcmSingleton.Projection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultSingletonCapabilities,
                "because a singleton without any capability annotation should have default capabilities");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmSingleton.Projection, "because Singleton's Projection is retrieved from its OdcmType's cache of Projections");

            odcmEntityType.DefaultProjection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultOdcmCapabilities,
                    "because every OdcmType should have a default Projection with default capabilities");
        }

        [Fact]
        public void When_NavigationProperty_has_no_Capability_Annotation_Then_Its_OdcmProperty_has_all_the_default_OdcmCapabilities()
        {
            var insertable = Any.Bool();
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            //only entityset has insert restriction, the navigation properties are not annotated
            entitySetElement.Add(Any.Csdl.InsertRestrictionAnnotation(insertable, null));

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            OdcmProperty odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.RandomElement();

            odcmNavProperty.Projection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultPropertyCapabilities,
                "because a navigation property without any capability annotation should have default capabilities");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");

            odcmEntityType.DefaultProjection.Capabilities
                .Should()
                .BeEquivalentTo(OdcmCapability.DefaultOdcmCapabilities,
                    "because every OdcmType should have a default Projection with default capabilities");
        }

        [Fact]
        public void When_EntitySet_has_InsertRestriction_Then_Its_OdcmProperty_has_OdcmInsertCapability()
        {
            var insertable = Any.Bool();
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(Any.Csdl.InsertRestrictionAnnotation(insertable, null));

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.SupportsInsert()
                .Should()
                .Be(insertable, "Because an entity set with insert annotation should have OdcmInsertCapability");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_EntitySet_has_UpdateRestriction_Then_Its_OdcmProperty_has_OdcmUpdateCapability()
        {
            var updatable = Any.Bool();
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(updatable, null));

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.SupportsUpdate()
                .Should()
                .Be(updatable, "Because an entity set with update annotation should have OdcmUpdateCapability");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_EntitySet_has_DeleteRestriction_Then_Its_OdcmProperty_has_OdcmDeleteCapability()
        {
            var deletable = Any.Bool();
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(Any.Csdl.DeleteRestrictionAnnotation(deletable, null));

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.SupportsDelete()
                .Should()
                .Be(deletable, "Because an entity set with delete annotation should have OdcmDeleteCapability");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_EntitySet_has_ExpandRestriction_Then_Its_OdcmProperty_has_OdcmExpandCapability()
        {
            var expandable = Any.Bool();
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(Any.Csdl.ExpandRestrictionAnnotation(expandable, null));

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.SupportsExpand()
                .Should()
                .Be(expandable, "Because an entity set with expand annotation should have OdcmExpandCapability");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_is_annotated_with_NonUpdatableNavigationProperties_Then_Its_OdcmProperty_has_OdcmUpdateLinkCapability()
        {
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            var navPropertyName = GetRandomNavigationProperty(entityTypeElement);

            var updatable = Any.Bool();
            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(updatable, new List<string> { navPropertyName }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            odcmNavProperty.Projection.SupportsUpdateLink()
                .Should()
                .BeFalse("Because a navigation property with update annotation should not support OdcmUpdateLinkCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_is_annotated_with_NonDeletableNavigationProperties_Then_Its_OdcmProperty_has_OdcmDeleteLinkCapability()
        {
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            var navPropertyName = GetRandomNavigationProperty(entityTypeElement);

            var deletable = Any.Bool();
            entitySetElement.Add(Any.Csdl.DeleteRestrictionAnnotation(deletable, new List<string> { navPropertyName }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            odcmNavProperty.Projection.SupportsDeleteLink()
                .Should()
                .BeFalse("Because a navigation property with delete annotation should not support OdcmDeleteCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_EntitySet_has_multiple_Capability_Annotations_Then_Its_OdcmProperty_has_corresponding_OdcmCapabilities()
        {
            var booleanValue = Any.Bool();
            var randomRestrictionAnnotations = GetRandomRestrictionAnnotationElements(booleanValue, null);

            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(randomRestrictionAnnotations);

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            foreach (var restrictionAnnotation in randomRestrictionAnnotations)
            {
                Type capabilityType = GetCapabilityTypeForAnnotation(restrictionAnnotation);
                HasOdcmCapability(odcmEntitySet, capabilityType, booleanValue)
                    .Should()
                    .BeTrue("Because an entity set should have the correct OdcmCapability");
            }

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_has_multiple_Capability_Annotations_Then_Its_OdcmProperty_has_corresponding_OdcmCapabilities()
        {
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            var navPropertyName = GetRandomNavigationProperty(entityTypeElement);

            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(Any.Bool(),
                new List<string> { navPropertyName }));
            entitySetElement.Add(Any.Csdl.DeleteRestrictionAnnotation(Any.Bool(),
                new List<string> { navPropertyName }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            odcmNavProperty.Projection.SupportsUpdateLink()
                    .Should()
                    .BeFalse("Because a navigation property should have the correct OdcmUpdateLinkCapability");

            odcmNavProperty.Projection.SupportsDeleteLink()
                    .Should()
                    .BeFalse("Because a navigation property should have the correct OdcmDeleteLinkCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_multiple_OdcmProperties_with_same_OdcmType_have_different_Capabilities_Then_they_have_two_different_OdcmProjections()
        {
            // get a random entity set
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            // get the corresponding entity type
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            // get a random entity type which is different from the above entity type
            // we will use this as the type for a navigation property
            var navigationPropertyTypeName =
                _entitySetToEntityTypeMapping.Values.Where(element => element != entityTypeElement)
                    .RandomElement()
                    .GetAttribute("Name");

            // Lets create two navigation properties of the same type
            var navPropertyElement1 = Any.Csdl.NavigationProperty(OdcmObject.MakeCanonicalName(navigationPropertyTypeName, _schemaNamespace));
            var navPropertyElement1Name = navPropertyElement1.GetAttribute("Name");
            var navPropertyElement2 = Any.Csdl.NavigationProperty(OdcmObject.MakeCanonicalName(navigationPropertyTypeName, _schemaNamespace));
            var navPropertyElement2Name = navPropertyElement2.GetAttribute("Name");

            // now add these two navigation properties to an EntityType
            entityTypeElement = _schema.Elements().Single(e => e.GetAttribute("Name") == entityTypeElement.GetAttribute("Name"));
            entityTypeElement.Add(navPropertyElement1);
            entityTypeElement.Add(navPropertyElement2);

            // Scenario where they share one restriction annotation, but have two other different restriction annotation
            // This must create two different projections for these navigation properties
            entitySetElement.Add(Any.Csdl.InsertRestrictionAnnotation(Any.Bool(),
                new List<string> { navPropertyElement1Name, navPropertyElement2Name }));
            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(Any.Bool(), new List<string> { navPropertyElement1Name }));
            entitySetElement.Add(Any.Csdl.DeleteRestrictionAnnotation(Any.Bool(), new List<string> { navPropertyElement2Name }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmType odcmNavPropertyType = GetOdcmEntityType(odcmModel, navigationPropertyTypeName);

            OdcmProperty odcmNavProperty1 = (odcmType as OdcmClass).Properties.Single(p => p.Name == navPropertyElement1Name);
            OdcmProperty odcmNavProperty2 = (odcmType as OdcmClass).Properties.Single(p => p.Name == navPropertyElement2Name);

            odcmNavProperty1.Projection
                .Should()
                .NotBeSameAs(odcmNavProperty2.Projection,
                    "Two navigation properties with different capabilities should have different Projections");

            odcmNavPropertyType.Projections
                .Should()
                .Contain(odcmNavProperty1.Projection,
                    "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");

            odcmNavPropertyType.Projections
                .Should()
                .Contain(odcmNavProperty2.Projection,
                    "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_multiple_OdcmProperties_with_same_OdcmType_have_same_Capabilities_Then_they_have_one_shared_OdcmProjection()
        {
            // get a random entity set
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            // get the corresponding entity type
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            // get a random entity type which is different from the above entity type
            // we will use this as the type for a navigation property
            var navigationPropertyTypeName =
                _entitySetToEntityTypeMapping.Values.Where(element => element != entityTypeElement)
                    .RandomElement()
                    .GetAttribute("Name");

            // Lets create two navigation properties of the same type
            var navPropertyElement1 = Any.Csdl.NavigationProperty(OdcmObject.MakeCanonicalName(navigationPropertyTypeName, _schemaNamespace));
            var navPropertyElement1Name = navPropertyElement1.GetAttribute("Name");
            var navPropertyElement2 = Any.Csdl.NavigationProperty(OdcmObject.MakeCanonicalName(navigationPropertyTypeName, _schemaNamespace));
            var navPropertyElement2Name = navPropertyElement2.GetAttribute("Name");

            // now add these two navigation properties to an EntityType
            entityTypeElement = _schema.Elements().Single(e => e.GetAttribute("Name") == entityTypeElement.GetAttribute("Name"));
            entityTypeElement.Add(navPropertyElement1);
            entityTypeElement.Add(navPropertyElement2);

            // make sure that these two navigation properites have same restriction annotation
            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(Any.Bool(),
                new List<string> {navPropertyElement1Name, navPropertyElement2Name}));
            entitySetElement.Add(Any.Csdl.DeleteRestrictionAnnotation(Any.Bool(),
                new List<string> {navPropertyElement1Name, navPropertyElement2Name}));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmType odcmNavPropertyType = GetOdcmEntityType(odcmModel, navigationPropertyTypeName);

            odcmNavPropertyType.Projections.Count()
                .Should()
                .Be(2, "Because the OdcmModel must create 2 projections including the Projection with Default Capabilities");

            OdcmProperty odcmNavProperty1 = (odcmType as OdcmClass).Properties.Single(p => p.Name == navPropertyElement1Name);
            OdcmProperty odcmNavProperty2 = (odcmType as OdcmClass).Properties.Single(p => p.Name == navPropertyElement2Name);

            odcmNavProperty1.Projection
                .Should()
                .BeSameAs(odcmNavProperty2.Projection,
                    "Two navigation properties with same capabilities should have the same Projection");

            odcmNavPropertyType.Projections
                .Should()
                .Contain(odcmNavProperty1.Projection,
                    "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        public Given_a_valid_edmx_with_Capability_Annotations()
        {
            _odcmReader = new OdcmReader();
            _entitySetToEntityTypeMapping = new Dictionary<XElement, XElement>();

            _edmxElement = Any.Csdl.EdmxToSchema(schema => _schema = schema);
            _schemaNamespace = _schema.Attribute("Namespace").Value;

            _entityContainerElement = Any.Csdl.EntityContainer();
            _entityContainerName = _entityContainerElement.Attribute("Name").Value;

            string entityTypesEdmx = string.Format(ENTITY_TYPES_EDMX, _schemaNamespace);
            _entityTypeElements =
                XElement.Parse(entityTypesEdmx).Elements().Where(element => element.Name.LocalName == "EntityType");

            foreach (var entityTypeElement in _entityTypeElements)
            {
                _schema.Add(entityTypeElement);
                var entitySetElement = Any.Csdl.EntitySet();
                entitySetElement.AddAttribute("EntityType",
                    string.Format("{0}.{1}", _schemaNamespace, entityTypeElement.GetAttribute("Name")));
                _entitySetToEntityTypeMapping.Add(entitySetElement, entityTypeElement);
                _entityContainerElement.Add(entitySetElement);
            }

            _schema.Add(_entityContainerElement);
        }

        private Vipr.Reader.OData.v4.OdcmReader _odcmReader;
        private string _schemaNamespace;
        private string _entityContainerName;
        private XElement _edmxElement;
        private XElement _schema;
        private XElement _entityContainerElement;
        private Dictionary<XElement, XElement> _entitySetToEntityTypeMapping;
        private IEnumerable<XElement> _entityTypeElements;


        private const string ENTITY_TYPES_EDMX =
                      @"<EntityTypes>
                          <EntityType Name=""Notebook"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">	
	                        <NavigationProperty Name=""sections"" Type=""Collection({0}.Section)"" />
	                        <NavigationProperty Name=""sectionGroups"" Type=""Collection({0}.SectionGroup)"" />
                          </EntityType>
                          <EntityType Name=""SectionGroup"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
	                        <NavigationProperty Name=""parentNotebook"" Type=""{0}.Notebook"" />
	                        <NavigationProperty Name=""parentSectionGroup"" Type=""{0}.SectionGroup"" />
	                        <NavigationProperty Name=""sections"" Type=""Collection({0}.Section)"" />
	                        <NavigationProperty Name=""sectionGroups"" Type=""Collection({0}.SectionGroup)"" />
                          </EntityType>
                          <EntityType Name=""Section"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
	                        <NavigationProperty Name=""parentNotebook"" Type=""{0}.Notebook"" />
	                        <NavigationProperty Name=""parentSectionGroup"" Type=""{0}.SectionGroup"" />
	                        <NavigationProperty Name=""pages"" Type=""Collection({0}.Page)"" />
                          </EntityType>
                          <EntityType Name=""Page"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
	                        <NavigationProperty Name=""parentSection"" Type=""{0}.Section"" />
	                        <NavigationProperty Name=""parentNotebook"" Type=""{0}.Notebook"" />
                          </EntityType>
                        </EntityTypes>";

        private OdcmModel GetOdcmModel(XElement edmxElement)
        {
            var serviceMetadata = new List<TextFile>
            {
                new TextFile("$metadata", edmxElement.ToString())
            };

            var odcmModel = _odcmReader.GenerateOdcmModel(serviceMetadata);
            return odcmModel;
        }

        private OdcmType GetOdcmEntityContainer(OdcmModel odcmModel)
        {
            OdcmType odcmEntityContainer;
            odcmModel.TryResolveType(_entityContainerName, _schemaNamespace, out odcmEntityContainer)
                .Should()
                .BeTrue("because an entity container in the schema should result in an OdcmType");

            return odcmEntityContainer;
        }

        private OdcmType GetOdcmEntityType(OdcmModel odcmModel, string entityTypeName)
        {
            OdcmType odcmType;
            odcmModel.TryResolveType(entityTypeName, _schemaNamespace, out odcmType)
                .Should()
                .BeTrue("because an entity type in the schema should result in an OdcmType");

            return odcmType;
        }

        private string GetRandomNavigationProperty(XElement entityTypeElement)
        {
            string navPropertyName = string.Empty;
            XElement navPropertyElement =
                entityTypeElement.Elements()
                    .Where(element => element.Name.LocalName == "NavigationProperty")
                    .RandomElement();

            if (navPropertyElement != null)
            {
                navPropertyName = navPropertyElement.GetAttribute("Name");
            }

            return navPropertyName;
        }

        private IEnumerable<XElement> GetRandomRestrictionAnnotationElements(bool booleanValue, List<string> navPropertyPaths)
        {
            List<XElement> restrictionAnnotations = new List<XElement>()
            {
                Any.Csdl.InsertRestrictionAnnotation(booleanValue, navPropertyPaths),
                Any.Csdl.UpdateRestrictionAnnotation(booleanValue, navPropertyPaths),
                Any.Csdl.DeleteRestrictionAnnotation(booleanValue, navPropertyPaths),
                Any.Csdl.ExpandRestrictionAnnotation(booleanValue, navPropertyPaths)
            };

            int count = Any.Int(2, restrictionAnnotations.Count);
            return restrictionAnnotations.RandomSubset(count);
        }

        private string GetAnnotationTermName(XElement annotation)
        {
            string termName = string.Empty;
            if (annotation.Name.LocalName == "Annotation")
            {
                termName = annotation.GetAttribute("Term");
            }

            return termName;
        }


        private Type GetCapabilityTypeForAnnotation(XElement annotation)
        {
            Type returnType = null;
            var termName = GetAnnotationTermName(annotation);

            return GetCapabilityTypeForAnnotation(termName);
        }

        private Type GetCapabilityTypeForAnnotation(string termName)
        {
            Type returnType = null;

            var odcmCapability = OdcmCapability.DefaultOdcmCapabilities.SingleOrDefault(
                capability => capability.TermName == termName);

            if (odcmCapability != null)
                returnType = odcmCapability.GetType();

            return returnType;
        }

        private bool HasOdcmCapability(OdcmProperty odcmProperty, Type capabilityType, bool booleanValue)
        {
            var odcmCapability = odcmProperty.Projection.Capabilities.SingleOrDefault(capability =>
            {
                var booleanProperty = capability.GetType().Properties().OfType<bool>().First();
                return capability.GetType() == capabilityType &&
                       (bool)booleanProperty.GetValue(capability) == booleanValue;
            });

            return odcmCapability != null;
        }
    }
}
