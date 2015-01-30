// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.OData.Client;

namespace Microsoft.OData.ProxyExtensions
{
    public partial class DataServiceContextWrapper
    {
        public DataServiceContextWrapper()
        {
        }

        public virtual new void AddObject(string entitySetName, object entity)
        {
            base.AddObject(entitySetName, entity);
        }

        public virtual new void AddRelatedObject(object source, string sourceProperty, object target)
        {
            base.AddRelatedObject(source, sourceProperty, target);
        }

        public virtual new DataServiceQuery<T> CreateQuery<T>(string path)
        {
            return base.CreateQuery<T>(path);
        }
        // made virtual
        //public partial new virtual Task SaveChangesAsync();
    }
}