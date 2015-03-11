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
    public abstract class Given_an_OdcmClass_Service_Bound_Function_Base : EntityTestBase
    {
        protected OdcmMethod Method;
        protected Func<Type, Type> ReturnTypeGenerator;
        protected bool IsCollection;
        private Type _expectedReturnType;
        private string _expectedMethodName;
        private IEnumerable<Type> _expectedMethodParameters;

        public void Init(Action<OdcmMethod> config = null)
        {
            Init(model => model.EntityContainer
                .Methods.Add(Method = Any.OdcmMethod(m =>
                {
                    m.ReturnType = model.Namespaces[0].Classes.First();
                    m.IsCollection = IsCollection;
                    m.IsBoundToCollection = false;

                    if (config != null) config(m);
                })));

            _expectedReturnType = ReturnTypeGenerator(ConcreteInterface);

            _expectedMethodName = Method.Name + "Async";

            _expectedMethodParameters = GetMethodParameterTypes();
        }

        [Fact]
        public void The_EntityContainer_interface_exposes_the_method()
        {
            EntityContainerInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                _expectedReturnType,
                _expectedMethodName,
                _expectedMethodParameters);
        }

        [Fact]
        public void The_EntityContainer_class_exposes_the_async_method()
        {
            EntityContainerType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                true,
                _expectedReturnType,
                _expectedMethodName,
                _expectedMethodParameters);
        }

        [Fact]
        public void When_the_verb_is_POST_the_Collection_passes_parameters_on_the_URI_and_in_the_body()
        {
            Init(model =>
            {
                Method = Any.OdcmMethodPost();
                Method.Class = model.EntityContainer;
                Method.ReturnType = Class;
                Method.IsCollection = false;
                Method.IsBoundToCollection = false;
                model.EntityContainer.Methods.Add(Method);
            });

            using (var mockService = new MockService()
                .Start())
            {
                var service = mockService
                    .CreateContainer(EntityContainerType);

                mockService.ValidateParameterPassing("POST", service, "", Method,
                    mockService.GetOdataJsonInstance(TargetEntity));
            }
        }

        [Fact]
        public void When_the_verb_is_GET_the_Collection_passes_parameters_on_the_URI()
        {
            Init(model =>
            {
                Method = Any.OdcmMethodGet();
                Method.Class = model.EntityContainer;
                Method.ReturnType = Class;
                Method.IsCollection = false;
                Method.IsBoundToCollection = false;
                model.EntityContainer.Methods.Add(Method);
            });

            using (var mockService = new MockService()
                .Start())
            {
                var service = mockService
                    .CreateContainer(EntityContainerType);

                mockService.ValidateParameterPassing("GET", service, "", Method,
                    mockService.GetOdataJsonInstance(TargetEntity));
            }
        }

        private IEnumerable<Type> GetMethodParameterTypes()
        {
            return Method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));
        }
    }
}
