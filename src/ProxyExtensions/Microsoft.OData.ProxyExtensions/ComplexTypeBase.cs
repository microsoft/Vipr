// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.OData.ProxyExtensions
{
    public class ComplexTypeBase
    {
        private Func<Tuple<EntityBase, string>> _entity;

        protected ComplexTypeBase()
        {
        }

        public virtual void SetContainer(Func<Tuple<EntityBase, string>> entity)
        {
            _entity = entity;
        }

        protected Tuple<EntityBase, string> GetContainingEntity(string propertyName)
        {
            return _entity != null ? _entity() : null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var tuple = GetContainingEntity(propertyName);

            if (tuple != null)
            {
                tuple.Item1.OnPropertyChanged(tuple.Item2);
            }
        }
    }
}
