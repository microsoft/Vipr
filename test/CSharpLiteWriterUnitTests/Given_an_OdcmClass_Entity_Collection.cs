// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using System.Linq;
using Xunit;

namespace CSharpLiteWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection : EntityTestBase
    {
        
        public Given_an_OdcmClass_Entity_Collection()
        {
            base.Init();
        }

        [Fact]
        public void The_Collection_class_is_Internal()
        {
            CollectionType.IsInternal()
                .Should().BeTrue("Because entity types are accessed by the Concrete, Fetcher, " +
                                 "and Collection interfaces they implement.");
        }

        [Fact]
        public void The_Collection_class_derives_from_QueryableSetofT()
        {
            CollectionType
                .Should().BeDerivedFrom(typeof(QueryableSet<>).MakeGenericType(ConcreteInterface),
                                        "Because it is a QueryableSet<IConcreteType> which empowers it" +
                                        "with Linq expressions like Where, OrderBy, Expand, Select," +
                                        "Skip, and Take.");
        }

        [Fact]
        public void The_Collection_class_exposes_a_Collection_constructor()
        {
            CollectionType.Should().HaveConstructor(CSharpAccessModifiers.Internal, CollectionConstructorParameterTypes,
                    "Because these are the parameters required for the QueryableBase constructor.");
        }

        [Fact]
        public void The_Collection_class_implements_the_Collection_Interface()
        {
            CollectionType.Should()
                .Implement(CollectionInterface,
                    "Because the implementaiton is internal and only accessible via the interface.");
        }

        private static Type[] CollectionConstructorParameterTypes
        {
            get
            {
                return new Type[]
                {
                    typeof (DataServiceQuery),
                    typeof(DataServiceContextWrapper),
                    typeof (object),
                    typeof (string)
                };
            }
        }
    }
}
