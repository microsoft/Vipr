// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IReadOnlyQueryableSet<TSource> : IReadOnlyQueryableSetBase<TSource>
    {
        Task<IPagedCollection<TSource>> ExecuteAsync();

        Task<TSource> ExecuteSingleAsync();
    }
}
