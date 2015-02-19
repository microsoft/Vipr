// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IPagedCollection
    {
        IReadOnlyList<object> CurrentPage { get; }

        bool MorePagesAvailable { get; }

        Task<IPagedCollection> GetNextPageAsync();
    }

    public interface IPagedCollection<TElement>
    {
        IReadOnlyList<TElement> CurrentPage { get; }

        bool MorePagesAvailable { get; }

        Task<IPagedCollection<TElement>> GetNextPageAsync();
    }
}
