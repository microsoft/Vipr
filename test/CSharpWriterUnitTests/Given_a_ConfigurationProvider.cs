// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CSharpWriter;
using CSharpWriter.Settings;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using Microsoft.Owin;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_a_ConfigurationProvider : CodeGenTestBase
    {
        private OdcmModel _model;

        
        public Given_a_ConfigurationProvider()
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
        public void When_it_changes_a_namespace_then_requests_OdataType_is_set_to_the_old_namespace()
        {
            var oldNamespace = _model.EntityContainer.Namespace;

            var namespacePrefix = Any.CSharpIdentifier();

            var namespaceRename = Any.CSharpIdentifier();

            var newNamespace = new OdcmNamespace(namespacePrefix + "." + namespaceRename);

            var namespaceMap = new Dictionary<string, string> { { oldNamespace.Name, namespaceRename } };

            var proxy = GetProxyWithChangedNamespaces(namespacePrefix, namespaceMap);

            var @class = oldNamespace.Classes.OfType<OdcmEntityClass>().First();

            var entityArtifacts = GetEntityArtifactsFromNewNamespace(@class, newNamespace, proxy, oldNamespace);

            using (var mockService = new MockService())
            {
                mockService
                    .OnRequest(c => c.Request.Method == "POST" &&
                                c.Request.Path.Value == @class.GetDefaultEntitySetPath() &&
                                IsNamespaceReplaced(c.Request, oldNamespace.Name, newNamespace.Name))
                    .RespondWith(
                        (c, b) =>
                        {
                            c.Response.StatusCode = 201;
                            c.Response.WithDefaultODataHeaders();
                            c.Response.WithODataEntityResponseBody(b,
                                @class.GetDefaultEntitySetName(), null);
                        });

                var collection = mockService
                    .CreateContainer(proxy.GetClass(newNamespace.Name, _model.EntityContainer.Name))
                    .GetPropertyValue<ReadOnlyQueryableSetBase>(entityArtifacts.Class.GetDefaultEntitySetName());

                var instance = entityArtifacts.ConcreteType.Initialize(@class.GetSampleKeyArguments().ToArray());

                var task = collection.InvokeMethod<Task>("Add" + @class.Name + "Async", args: new[] { instance, false });

                task.Wait();
            }
        }

        [Fact(Skip = "https://github.com/Microsoft/Vipr/issues/43")]
        public void When_it_changes_a_namespace_then_responses_odata_type_is_translated_to_new_namespace()
        {
            var oldNamespace = _model.EntityContainer.Namespace; 
            
            var namespacePrefix = Any.CSharpIdentifier();

            var namespaceRename = Any.CSharpIdentifier();

            var newNamespace = new OdcmNamespace(namespacePrefix + "." + namespaceRename);

            var namespaceMap = new Dictionary<string, string> { { oldNamespace.Name, namespaceRename } };

            var entityClasses = oldNamespace.Classes.OfType<OdcmEntityClass>().ToList();

            var baseClass = entityClasses.Where(c => c.Base == null).RandomElement();

            entityClasses.Remove(baseClass);

            baseClass.IsAbstract = true;

            var derivedClass = entityClasses.RandomElement();

            entityClasses.Remove(derivedClass);

            derivedClass.Base = baseClass;

            entityClasses.RandomElement().Base = baseClass;

            var proxy = GetProxyWithChangedNamespaces(namespacePrefix, namespaceMap);

            var entityArtifacts = GetEntityArtifactsFromNewNamespace(derivedClass, newNamespace, proxy, oldNamespace);

            var responseObject = entityArtifacts.ConcreteType.Initialize(derivedClass.GetSampleKeyArguments().Concat(baseClass.GetSampleKeyArguments()).ToArray());

            var responseOdataType = String.Format("#{0}.{1}", oldNamespace.Name, derivedClass.Name);

            var singletonPath = baseClass.GetDefaultSingletonPath();

            using (var mockService = new MockService(true))
            {
                mockService
                    .OnRequest(c => c.Request.Method == "GET" && c.Request.Path.Value == singletonPath)
                    .RespondWith(
                        (c, b) =>
                        {
                            c.Response.StatusCode = 200;
                            c.Response.WithDefaultODataHeaders();
                            c.Response.WithODataEntityResponseBody(mockService.GetBaseAddress(),
                                baseClass.GetDefaultEntitySetName(), responseObject, new JProperty("@odata.type", new JValue(responseOdataType)));
                        });

                var fetcher = mockService
                    .CreateContainer(proxy.GetClass(newNamespace.Name, _model.EntityContainer.Name))
                    .GetPropertyValue<RestShallowObjectFetcher>(baseClass.GetDefaultSingletonName());

                var task = fetcher.ExecuteAsync();

                var result = task.GetPropertyValue<EntityBase>("Result");
            }
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
                        _model.EntityContainer.Namespace.Name, new Dictionary<string, string>
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
                .Where(c => c.Namespace == _model.EntityContainer.Namespace.Name)
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
                    OmitUpcastMethods = true
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
                    OmitUpcastMethods = false
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

        [Fact]
        public void When_no_MediaEntityAddAsyncVisibility_is_specified_then_all_Media_Entity_AddAsync_methods_default_to_Public()
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings());

            var proxy = GetProxy(_model, configMock.Object);

            foreach (var ns in _model.Namespaces)
            {
                var mediaEntityTypes = from @class in ns.Types
                    where @class is OdcmMediaClass
                    select @class as OdcmMediaClass;

                foreach (var @class in mediaEntityTypes)
                {
                    var cl = @class;

                    var addmethod = (from method in proxy.GetClass(ns.Name, cl.Name + "Collection").Methods()
                        where method.Name.Equals("Add" + cl.Name + "Async")
                        select method).FirstOrDefault();

                    addmethod
                        .Should()
                        .NotBeNull("Because every media entity collection class should have an add async method.");

                    addmethod.GetCSharpAccessModifier()
                        .Should()
                        .Be(CSharpAccessModifiers.Public, "Because all media entity add async methods should default to public access.");
                }
            }
        }

        [Fact]
        public void When_MediaEntityAddAsyncVisibility_is_specified_then_all_Media_Entity_AddAsync_methods_have_that_visiblity()
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            for (var accessModifier = (CSharpAccessModifiers) 0; accessModifier <= (CSharpAccessModifiers) 4; accessModifier++)
            {
                Visibility visibility;
                Visibility.TryParse(accessModifier.ToString(), true, out visibility);

                configMock
                    .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                    .Returns(() => new CSharpWriterSettings
                    {
                        MediaEntityAddAsyncVisibility = visibility
                    });

                var proxy = GetProxy(_model, configMock.Object);

                foreach (var ns in _model.Namespaces)
                {
                    var mediaEntityTypes = from @class in ns.Types 
                                           where @class is OdcmMediaClass 
                                           select @class as OdcmMediaClass;

                    foreach (var @class in mediaEntityTypes)
                    {
                        var cl = @class;

                        var addmethod = (from method in proxy.GetClass(ns.Name, cl.Name + "Collection").Methods()
                                         where method.Name.Equals("Add" + cl.Name + "Async")
                                         select method).FirstOrDefault();

                        addmethod
                            .Should()
                            .NotBeNull("Because every media entity collection class should have an add async method.");

                        addmethod.GetCSharpAccessModifier()
                            .Should()
                            .Be(accessModifier, "Because all media entity add async methods should have the specified visibility.");
                    }
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

        private bool IsNamespaceReplaced(IOwinRequest request, string oldNamespace, string newNamespace)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(request.Body);
            var requestBody = reader.ReadToEndAsync().Result;
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBody.Contains(oldNamespace) &&
                   !requestBody.Contains(newNamespace);
        }

        private Assembly GetProxyWithChangedNamespaces(string namespacePrefix, Dictionary<string, string> namespaceMap)
        {
            var configMock = new Mock<IConfigurationProvider>(MockBehavior.Loose);

            configMock
                .Setup(c => c.GetConfiguration<CSharpWriterSettings>())
                .Returns(() => new CSharpWriterSettings
                {
                    NamespacePrefix = namespacePrefix,
                    OdcmNamespaceToProxyNamespace = namespaceMap
                });

            var proxy = GetProxy(_model, configMock.Object);
            return proxy;
        }

        private static EntityArtifacts GetEntityArtifactsFromNewNamespace(OdcmEntityClass @class, OdcmNamespace newNamespace,
            Assembly proxy, OdcmNamespace oldNamespace)
        {
            @class.Namespace = newNamespace;

            var entityArtifacts = @class.GetArtifactsFrom(proxy);

            @class.Namespace = oldNamespace;

            return entityArtifacts;
        }
    }
}
