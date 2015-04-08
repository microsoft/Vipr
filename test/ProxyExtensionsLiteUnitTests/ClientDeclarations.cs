// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.OData.Client;
using Microsoft.OData.ProxyExtensions.Lite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Spatial;

namespace ProxyExtensionsUnitTests
{
    [LowerCaseProperty]
    public interface IProduct
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
    public interface ISupplier
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
    public class Product : TestEntityBase, IProduct
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
    public class Supplier : TestEntityBase, ISupplier
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

    public class TestEntityBase : EntityBase
    {
        public new void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }

    public class TestRestShallowObjectFetcher : RestShallowObjectFetcher
    {
        public static TestRestShallowObjectFetcher CreateFetcher(DataServiceContextWrapper context, EntityBase entity)
        {
            var fetcher = new TestRestShallowObjectFetcher();
            Uri fullUri;
            context.TryGetUri(entity, out fullUri);

            var baseUri = context.BaseUri.ToString().TrimEnd('/');
            var resourcePath = fullUri.ToString().Substring(baseUri.Length + 1);

            fetcher.Initialize(context, resourcePath);
            return fetcher;
        }

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

        public Task UpdateAsync(object item, bool deferSaveChanges = false)
        {
            return base.UpdateAsync(item, deferSaveChanges);
        }

        public Task DeleteAsync(object item, bool deferSaveChanges = false)
        {
            return base.DeleteAsync(item, deferSaveChanges);
        }

        public Task SaveChangesAsync(bool deferSaveChanges = false, SaveChangesOptions saveChangesOption = SaveChangesOptions.None)
        {
            return base.FetcherSaveChangesAsync(deferSaveChanges, saveChangesOption);
        }
    }
}
