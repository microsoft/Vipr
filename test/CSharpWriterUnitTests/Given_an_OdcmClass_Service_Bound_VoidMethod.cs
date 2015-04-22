// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Service_Bound_VoidMethod : EntityTestBase
    {
        protected OdcmMethod Method;
        private readonly Type _expectedReturnType = typeof(Task);
        private readonly string _expectedMethodName;
        protected Func<string> ServerMethodNameGenerator;

        
        public Given_an_OdcmClass_Service_Bound_VoidMethod()
        {
            Method = Any.OdcmMethod();

            _expectedMethodName = Method.Name + "Async";

            Init(m => m.EntityContainer.Methods.Add(Method));
        }

        [Fact]
        public void The_EntityContainer_interface_exposes_the_method()
        {
            EntityContainerInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        [Fact]
        public void The_EntityContainer_class_exposes_the_async_method()
        {
            EntityContainerType.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                true,
                _expectedReturnType,
                _expectedMethodName,
                GetMethodParameterTypes());
        }

        private IEnumerable<Type> GetMethodParameterTypes()
        {
            return Method.Parameters.Select(p => Proxy.GetClass(p.Type.Namespace, p.Type.Name));
        }
    }
}
