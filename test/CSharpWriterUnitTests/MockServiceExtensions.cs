using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Microsoft.MockService.Extensions.ODataV4;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using Newtonsoft.Json.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    public static class MockServiceExtensions
    {
        public static DataServiceContextWrapper GetContext(this MockService serviceMock,
            Func<Task<string>> tokenGetterFunction = null)
        {
            tokenGetterFunction = tokenGetterFunction ?? Any.TokenGetterFunction();

            return new DataServiceContextWrapper(new Uri(serviceMock.GetBaseAddress()), ODataProtocolVersion.V4,
                tokenGetterFunction);
        }

        public static DataServiceContextWrapper GetDefaultContext(this MockService serviceMock,
            OdcmModel model)
        {
            return serviceMock
                .GetContext()
                .UseJson(model.ToEdmx(), true);
        }

        public static object CreateContainer(this MockService serviceMock, Type containerType,
            Func<Task<string>> tokenGetterFunction = null)
        {
            tokenGetterFunction = tokenGetterFunction ?? Any.TokenGetterFunction();

            return Activator.CreateInstance(containerType,
                new object[] {new Uri(serviceMock.GetBaseAddress()), tokenGetterFunction});
        }

        public static MockService SetupPostEntityPropertyChanges(this MockService mockService, EntityArtifacts targetEntity, IEnumerable<Tuple<string, object>> keyValues, OdcmProperty property)
        {
            return mockService
                .OnPostEntityRequest(targetEntity.Class.GetDefaultEntityPropertyPath(property, keyValues))
                .RespondWithODataOk();
        }

        public static MockService SetupGetEntityProperty(this MockService mockService, EntityArtifacts targetEntity, IEnumerable<Tuple<string, object>> keyValues, OdcmProperty property)
        {
            return mockService
                .OnGetEntityPropertyRequest(targetEntity.Class.GetDefaultEntityPath(keyValues), property.Name)
                .RespondWithGetEntity(targetEntity.Class.GetDefaultEntitySetName(),
                    targetEntity.ConcreteType.Initialize(targetEntity.Class.GetSampleKeyArguments()));
        }

        public static MockService SetupPostEntity(this MockService mockService, EntityArtifacts targetEntity, IEnumerable<Tuple<string, object>> propertyValues = null)
        {
            propertyValues = propertyValues ?? targetEntity.Class.GetSampleKeyArguments();

            return mockService
                .OnPostEntityRequest(targetEntity.Class.GetDefaultEntitySetPath())
                .RespondWithCreateEntity(targetEntity.Class.GetDefaultEntitySetName(), targetEntity.ConcreteType.Initialize(propertyValues));
        }

        public static MockService SetupGetEntity(this MockService mockService, EntityArtifacts targetEntity, 
            IEnumerable<Tuple<string, object>> keyValues = null, IEnumerable<string> expandTargets = null)
        {
            keyValues = keyValues ?? targetEntity.Class.GetSampleKeyArguments();

            keyValues = keyValues.ToList();

            var responseBuilder = expandTargets == null
                ? mockService.OnGetEntityRequest(targetEntity.Class.GetDefaultEntityPath(keyValues))
                : mockService.OnGetEntityWithExpandRequest(targetEntity.Class.GetDefaultEntityPath(keyValues),
                    expandTargets);

            return responseBuilder
                .RespondWithGetEntity(targetEntity.Class.GetDefaultEntitySetName(),
                    targetEntity.ConcreteType.Initialize(keyValues));
        }

        public static MockService SetupGetEntitySet(this MockService mockService, EntityArtifacts targetEntity,
            IEnumerable<string> expandTargets = null)
        {
            var responseBuilder = expandTargets == null
                ? mockService.OnGetEntityRequest(targetEntity.Class.GetDefaultEntitySetPath())
                : mockService.OnGetEntityWithExpandRequest(targetEntity.Class.GetDefaultEntitySetPath(), expandTargets);

            return responseBuilder
                .RespondWithGetEntity(targetEntity.Class.GetDefaultEntitySetName(),
                    targetEntity.ConcreteType.Initialize(targetEntity.Class.GetSampleKeyArguments()));
        }

        public static void ValidateParameterPassing(this MockService mockService, string httpMethod, object instance, string instancePath, OdcmMethod method, EntityArtifacts entityArtifacts)
        {
            var expectedMethodName = method.Name + "Async";

            var methodArguments = method.GetSampleArguments().ToArray();
            var uriArguments = method.UriParameters()
                .Select(p => methodArguments.First(a => a.Item1 == p.Name));
            var bodyArguments = method.BodyParameters()
                .Select(p => methodArguments.First(a => a.Item1 == p.Name));

            var responseBuilder = mockService
                .OnInvokeMethodRequest(httpMethod,
                    instancePath + "/" + method.FullName,
                    uriArguments.ToTestReadableStringCollection(),
                    ArgumentOfTupleExtensions.ToJObject(bodyArguments));

            if (entityArtifacts == null)
                responseBuilder.RespondWith(r => r.Response.StatusCode = 200);
            else
                responseBuilder.RespondWithGetEntity(entityArtifacts.Class.GetDefaultEntitySetName(),
                    entityArtifacts.ConcreteType.Initialize(entityArtifacts.Class.GetSampleKeyArguments()));

            instance.InvokeMethod<Task>(expectedMethodName, methodArguments.Select(t => t.Item2).ToArray())
                .Wait();
        }
    }
}
