// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public class ReadOnlyQueryableSet<TSource> : ReadOnlyQueryableSetBase<TSource>, IReadOnlyQueryableSet<TSource>
    {
        public ReadOnlyQueryableSet(DataServiceQuery inner, DataServiceContextWrapper context)
            : base(inner, context)
        {
        }
        
        public Task<IPagedCollection<TSource>> ExecuteAsync()
        {
            return ExecuteAsyncInternal();
        }

        public Task<TSource> ExecuteSingleAsync()
        {
            return ExecuteSingleAsyncInternal();
        }
    }
}
