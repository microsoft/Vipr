// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.ProxyExtensions;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Bound_Function_Instance : Given_an_OdcmClass_Entity_Bound_Function_Base
    {
        public Given_an_OdcmClass_Entity_Bound_Function_Instance()
        {
            IsCollection = false;

            ReturnTypeGenerator = (t) => typeof(Task<>).MakeGenericType(t);

            Init();
        }

        [Fact]
        public void The_Concrete_parses_the_response()
        {
            Init(m =>
            {
                m.Verbs = OdcmAllowedVerbs.Get;
                m.Parameters.Clear();
            });

            var entityKeyValues = Class.GetSampleKeyArguments().ToArray();
            var responseKeyValues = Class.GetSampleKeyArguments().ToArray();
            var instancePath = Class.GetDefaultEntityPath(entityKeyValues);

            using (var mockService = new MockService()
                .SetupPostEntity(TargetEntity, entityKeyValues)
                .Start())
            {
                mockService.SetupMethod("GET",
                    instancePath + "/" + Method.FullName,
                    null,
                    null,
                    mockService.GetOdataJsonInstance(TargetEntity, responseKeyValues));

                var concrete = mockService
                    .GetDefaultContext(Model)
                    .CreateConcrete(ConcreteType);

                var result = concrete.InvokeMethod<Task>(Method.Name + "Async").GetPropertyValue<EntityBase>("Result");

                result.ValidatePropertyValues(responseKeyValues);
            }
        }
    }
}
