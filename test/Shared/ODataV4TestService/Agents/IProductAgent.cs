// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using ODataV4TestService.Models;

namespace ODataV4TestService.Agents
{
    public interface IProductAgent
    {
        void AddRating(ProductRating productRating);
        Product GetBest();
        double GetSalesTaxRate(int postalCode);
        IQueryable<Product> RelatedProducts();
    }
}