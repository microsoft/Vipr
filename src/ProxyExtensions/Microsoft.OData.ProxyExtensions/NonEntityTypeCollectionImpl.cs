using System;

namespace Microsoft.OData.ProxyExtensions
{
    public class NonEntityTypeCollectionImpl<T> : global::System.Collections.ObjectModel.Collection<T>
    {
        private Func<Tuple<EntityBase, string>> _entity;

        private static readonly bool s_isComplexType = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T)).IsSubclassOf(typeof(ComplexTypeBase));

        public NonEntityTypeCollectionImpl()
            : base()
        {
        }

        public void SetContainer(Func<Tuple<EntityBase, string>> entity)
        {
            _entity = entity;

            if (s_isComplexType)
            {
                foreach (var i in this)
                {
                    (i as ComplexTypeBase).SetContainer(entity);
                }
            }
        }

        protected override void InsertItem(int index, T item)
        {
            var ct = item as ComplexTypeBase;
            if (ct != null)
            {
                ct.SetContainer(_entity);
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
            var ct = item as ComplexTypeBase;
            if (ct != null)
            {
                ct.SetContainer(_entity);
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