// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class PagedCollection<TElement, TConcrete> : IPagedCollection, IPagedCollection<TElement>
        where TConcrete : TElement
    {
        private readonly DataServiceContextWrapper _context;
        private readonly DataServiceQueryContinuation<TConcrete> _continuation;
        private readonly IReadOnlyList<TElement> _currentPage;

        // Creator - should be faster than Activator.CreateInstance
        public static PagedCollection<TElement, TConcrete> Create(DataServiceContextWrapper context,
            QueryOperationResponse<TConcrete> qor)
        {
            return new PagedCollection<TElement, TConcrete>(context, qor);
        }

        public PagedCollection(DataServiceContextWrapper context,
            QueryOperationResponse<TConcrete> qor)
        {
            _context = context;
            _currentPage = (IReadOnlyList<TElement>) qor.ToList();
            _continuation = qor.GetContinuation();
        }

        public PagedCollection(DataServiceContextWrapper context, DataServiceCollection<TConcrete> collection)
        {
            _context = context;
            _currentPage = (IReadOnlyList<TElement>) collection;
            if (_currentPage != null)
            {
                _continuation = collection.Continuation;
            }
        }

        public bool MorePagesAvailable
        {
            get { return _continuation != null; }
        }

        public IReadOnlyList<TElement> CurrentPage
        {
            get { return _currentPage; }
        }

        public async Task<IPagedCollection<TElement>> GetNextPageAsync()
        {
            if (_continuation != null)
            {
                var task = _context.ExecuteAsync<TConcrete, TElement>(_continuation);

                return new PagedCollection<TElement, TConcrete>(_context, await task);
            }

            return null;
        }

        IReadOnlyList<object> IPagedCollection.CurrentPage
        {
            get { return (IReadOnlyList<object>) CurrentPage; }
        }

        async Task<IPagedCollection> IPagedCollection.GetNextPageAsync()
        {
            var retval = await GetNextPageAsync();

            return (PagedCollection<TElement, TConcrete>) retval;
        }
    }
}
