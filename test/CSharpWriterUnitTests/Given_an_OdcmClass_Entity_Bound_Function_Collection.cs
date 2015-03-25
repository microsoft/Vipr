// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Bound_Function_Collection : Given_an_OdcmClass_Entity_Bound_Function_Base
    {
        public Given_an_OdcmClass_Entity_Bound_Function_Collection()
        {
            IsCollection = true;

            ReturnTypeGenerator = (t) => typeof (Task<>).MakeGenericType(typeof (IEnumerable<>).MakeGenericType(t));

            Init();
        }

        [Fact]
        public void The_Concrete_parses_the_response()
        {
            Func<MockService, EntityArtifacts, Tuple<string, object>> instanceGenerator =
                (m, e) =>
                {
                    var entityKeyValues = e.Class.GetSampleKeyArguments().ToArray();
                    var instancePath = Class.GetDefaultEntityPath(entityKeyValues);
                    var instance = m.SetupPostEntity(e, entityKeyValues)
                        .GetDefaultContext(Model)
                        .CreateConcrete(ConcreteType);

                    return new Tuple<string, object>(instancePath, instance);
                };

            ValidateCollectionResponseParsing(instanceGenerator);
        }

        [Fact]
        public void The_Fetcher_parses_the_response()
        {
            Func<MockService, EntityArtifacts, Tuple<string, object>> instanceGenerator =
                (m, e) =>
                {
                    var entityKeyValues = e.Class.GetSampleKeyArguments().ToArray();
                    var instancePath = e.Class.GetDefaultEntityPath(entityKeyValues);
                    var instance = m.GetDefaultContext(Model)
                        .CreateFetcher(FetcherType, instancePath);

                    return new Tuple<string, object>(instancePath, instance);
                };

            ValidateCollectionResponseParsing(instanceGenerator);
        }

        private void ValidateCollectionResponseParsing(
            Func<MockService, EntityArtifacts, Tuple<string, object>> instanceGenerator)

    {
        Init(m =>
        {
            m.Verbs = OdcmAllowedVerbs.Get;
            m.Parameters.Clear();
        });

        var responseKeyValues = Class.GetSampleKeyArguments().ToArray();
        var response = ConcreteType.Initialize(responseKeyValues);

        using (var mockService = new MockService())
        {
            var instanceAndPath = instanceGenerator(mockService, TargetEntity);

            mockService
                .OnInvokeMethodRequest("GET",
                    instanceAndPath.Item1 + "/" + Method.FullName,
                    null,
                    null)
                .RespondWithGetEntity(TargetEntity.Class.GetDefaultEntitySetName(), response);

            var instance = instanceAndPath.Item2;

            var result =
                instance.InvokeMethod<Task>(Method.Name + "Async").GetPropertyValue<IEnumerable<EntityBase>>("Result");

            result.ValidateCollectionPropertyValues(new List<IEnumerable<Tuple<string, object>>>
            {
                responseKeyValues.ToList()
            });
        }
    }
    }
}
