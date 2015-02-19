// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using CSharpWriter;
using FluentAssertions;
using Microsoft.Its.Recipes;
using System.Linq;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Type = System.Type;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Derived : EntityTestBase
    {
        private OdcmClass _baseClass;
        private Type _baseConcreteType;
        private Type _baseConcreteInterface;
        private Type _baseFetcherType;
        private Type _baseFetcherInterface;
        private Type _baseCollectionType;
        private Type _baseCollectionInterface;
        private string _toDerivedMethodName;

        
        public Given_an_OdcmClass_Entity_Derived()
        {
            base.Init(m =>
            {
                var @namespace = m.Namespaces[0];
                var derivedClass = @namespace.Classes.First();
                _baseClass = Any.EntityOdcmClass(@namespace);
                @namespace.Types.Add(_baseClass);
                derivedClass.Base = _baseClass;
                if (!_baseClass.Derived.Contains(derivedClass))
                {
                    _baseClass.Derived.Add(derivedClass);
                }
            });

            _baseConcreteType = Proxy.GetClass(_baseClass.Namespace, _baseClass.Name);

            _baseConcreteInterface = Proxy.GetInterface(_baseClass.Namespace, "I" + _baseClass.Name);

            _baseFetcherType = Proxy.GetClass(_baseClass.Namespace, _baseClass.Name + "Fetcher");

            _baseFetcherInterface = Proxy.GetInterface(_baseClass.Namespace, "I" + _baseClass.Name + "Fetcher");

            _baseCollectionType = Proxy.GetClass(_baseClass.Namespace, _baseClass.Name + "Collection");

            _baseCollectionInterface = Proxy.GetInterface(_baseClass.Namespace, "I" + _baseClass.Name + "Collection");

            _toDerivedMethodName = "To" + ConcreteType.Name;
        }

        [Fact]
        public void The_Concrete_interface_implements_Base_Concrete_interface()
        {
            ConcreteType.Should().Implement(_baseConcreteInterface);
        }

        [Fact]
        public void The_Concrete_type_derives_from_Base_Concrete_type()
        {
            ConcreteType.Should().BeDerivedFrom(_baseConcreteType);
        }

        [Fact]
        public void The_Fetcher_interface_implements_Base_Fetcher_interface()
        {
            FetcherInterface.Should().Implement(_baseFetcherInterface);
        }

        [Fact]
        public void The_Fetcher_class_derives_from_Base_Fetcher_class()
        {
            FetcherType.Should().BeDerivedFrom(_baseFetcherType);
        }

        [Fact]
        public void The_Collection_interface_exposes_a_GetById_method_for_full_key()
        {
            CollectionInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                FetcherInterface,
                "GetById",
                GetKeyTypes(),
                "Because it allows retrieving an instance by key");
        }

        [Fact]
        public void The_Collection_interface_exposes_a_GetById_Indexer_for_full_key()
        {
            CollectionInterface.Should().HaveIndexer(
                CSharpAccessModifiers.Public,
                null,
                FetcherInterface,
                GetKeyTypes(),
                "Because it allows retrieving an instance by key");
        }


        [Fact]
        public void The_Base_Fetcher_interface_exposes_a_ToDerived_method()
        {
            _baseFetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                FetcherInterface,
                _toDerivedMethodName,
                new Type[0],
                "Because the base fetcher interface should expose the ToDerived method");
        }

        [Fact]
        public void The_Base_Fetcher_implements_a_ToDerived_method()
        {
            _baseFetcherInterface.Should().HaveMethod(
                CSharpAccessModifiers.Public,
                FetcherInterface,
                _toDerivedMethodName,
                new Type[0],
                "Because the base fetcher type should expose the ToDerived method");
        }

        [Fact]
        public void The_Base_Concrete_explicitly_implements_a_ToDerived_method()
        {
            _baseConcreteType.Should().HaveExplicitMethod(
                _baseFetcherInterface,
                _toDerivedMethodName,
                FetcherInterface, new Type[0],
                "Because the base concrete type should expose the ToDerived method");
        }

        private IEnumerable<Type> GetKeyTypes()
        {
            return Class.Key
                .Select(p => p.Type)
                .Select(t => Proxy.GetClass(t.Namespace, t.Name));
        }
    }
}
