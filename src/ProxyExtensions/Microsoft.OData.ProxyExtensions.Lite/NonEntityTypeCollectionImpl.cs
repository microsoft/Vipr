// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Microsoft.OData.ProxyExtensions.Lite
{
    public class NonEntityTypeCollectionImpl<T> : Collection<T>
    {
        private Func<Tuple<EntityBase, string>> _entity;

        private static readonly bool GenericArgumentIsComplexType =
            typeof (T).GetTypeInfo().IsSubclassOf(typeof (ComplexTypeBase));

        public void SetContainer(Func<Tuple<EntityBase, string>> entity)
        {
            _entity = entity;

            if (GenericArgumentIsComplexType)
            {
                foreach (var i in this)
                {
                    (i as ComplexTypeBase).SetContainer(entity);
                }
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (item != null && GenericArgumentIsComplexType)
            {
                (item as ComplexTypeBase).SetContainer(_entity);
            }

            base.InsertItem(index, item);

            InvokeOnPropertyChanged();
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            InvokeOnPropertyChanged();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            InvokeOnPropertyChanged();
        }

        protected override void SetItem(int index, T item)
        {
            if (item != null && GenericArgumentIsComplexType)
            {
                (item as ComplexTypeBase).SetContainer(_entity);
            }

            base.SetItem(index, item);

            InvokeOnPropertyChanged();
        }

        private void InvokeOnPropertyChanged()
        {
            var tuple = _entity != null ? _entity() : null;

            if (tuple != null)
            {
                tuple.Item1.OnPropertyChanged(tuple.Item2);
            }
        }
    }
}
