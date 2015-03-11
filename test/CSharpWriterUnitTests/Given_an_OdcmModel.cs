// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmModel : CodeGenTestBase
    {
        private OdcmModel _model;

        
        public Given_an_OdcmModel()
        {
            _model = new OdcmModel(Any.ServiceMetadata());
        }

        [Fact]
        public void When_it_has_no_namespace_its_proxy_is_empty()
        {
            var proxy = GetProxy(new OdcmModel(Any.ServiceMetadata()));

            proxy.GetNamespaces()
                .Should().BeEmpty("Because there was nothing to generate");
        }

        [Fact]
        public void When_ServiceType_is_ODataV4_then_Microsoft_OData_is_Referenced()
        {
            var model = new OdcmModel(Any.ServiceMetadata(), ServiceType.ODataV4);

            var odmcNamespace = Any.OdcmNamespace();

            model.Namespaces.Add(odmcNamespace);

            model.AddType(Any.ServiceOdcmClass(odmcNamespace));

            var Class = Any.EntityOdcmClass(odmcNamespace);
            model.AddType(Class);

            var proxy = GetProxy(model);

            proxy.GetReferencedAssemblies()
                .Select(a => a.Name)
                .Distinct()
                .Should().Contain(new[]
                {
                    "System.Runtime", "Microsoft.OData.ProxyExtensions", "System.Threading.Tasks",
                    "System.Linq.Expressions", "Microsoft.OData.Client", "Microsoft.OData.Edm",
                    "System.Xml.ReaderWriter", "System.Diagnostics.Debug", "System.Reflection", "System.IO",
                });
        }
    }
}
