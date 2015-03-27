// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using CSharpWriter.Settings;
using FluentAssertions;
using Microsoft.CSharp;
using Microsoft.Its.Recipes;
using Moq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmObject_with_Description : XMLDocumentTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;

        public Given_an_OdcmObject_with_Description()
        {
            _model = new OdcmModel(Any.ServiceMetadata());
            _namespace = Any.EmptyOdcmNamespace();
            _model.Namespaces.Add(_namespace);
        }

        private string GetSummary(string xmlContent, string name)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            XElement members = doc.Root.Element("members");
            var member = members.Elements("member").SingleOrDefault(element => element.Attribute("name").Value == name);
            var summary = member == null ? string.Empty : member.Value.Trim();
            return summary;
        }

        [Fact]
        public void When_OdcmEnum_has_Description_then_it_has_the_right_summary_tag()
        {
            var @enum = Any.OdcmEnum(e =>
            {
                e.Namespace = _namespace;
                e.Description = Any.Paragraph(Any.Int(10, 20));
            });

            _model.AddType(@enum);
            string enumName = string.Format("T:{0}.{1}", @enum.Namespace.Name, @enum.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, enumName);
            summary
                .Should()
                .BeEquivalentTo(@enum.Description, "OdcmEnum.Description should be captured as C# document comment for enum");
        }

        [Fact]
        public void When_ComplexType_OdcmClass_has_Description_then_concrete_class_has_the_right_summary_tag()
        {
            var @class = Any.ComplexOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string className = string.Format("T:{0}.{1}", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, className);
            summary
                .Should()
                .BeEquivalentTo(@class.Description, "OdcmClass.Description should be captured as C# document comment for concrete class");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_concrete_class_has_the_right_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string className = string.Format("T:{0}.{1}", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, className);
            summary
                .Should()
                .BeEquivalentTo(@class.Description, "OdcmClass.Description should be captured as C# document comment for concrete class");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_concrete_interface_has_the_right_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string interfaceName = string.Format("T:{0}.I{1}", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, interfaceName);
            summary
                .Should()
                .BeEquivalentTo(@class.Description, "OdcmClass.Description should be captured as C# document comment for concrete interface");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_fetcher_class_does_not_have_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string fetcherName = string.Format("T:{0}.{1}Fetcher", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, fetcherName);
            summary
                .Should()
                .BeEmpty("OdcmClass.Description should not be captured as C# document comment for Fetcher class");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_fetcher_interface_does_not_have_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string fetcherInterfaceName = string.Format("T:{0}.I{1}Fetcher", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, fetcherInterfaceName);
            summary
                .Should()
                .BeEmpty("OdcmClass.Description should not be captured as C# document comment for Fetcher interface");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_collection_class_does_not_have_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string collectionName = string.Format("T:{0}.{1}Collection", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, collectionName);
            summary
                .Should()
                .BeEmpty("OdcmClass.Description should not be captured as C# document comment for Collection class");
        }

        [Fact]
        public void When_EntityType_OdcmClass_has_Description_then_collection_interface_does_not_have_summary_tag()
        {
            var @class = Any.EntityOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string collectionInterface = string.Format("T:{0}.I{1}Collection", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, collectionInterface);
            summary
                .Should()
                .BeEmpty("OdcmClass.Description should not be captured as C# document comment for Collection interface");
        }

        [Fact]
        public void When_OdcmServiceClass_has_Description_then_container_class_has_the_right_summary_tag()
        {
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string className = string.Format("T:{0}.{1}", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, className);
            summary
                .Should()
                .BeEquivalentTo(@class.Description, "OdcmServiceClass.Description should be captured as C# document comment for entity container class");
        }

        [Fact]
        public void When_OdcmServiceClass_has_Description_then_container_interface_has_the_right_summary_tag()
        {
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Description = Any.Paragraph(Any.Int(10, 20)));
            _model.AddType(@class);
            string interfaceName = string.Format("T:{0}.I{1}", @class.Namespace.Name, @class.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, interfaceName);
            summary
                .Should()
                .BeEquivalentTo(@class.Description, "OdcmServiceClass.Description should be captured as C# document comment for entity container interface");
        }

        [Fact]
        public void When_Structural_OdcmProperty_has_Description_then_concrete_class_property_has_the_right_summary_tag()
        {
            var property = Any.PrimitiveOdcmProperty(p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            string propertyName = string.Format("P:{0}.{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for concrete class property");
        }

        [Fact]
        public void When_Structural_OdcmProperty_has_Description_then_concrete_interface_property_has_the_right_summary_tag()
        {
            var property = Any.PrimitiveOdcmProperty(p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            string propertyName = string.Format("P:{0}.I{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for concrete interface property");
        }

        [Fact]
        public void When_Navigation_OdcmProperty_has_Description_then_concrete_class_properties_have_the_right_summary_tags()
        {
            var property = Any.EntityOdcmProperty(_namespace, p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);           

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string propertyName = string.Format("P:{0}.{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for concrete class property");

            string explicitInterfaceProperty = string.Format("P:{0}.{1}.{2}#I{3}#{4}", @class.Namespace.Name, @class.Name, @class.Namespace.Name, @class.Name, property.Name);
            summary = GetSummary(xmlContent, explicitInterfaceProperty);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for explicit concrete interface property");

            string explicitFetcherInterfaceProperty = string.Format("P:{0}.{1}.{2}#I{3}Fetcher#{4}", @class.Namespace.Name, @class.Name, @class.Namespace.Name, @class.Name, property.Name);
            summary = GetSummary(xmlContent, explicitFetcherInterfaceProperty);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for explicit fetcher interface property");
        }

        [Fact]
        public void When_Navigation_OdcmProperty_has_Description_then_concrete_interface_property_has_the_right_summary_tag()
        {
            var property = Any.EntityOdcmProperty(_namespace, p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);
            string propertyName = string.Format("P:{0}.I{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for concrete interface  property");
        }

        [Fact]
        public void When_Navigation_OdcmProperty_has_Description_then_fetcher_class_property_has_the_right_summary_tag()
        {
            var property = Any.EntityOdcmProperty(_namespace, p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);
            string propertyName = string.Format("P:{0}.{1}Fetcher.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for fetcher class property");
        }

        [Fact]
        public void When_Navigation_OdcmProperty_has_Description_then_fetcher_interface_property_has_the_right_summary_tag()
        {
            var property = Any.EntityOdcmProperty(_namespace, p => p.Description = Any.Paragraph(Any.Int(10, 20)));
            var @class = Any.EntityOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);
            string propertyName = string.Format("P:{0}.I{1}Fetcher.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, propertyName);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for fetcher interface property");
        }

        [Fact]
        public void When_EntitySet_OdcmProperty_has_Description_then_entity_container_class_property_has_the_right_summary_tag()
        {
            var property = Any.EntityOdcmProperty(_namespace, p => 
            {
                p.Description = Any.Paragraph(Any.Int(10, 20));
                p.IsCollection = true;
            });
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);
            string className = string.Format("P:{0}.{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, className);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for entity container class property");
        }

        [Fact]
        public void When_EntitySet_OdcmProperty_has_Description_then_entity_container_interface_property_has_the_right_summary_tag()
        {
            var property = Any.EntityOdcmProperty(_namespace, p =>
            {
                p.Description = Any.Paragraph(Any.Int(10, 20));
                p.IsCollection = true;
            });
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Properties.Add(property));
            property.Class = @class;
            _model.AddType(@class);
            _model.AddType(property.Type);
            string className = string.Format("P:{0}.I{1}.{2}", @class.Namespace.Name, @class.Name, property.Name);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            var summary = GetSummary(xmlContent, className);
            summary
                .Should()
                .BeEquivalentTo(property.Description, "OdcmProperty.Description should be captured as C# document comment for entity container interface property");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_concrete_class_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m => 
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.EntityOdcmClass(_namespace, c => c.Methods.Add(method));            
            _model.AddType(@class);            

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.{1}.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for concrete class method");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_concrete_interface_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.EntityOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.I{1}.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for concrete interface method");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_fetcher_class_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.EntityOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.{1}Fetcher.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for fetcher class method");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_fetcher_interface_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.EntityOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.I{1}Fetcher.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for fetcher interface method");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_entity_container_class_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.{1}.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for entity container class method");
        }

        [Fact]
        public void When_OdcmMethod_has_Description_then_entity_container_interface_method_has_the_right_summary_tag()
        {
            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
            });
            var @class = Any.ServiceOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            string methodName = string.Format("M:{0}.I{1}.{2}Async", @class.Namespace.Name, @class.Name, method.Name);
            var summary = GetSummary(xmlContent, methodName);
            summary
                .Should()
                .BeEquivalentTo(method.Description, "OdcmMethod.Description should be captured as C# document comment for entity container interface method");
        }

        [Fact]
        public void When_OdcmParameter_has_Description_then_parameter_has_the_right_param_tag()
        {
            var parameter = Any.OdcmParameter(p => 
            {
                p.Type = new OdcmPrimitiveType("Int64", Vipr.Core.CodeModel.OdcmNamespace.Edm);
                p.Description = Any.Paragraph(Any.Int(10, 20));
            });            

            var method = Any.OdcmMethod(m =>
            {
                m.Description = Any.Paragraph(Any.Int(10, 20));
                m.Parameters.Clear();
                m.Parameters.Add(parameter);
            });

            var @class = Any.EntityOdcmClass(_namespace, c => c.Methods.Add(method));
            _model.AddType(@class);

            var xmlContent = GetProxyXmlDocumentContent(_model);
            XDocument doc = XDocument.Parse(xmlContent);
            var members = doc.Root.Element("members").Elements("member");
            
            foreach(var member in members)
            {
                var param = member.Elements("param").Single(p => p.Attribute("name").Value == parameter.Name);
                var paramTag = param.Value.Trim();
                paramTag
                    .Should()
                    .BeEquivalentTo(parameter.Description, "OdcmParameter.Description should be captured as C# document comment for method parameters");
            }
        }
    }
}
