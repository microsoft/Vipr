// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Linq;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmEnum : CodeGenTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;

        
        public Given_an_OdcmEnum()
        {
            _model = new OdcmModel(Any.ServiceMetadata());

            _namespace = Any.EmptyOdcmNamespace();

            _model.Namespaces.Add(_namespace);
        }

        [Fact]
        public void When_it_is_generated_it_has_the_right_namespace_and_name()
        {
            var @enum = Any.OdcmEnum(e => e.Namespace = _namespace.Name);

            _model.AddType(@enum);

            var proxy = GetProxy(_model);

            var enumTypes = proxy.GetEnums();

            enumTypes.First().Name
                .Should().Be(@enum.Name);

            enumTypes.First().Namespace
                .Should().Be(@enum.Namespace);
        }

        [Fact]
        public void When_it_has_no_members_then_its_proxy_exposes_it_as_empty()
        {
            var @enum = Any.OdcmEnum(e => e.Namespace = _namespace.Name);

            _model.AddType(@enum);

            var proxy = GetProxy(_model);

            var enumType = proxy.GetEnum(@enum.Namespace, @enum.Name);

            enumType
                .Should().NotBeNull();

            enumType.Members
                .Should().HaveCount(0);
        }

        [Fact]
        public void When_it_has_no_underlying_type_then_its_proxy_exposes_underlying_type_as_int32()
        {
            var @enum = Any.OdcmEnum(e =>
                {
                    e.Namespace = _namespace.Name;
                    e.UnderlyingType = null;
                });

            _model.AddType(@enum);

            var proxy = GetProxy(_model);

            var enumType = proxy.GetEnum(@enum.Namespace, @enum.Name);

            enumType
                .Should().NotBeNull();

            enumType.UnderlyingType()
                .Should().Be<int>();
        }

        [Fact]
        public void When_it_has_members_without_values_then_its_proxy_exposes_it_with_the_members()
        {
            var @enum = Any.OdcmEnum(e => e.Namespace = _namespace.Name);

            @enum.Members.Add(Any.OdcmEnumMember());
            @enum.Members.Add(Any.OdcmEnumMember());
            @enum.Members.Add(Any.OdcmEnumMember());

            _model.AddType(@enum);

            var proxy = GetProxy(_model);

            var enumType = proxy.GetEnum(@enum.Namespace, @enum.Name);

            enumType
                .Should().NotBeNull();

            enumType.UnderlyingType()
                .Should().Be(EdmToClrTypeMap[@enum.UnderlyingType.Name]);

            enumType.Members.Keys
                .Should().HaveCount(3)
                .And.Equal(@enum.Members.Select(m => m.Name));

            enumType.Members.Values.Distinct().Count()
                .Should().Be(enumType.Members.Values.Count());
        }

        [Fact]
        public void When_it_has_members_with_values_then_its_proxy_exposes_it_with_the_members_and_values()
        {
            var @enum = Any.OdcmEnum(e => e.Namespace = _namespace.Name);

            Any.Sequence(x => x, 127)
                .RandomSubset(3).ToList()
                .ForEach(v => @enum.Members.Add(Any.OdcmEnumMember(m => m.Value = v)));

            _model.AddType(@enum);

            var proxy = GetProxy(_model);

            var enumType = proxy.GetEnum(@enum.Namespace, @enum.Name);

            enumType
                .Should().NotBeNull();

            enumType.UnderlyingType()
                .Should().Be(EdmToClrTypeMap[@enum.UnderlyingType.Name]);

            var odcmEnumMemberDict = @enum.Members.ToDictionary(e => e.Name, e => Convert.ChangeType(e.Value, enumType.UnderlyingType()));
            enumType.Members
                .Should().HaveCount(3)
                .And.Equal(odcmEnumMemberDict);
        }
    }
}
