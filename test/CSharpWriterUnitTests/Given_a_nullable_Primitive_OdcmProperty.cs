// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Microsoft.Its.Recipes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Vipr.Core.CodeModel;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_a_nullable_Primitive_OdcmProperty : CodeGenTestBase
    {
        private OdcmModel _model;
        private OdcmNamespace _namespace;
        private OdcmClass _class;
        private OdcmProperty _property;
        private Assembly _proxy;
        private Type _classType;

        
        public Given_a_nullable_Primitive_OdcmProperty()
        {
            _model = new OdcmModel(Any.ServiceMetadata());

            _namespace = Any.EmptyOdcmNamespace();

            _model.Namespaces.Add(_namespace);

            _class = Any.EntityOdcmClass(_namespace);

            _property = Any.PrimitiveOdcmProperty(p => p.IsNullable = true);

            _class.Properties.Add(_property);

            _model.AddType(_class);
        }

        [Fact]
        public void When_the_property_is_Boolean_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Boolean");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Boolean>));
        }

        [Fact]
        public void When_the_property_is_Byte_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Byte");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Byte>));
        }

        [Fact]
        public void When_the_property_is_Date_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Date");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<DateTimeOffset>));
        }

        [Fact]
        public void When_the_property_is_DateTimeOffset_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("DateTimeOffset");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<DateTimeOffset>));
        }

        [Fact]
        public void When_the_property_is_Decimal_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Decimal");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Decimal>));
        }

        [Fact]
        public void When_the_property_is_Duration_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Duration");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<TimeSpan>));
        }

        [Fact]
        public void When_the_property_is_Int16_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Int16");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Int16>));
        }

        [Fact]
        public void When_the_property_is_Int32_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Int32");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Int32>));
        }

        [Fact]
        public void When_the_property_is_Int64_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Int64");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Int64>));
        }

        [Fact]
        public void When_the_property_is_SByte_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("SByte");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<SByte>));
        }

        [Fact]
        public void When_the_property_is_TimeOfDay_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("TimeOfDay");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<DateTimeOffset>));
        }

        [Fact]
        public void When_the_property_is_Guid_it_is_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("Guid");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(Nullable<Guid>));
        }

        [Fact]
        public void When_the_property_is_Enum_it_is_NOT_exposed_as_Nullable()
        {
            var @enum = Any.OdcmEnum(e => e.Namespace = _namespace.Name);
            _model.AddType(@enum);
            _property.Type = @enum;
            _proxy = GetProxy(_model);
            _classType = _proxy.GetClass(_class.Namespace, _class.Name);
            var _enumType = _proxy.GetEnum(@enum.Namespace, @enum.Name);

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType.Name
                .Should().Be(_enumType.Name);

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType.Namespace
                .Should().Be(_enumType.Namespace);
        }

        [Fact]
        public void When_the_property_is_String_it_is_NOT_exposed_as_Nullable()
        {
            CreateProxyWithPropertyType("String");

            // When the primitive property is a reference type (eg. Edm.String, Edm.Stream) 
            // then the proxy property must not be nullable
            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(String));
        }

        [Fact]
        public void When_the_property_is_Collection_of_Primitive_it_is_NOT_exposed_as_Collection_of_Nullable()
        {
            _property.IsCollection = true;

            CreateProxyWithPropertyType("Int32");

            _classType.GetProperty(_property.Name, PermissiveBindingFlags).PropertyType
                .Should().Be(typeof(IList<Int32>));
        }

        private void CreateProxyWithPropertyType(string type)
        {
            _property.Type = new OdcmPrimitiveType(type, "Edm");

            _proxy = GetProxy(_model);

            _classType = _proxy.GetClass(_class.Namespace, _class.Name);
        }
    }
}
