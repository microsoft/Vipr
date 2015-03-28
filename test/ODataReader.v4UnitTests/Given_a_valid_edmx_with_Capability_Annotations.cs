// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using ODataReader.v4;
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
                .BeEquivalentTo(OdcmCapability.DefaultOdcmCapabilities,
                "because an entity set without any capability annotation should have default capabilities");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
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
                .BeEquivalentTo(OdcmCapability.DefaultOdcmCapabilities,
                "because a navigation property without any capability annotation should have default capabilities");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
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

            HasOdcmCapability<OdcmInsertCapability>(odcmEntitySet, insertable)
                .Should()
                .BeTrue("Because an entity set with insert annotation should have OdcmInsertCapability");

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

            HasOdcmCapability<OdcmUpdateCapability>(odcmEntitySet, updatable)
                .Should()
                .BeTrue("Because an entity set with update annotation should have OdcmUpdateCapability");

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

            HasOdcmCapability<OdcmDeleteCapability>(odcmEntitySet, deletable)
                .Should()
                .BeTrue("Because an entity set with delete annotation should have OdcmDeleteCapability");

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

            HasOdcmCapability<OdcmExpandCapability>(odcmEntitySet, expandable)
                            .Should()
                            .BeTrue("Because an entity set with expand annotation should have OdcmExpandCapability");

            odcmEntityType.Projections
                .Should()
                .Contain(odcmEntitySet.Projection, "because entity set's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_has_InsertRestriction_Then_Its_OdcmProperty_has_OdcmInsertCapability()
        {
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            var navPropertyName = GetRandomNavigationProperty(entityTypeElement);

            var insertable = Any.Bool();
            entitySetElement.Add(Any.Csdl.InsertRestrictionAnnotation(insertable, new List<string> { navPropertyName }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            HasOdcmCapability<OdcmInsertCapability>(odcmNavProperty, false)
                .Should()
                .BeTrue("Because a navigation property with insert annotation should have OdcmInsertCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_has_UpdateRestriction_Then_Its_OdcmProperty_has_OdcmUpdateCapability()
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

            HasOdcmCapability<OdcmUpdateCapability>(odcmNavProperty, false)
                .Should()
                .BeTrue("Because a navigation property with update annotation should have OdcmUpdateCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_has_DeleteRestriction_Then_Its_OdcmProperty_has_OdcmDeleteCapability()
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

            HasOdcmCapability<OdcmDeleteCapability>(odcmNavProperty, false)
                .Should()
                .BeTrue("Because a navigation property with delete annotation should have OdcmDeleteCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_NavigationProperty_has_ExpandRestriction_Then_Its_OdcmProperty_has_OdcmExpandCapability()
        {
            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entityTypeElement = _entitySetToEntityTypeMapping[entitySetElement];
            var entityTypeElementName = entityTypeElement.GetAttribute("Name");
            var navPropertyName = GetRandomNavigationProperty(entityTypeElement);

            var expandable = Any.Bool();
            entitySetElement.Add(Any.Csdl.ExpandRestrictionAnnotation(expandable, new List<string> { navPropertyName }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            HasOdcmCapability<OdcmExpandCapability>(odcmNavProperty, false)
                .Should()
                .BeTrue("Because a navigation property with expand annotation should have OdcmExpandCapability");

            odcmNavProperty.Projection.Type.Projections
                .Should()
                .Contain(odcmNavProperty.Projection, "because navigation property's Projection is retrieved from its OdcmType's cache of Projections");
        }

        [Fact]
        public void When_EntitySet_has_multiple_Capability_Annotations_Then_Its_OdcmProperty_has_corresponding_OdcmCapabilities()
        {
            var booleanValue = Any.Bool();
            var randomRestrictionAnnotations = GetRandomRestrictionAnnotationElements(booleanValue, null);
            int annotationCount = randomRestrictionAnnotations.Count();

            var entitySetElement = _entitySetToEntityTypeMapping.Keys.RandomElement();
            var entitySetElementName = entitySetElement.GetAttribute("Name");
            entitySetElement.Add(randomRestrictionAnnotations);

            var odcmModel = GetOdcmModel(_edmxElement);
            var odcmEntityContainer = GetOdcmEntityContainer(odcmModel);

            var odcmEntitySet = odcmEntityContainer.As<OdcmClass>().Properties.Single(p => p.Name == entitySetElementName);
            OdcmType odcmEntityType = odcmEntitySet.Projection.Type;

            odcmEntitySet.Projection.Capabilities.Count
                .Should()
                .Be(annotationCount,
                    string.Format(
                        "Because an entity set with {0} restriction annotations should have {0} OdcmCapabilities",
                        annotationCount));

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

            var randomRestrictionAnnotations = GetRandomRestrictionAnnotationElements(Any.Bool(),
                new List<string> { navPropertyName });
            int annotationCount = randomRestrictionAnnotations.Count();

            entitySetElement.Add(randomRestrictionAnnotations);

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmEntityType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmProperty odcmNavProperty = (odcmEntityType as OdcmClass).Properties.Single(p => p.Name == navPropertyName);

            odcmNavProperty.Projection.Capabilities.Count
               .Should()
               .Be(annotationCount,
                   string.Format(
                       "Because a navigation property with {0} restriction annotations should have {0} OdcmCapabilities",
                       annotationCount));

            foreach (var restrictionAnnotation in randomRestrictionAnnotations)
            {
                Type capabilityType = GetCapabilityTypeForAnnotation(restrictionAnnotation);
                HasOdcmCapability(odcmNavProperty, capabilityType, false)
                    .Should()
                    .BeTrue("Because a navigation property should have the correct OdcmCapability");
            }

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
            entitySetElement.Add(Any.Csdl.UpdateRestrictionAnnotation(Any.Bool(),
                new List<string> { navPropertyElement1Name, navPropertyElement2Name }));
            entitySetElement.Add(Any.Csdl.ExpandRestrictionAnnotation(Any.Bool(), new List<string> { navPropertyElement1Name }));
            entitySetElement.Add(Any.Csdl.InsertRestrictionAnnotation(Any.Bool(), new List<string> { navPropertyElement2Name }));

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmType odcmNavPropertyType = GetOdcmEntityType(odcmModel, navigationPropertyTypeName);

            odcmNavPropertyType.Projections.Count()
                .Should()
                .Be(3, "Because the OdcmModel must create 3 propjections including the Projection with Default Capabilities");

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

            // make sure that these two navigation properites have different restriction annotation
            var annotations = GetRandomRestrictionAnnotationElements(Any.Bool(),
                new List<string> { navPropertyElement1Name, navPropertyElement2Name });
            entitySetElement.Add(annotations);

            var odcmModel = GetOdcmModel(_edmxElement);
            OdcmType odcmType = GetOdcmEntityType(odcmModel, entityTypeElementName);
            OdcmType odcmNavPropertyType = GetOdcmEntityType(odcmModel, navigationPropertyTypeName);

            odcmNavPropertyType.Projections.Count()
                .Should()
                .Be(2, "Because the OdcmModel must create 2 propjections including the Projection with Default Capabilities");

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
            IEnumerable<XElement> entityTypeElements = XElement.Parse(entityTypesEdmx).Elements().Where(element => element.Name.LocalName == "EntityType");

            foreach (var entityTypeElement in entityTypeElements)
            {
                _schema.Add(entityTypeElement);
                var entitySetElement = Any.Csdl.EntitySet();
                entitySetElement.AddAttribute("EntityType", string.Format("{0}.{1}", _schemaNamespace, entityTypeElement.GetAttribute("Name")));
                _entitySetToEntityTypeMapping.Add(entitySetElement, entityTypeElement);
                _entityContainerElement.Add(entitySetElement);
            }

            _schema.Add(_entityContainerElement);
        }

        private ODataReader.v4.OdcmReader _odcmReader;
        private string _schemaNamespace;
        private string _entityContainerName;
        private XElement _edmxElement;
        private XElement _schema;
        private XElement _entityContainerElement;
        private Dictionary<XElement, XElement> _entitySetToEntityTypeMapping;


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
            var serviceMetadata = new TextFileCollection
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

        private bool HasOdcmCapability<T>(OdcmProperty odcmProperty, bool booleanValue) where T : OdcmBooleanCapability
        {
            return HasOdcmCapability(odcmProperty, typeof(T), booleanValue);
        }
    }
}
