// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions;
using System.Collections.Generic;
using Microsoft.Spatial;

namespace ProxyExtensionsUnitTests
{
    [LowerCaseProperty]
    public interface IProduct : IEntityBase
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        string Category { get; set; }
        Description Comment { get; set; }
    }

    [LowerCaseProperty]
    public interface ISuppliedProduct : IProduct
    {
        int? SupplierId { get; set; }
        Supplier Supplier { get; set; }
    }

    [LowerCaseProperty]
    public interface ISupplier : IEntityBase
    {
        int Id
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        ICollection<SuppliedProduct> Products
        {
            get;
        }

        byte[] Blob
        {
            get;
            set;
        }

        GeographyPoint Location
        {
            get;
            set;
        }
    }

    //[HasStream]
    [global::Microsoft.OData.Client.Key("Id")]
    public class Product : EntityBase, IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public Description Comment { get; set; }
    }

    public class Description : ComplexTypeBase
    {
        public string Title { get; set; }
        public string Details { get; set; }
    }

    [global::Microsoft.OData.Client.Key("Id")]
    public class SuppliedProduct : Product, ISuppliedProduct
    {
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public new string GetPath(string propertyName)
        {
            return base.GetPath(propertyName);
        }
    }

    [global::Microsoft.OData.Client.Key("Id")]
    public class Supplier : EntityBase, ISupplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SuppliedProduct> Products { get; set; }
        public byte[] Blob { get; set; }
        public GeographyPoint Location { get; set; }

        public new string GetPath(string propertyName)
        {
            return base.GetPath(propertyName);
        }
    }

    public class QueryableSetOfIProduct : QueryableSet<IProduct>
    {
        public QueryableSetOfIProduct(DataServiceQuery inner, DataServiceContextWrapper context, EntityBase entity, string path) : base(inner, context, entity, path)
        {
        }

        public string GetPath(Expression<Func<Product, bool>> whereExpression)
        {
            return base.GetPath(whereExpression);
        }
    }

    public class TestRestShallowObjectFetcher : RestShallowObjectFetcher
    {
        public new IReadOnlyQueryableSet<TIInstance> CreateQuery<TInstance, TIInstance>() 
            where TInstance:EntityBase, TIInstance
        {
            return base.CreateQuery<TInstance, TIInstance>();
        }

        public new string GetPath(string propertyName)
        {
            return base.GetPath(propertyName);
        }

        public new Uri GetUrl()
        {
            return base.GetUrl();
        }
    }
}
