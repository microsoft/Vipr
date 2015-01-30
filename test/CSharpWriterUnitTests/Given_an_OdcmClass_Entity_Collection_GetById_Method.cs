// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.OData.ProxyExtensions;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_GetById_Method : Given_an_OdcmClass_Entity_Collection_GetById_Base
    {
        private MethodInfo _getByIdMethod;

        
        public Given_an_OdcmClass_Entity_Collection_GetById_Method()
        {
            base.Init();

            _getByIdMethod = CollectionInterface
                .GetMethod(
                    "GetById",
                    ConcreteType.GetKeyProperties()
                        .Select(p => p.PropertyType)
                        .ToArray());
        }

        [Fact]
        public void It_returns_a_Fetcher_with_the_right_Context_and_Path()
        {
            var fetcher = CallGetByIdMethod(CollectionInstance, Params.Select(p => p.Item2));

            fetcher.Context.Should().Be(DscwMock.Object);

            FetcherType.BaseType.GetField("_path", PermissiveBindingFlags).GetValue(fetcher)
                .Should().Be(InstancePath);
        }

        protected RestShallowObjectFetcher CallGetByIdMethod(object collectionInstance, IEnumerable<object> parameters)
        {
            return _getByIdMethod.Invoke(collectionInstance, parameters.ToArray()) as RestShallowObjectFetcher;
        }
    }
}
