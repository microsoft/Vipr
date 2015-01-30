// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Spatial;
using ODataV4TestService.Models;
using System;
using System.Linq;

namespace Microsoft.Its.Recipes
{
    internal static partial class Any
    {
        public static Supplier Supplier(Action<Supplier> config = null)
        {
            var retVal = new Supplier
            {
                Id = Any.Int(),
                Name = Any.CompanyName(),
                Blob = Any.Sequence<byte>(x => Any.Byte()).ToArray(),
                Location = GeographyPoint.Create(Any.Double(-90, 90), Any.Double(-180, 180))
            };

            retVal.Products = Any.Sequence(_ => Any.SuppliedProduct(p =>
            {
                p.SupplierId = retVal.Id;
                p.Supplier = retVal;
            })).ToList();

            if (config != null) config(retVal);

            return retVal;
        }

        public static IQueryable<Supplier> Suppliers(int count = 5)
        {
            return Any.Sequence(x => Any.Supplier(s =>
            {
                s.Id = x;
                s.Products = Any.Sequence(y => Any.SuppliedProduct(p =>
                {
                    p.Id = x * 100 + y;
                    p.SupplierId = x;
                    p.Supplier = s;
                })).ToList();
            }), count).ToList().AsQueryable();
        }

        public static SuppliedProduct SuppliedProduct(Action<SuppliedProduct> config = null)
        {
            var retVal = new SuppliedProduct
            {
                Id = Any.Int(),
                Category = Any.AlphanumericString(),
                Price = Any.Decimal(),
                Name = Any.CompanyName()
            };

            if (config != null) config(retVal);

            return retVal;
        }
        public static Product Product(Action<Product> config = null)
        {
            var retVal = new Product
            {
                Id = Any.Int(),
                Category = Any.AlphanumericString(),
                Price = Any.Decimal(),
                Name = Any.CompanyName(),
                Comment = Any.Description()
            };

            if (config != null) config(retVal);

            return retVal;
        }

        public static IQueryable<Product> Products(int count = 5)
        {
            return Any.Sequence(x => Any.Product(p => p.Id = x), count).AsQueryable();
        }

        public static Description Description(Action<Description> config = null)
        {
            var retVal = new Description
            {
                Title = Any.AlphanumericString(),
                Details = Any.AlphanumericString()
            };

            if (config != null) config(retVal);

            return retVal;
        }

        public static Account Account(Action<Account> config = null)
        {
            var retVal = new Account
            {
                AccountId = Any.Int(),
                Name = Any.FullName(),
                PayinPIs = Any.Sequence(x => Any.PaymentInstrument()).ToList()
            };

            if (config != null)
                config(retVal);

            return retVal;
        }

        public static PaymentInstrument PaymentInstrument(Action<PaymentInstrument> config = null)
        {
            var retVal = new PaymentInstrument
            {
                FriendlyName = Any.CompanyName(),
                PaymentInstrumentId = Any.Int()
            };

            if (config != null) config(retVal);

            return retVal;
        }
    }
}