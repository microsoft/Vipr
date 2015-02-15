// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using CSharpWriter.Settings;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_a_Configuration_Provider : CodeGenTestBase
    {
        private OdcmModel _model;

        
        public Given_a_Configuration_Provider()
        {
            _model = Any.OdcmModel();
        }

        [Fact]
        public void When_it_changes_a_namespace_then_the_proxy_reflects_the_change()
        {
            var namespaceMap = _model.Namespaces
                .RandomSubset(2)
                .ToDictionary(n => n.Name, n => Any.CSharpIdentifier());

            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    OdcmNamespaceToProxyNamespace = namespaceMap
                });

            var proxy = GetProxy(_model, configMock.Object);

            proxy.GetNamespaces()
                .Should()
                    .Contain(namespaceMap.Values)
                    .And
                    .NotContain(namespaceMap.Keys);
        }

        [Fact]
        public void When_it_changes_an_entity_name_then_the_proxy_reflects_the_change()
        {
            var nameMap = _model.Namespaces
                .ToDictionary(
                    n => n.Name,
                    n => (IDictionary<string, string>)n.Classes
                              .Where(c => c.Kind == OdcmClassKind.Entity)
                              .RandomSubset(2)
                              .ToDictionary(c => c.Name, c => Any.CSharpIdentifier()));

            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    OdcmClassNameToProxyClassName = nameMap
                });

            var proxy = GetProxy(_model, configMock.Object);

            foreach (var targetNamespace in nameMap.Keys)
            {
                var @namespace = targetNamespace;

                proxy.GetClasses()
                    .Where(c => c.Namespace == @namespace)
                    .Select(cl => cl.Name)
                    .Should()
                        .Contain(nameMap[targetNamespace].Values)
                        .And
                        .NotContain(nameMap[targetNamespace].Keys);
            }
        }

        [Fact]
        public void When_it_changes_an_entity_container_name_then_the_proxy_reflects_the_change()
        {
            var newEntityContainerName = Any.CSharpIdentifier();

            var nameMap =
                new Dictionary<string, IDictionary<string, string>>
                {
                    {
                        _model.EntityContainer.Namespace, new Dictionary<string, string>
                        {
                            {_model.EntityContainer.Name, newEntityContainerName}
                        }
                    }
                };

            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    OdcmClassNameToProxyClassName = nameMap
                });

            var proxy = GetProxy(_model, configMock.Object);

            proxy.GetClasses()
                .Where(c => c.Namespace == _model.EntityContainer.Namespace)
                .Select(c => c.Name)
                .Should()
                    .Contain(newEntityContainerName)
                    .And
                    .NotContain(_model.EntityContainer.Name);
        }

        [Fact]
        public void When_it_requests_a_namespace_prefix_then_the_proxy_reflects_the_prefix()
        {
            var prefix = Any.CSharpIdentifier();

            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);


            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    NamespacePrefix = prefix
                });

            var proxy = GetProxy(_model, configMock.Object);

            proxy.GetNamespaces()
                .Count(n => !n.StartsWith(prefix + "."))
                .Should().Be(0, "Because all namespaces should be prefixed");
        }

        [Fact]
        public void When_OmitFetcherCastOperators_is_true_then_base_types_do_not_expose_ToDerived_methods()
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);
            
            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    OmitFetcherUpcastMethods = true
                });

            var proxy = GetProxy(_model, configMock.Object);

            var fetcherCastMethods = _model.Namespaces.SelectMany(n => n.Classes.Select(c => "To" + c.Name));

            proxy.GetClasses()
                .Where(c => c.BaseType != null)
                .Select(c => c.BaseType)
                .Any(c => c.Methods().Any(m => fetcherCastMethods.Any(fcm => m.Name.EndsWith(fcm))))
                .Should()
                .BeFalse("Because no ToDerived methods should exist");
        }

        [Fact]
        public void When_OmitFetcherCastOperators_is_false_then_base_types_do_expose_ToDerived_methods()
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    OmitFetcherUpcastMethods = false
                });

            var proxy = GetProxy(_model, configMock.Object);

            var fetcherCastMethods = _model.Namespaces.SelectMany(n => n.Classes.Select(c => "To" + c.Name));

            proxy.GetClasses()
                .Where(c => c.BaseType != null)
                .Select(c => c.BaseType)
                .Any(c => c.Methods().Any(m => fetcherCastMethods.Any(fcm => m.Name.EndsWith(fcm))))
                .Should()
                .BeTrue("Because ToDerived methods should exist");
        }

        [Fact]
        public void When_ForcePropertyPascalCasing_is_true_then_all_properties_are_Pascal_Cased()
        {
            CamelCaseAllModelProperties();

            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);


            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    ForcePropertyPascalCasing = true
                });

            var proxy = GetProxy(_model, configMock.Object);

            foreach (var ns in _model.Namespaces)
            {
                foreach (var @class in ns.Classes)
                {
                    var cl = @class;

                    var propertyNames = proxy.GetClass(ns.Name, cl.Name)
                        .Properties()
                        .Select(p => p.Name.Substring(p.Name.LastIndexOf('.') + 1)).ToList();

                    if (cl.Kind != OdcmClassKind.Service)
                    {
                        propertyNames.Should().Contain(
                            cl.Properties.Select(p => p.Name).Where(n => n[0] != '_'),
                            because: "Because Complex and Entity Types should have obsoleted properties with original names.");
                    }

                    if (cl.Kind == OdcmClassKind.Entity)
                    {
                        proxy.GetInterface(ns.Name, "I" + cl.Name + "Fetcher")
                            .Properties()
                            .Select(p => p.Name.Substring(p.Name.LastIndexOf('.') + 1))
                            .Should().Contain(cl.NavigationProperties().Select(GetPascalCaseName).Where(n => n[0] != '_'), because: "Because the fetcher's navigation properties should be capitalized.")
                            .And.NotContain(cl.NavigationProperties().Select(p => p.Name).Where(n => n[0] != '_'), because: "Because the fetcher should not have obsoleted properties.");
                    }

                    propertyNames.Should()
                        .Contain(cl.Properties.Select(GetPascalCaseName).Where(n => n[0] != '_'), because: "Because all generated classes should have Pascal-Cased properties.");
                }
            }
        }

        [Fact]
        public void When_ForcePropertyPascalCasing_is_false_then_all_properties_maintain_casing()
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);


            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    ForcePropertyPascalCasing = false
                });

            var proxy = GetProxy(_model, configMock.Object);

            foreach (var ns in _model.Namespaces)
            {
                foreach (var @class in ns.Classes)
                {
                    var cl = @class;

                    proxy.GetClass(ns.Name, cl.Name)
                         .Properties()
                         .Select(p => p.Name.Substring(p.Name.LastIndexOf('.') + 1))
                         .Should()
                         .Contain(cl.Properties.Select(p => p.Name));
                }
            }
        }

        private void CamelCaseAllModelProperties()
        {
            foreach (var ns in _model.Namespaces)
            {
                foreach (var cl in ns.Classes)
                {
                    foreach (var property in cl.Properties.ToList())
                    {
                        cl.Properties.Remove(property);

                        var lowerCaseProperty =
                            new OdcmProperty(GetCamelCaseName(property))
                            {
                                Class = property.Class,
                                ReadOnly = property.ReadOnly,
                                Type = property.Type
                            };

                        cl.Properties.Add(lowerCaseProperty);
                    }
                }
            }
        }

        private static string GetCamelCaseName(OdcmProperty property)
        {
            return property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1);
        }

        private static string GetPascalCaseName(OdcmProperty property)
        {
            return property.Name.Substring(0, 1).ToUpper() + property.Name.Substring(1);
        }
    }
}
