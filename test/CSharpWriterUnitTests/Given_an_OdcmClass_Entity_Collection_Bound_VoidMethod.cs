// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.MockService;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_Bound_VoidMethod : EntityTestBase
    {
        protected OdcmMethod Method;
        private readonly Type _expectedReturnType = typeof(Task);
        private readonly string _expectedMethodName;
        private readonly IEnumerable<Type> _expectedMethodParameters;
        protected Func<string> ServerMethodNameGenerator;

        
        public Given_an_OdcmClass_Entity_Collection_Bound_VoidMethod()
        {
            Method = Any.OdcmMethod(m => m.IsBoundToCollection = true);

            _expectedMethodName = Method.Name + "Async";

            _expectedMethodParameters = Method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));

            Init(m => m.Namespaces[0].Classes.First().Methods.Add(Method));

            ServerMethodNameGenerator = () => Method.FullName;
        }

        [Fact]
        public void The_Concrete_interface_does_not_expose_the_method()
        {
            ConcreteInterface.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void The_Concrete_class_does_not_expose_the_method()
        {
            ConcreteType.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void The_Fetcher_interface_does_not_expose_the_method()
        {
            FetcherInterface.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void The_Fetcher_class_does_not_expose_the_method()
        {
            FetcherType.Should().NotHaveMethod(_expectedMethodName);
        }

        [Fact]
        public void The_Collection_interface_exposes_the_method()
        {
            CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                _expectedReturnType,
                _expectedMethodName,
                _expectedMethodParameters);
        }

        [Fact]
        public void The_Collection_class_exposes_the_async_method()
        {
            CollectionType.Should().HaveMethod(
            CSharpAccessModifiers.Public,
            true,
            _expectedReturnType,
            _expectedMethodName,
            _expectedMethodParameters);
        }

        [Fact]
        public void When_the_verb_is_POST_the_Collection_passes_parameters_on_the_URI_and_in_the_body()
        {
            Init(m =>
            {
                Method = Any.OdcmMethodPost();
                Method.Class = Class;
                Method.ReturnType = null;
                Method.IsCollection = false;
                Method.IsBoundToCollection = true;
                Class.Methods.Add(Method);
            });

            using (var mockService = new MockService(true))
            {
                var collectionPath = Any.UriPath(1);

                var collection = mockService
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, collectionPath);

                mockService.ValidateParameterPassing("POST", collection, "/" + collectionPath, Method,
                    ServerMethodNameGenerator(), null);
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Concrete_passes_parameters_on_the_URI()
        {
            Init(m =>
            {
                Method = Any.OdcmMethodGet();
                Method.Class = Class;
                Method.ReturnType = null;
                Method.IsCollection = false;
                Method.IsBoundToCollection = true;
                Class.Methods.Add(Method);
            });

            using (var mockService = new MockService(true))
            {
                var collectionPath = Any.UriPath(1);

                var collection = mockService
                    .GetDefaultContext(Model)
                    .CreateCollection(CollectionType, ConcreteType, collectionPath);

                mockService.ValidateParameterPassing("GET", collection, "/" + collectionPath, Method,
                    ServerMethodNameGenerator(), null);
            }
        }
    }
}
