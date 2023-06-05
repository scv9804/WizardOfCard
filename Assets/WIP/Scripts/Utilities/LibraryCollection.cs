using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== Library

    [Serializable] public class Library<TKey, TValue> : ICollection<KeyValueData<TKey, TValue>>, IEnumerable<KeyValueData<TKey, TValue>>, ICollection, IEnumerable
    {
        // ==================================================================================================== Field

        // =========================================================================== ??????????

        [SerializeField] private KeyCollection _keys = new KeyCollection();

        [SerializeField] private ValueCollection _values = new ValueCollection();

        // ==================================================================================================== Property

        // =========================================================================== ??????????

        public KeyCollection Keys
        {
            get
            {
                return _keys;
            }
        }

        public ValueCollection Values
        {
            get
            {
                return _values;
            }
        }

        // =========================================================================== ??????????

        public int Count
        {
            get
            {
                return Keys.Count;
            }
        }

        bool ICollection<KeyValueData<TKey, TValue>>.IsReadOnly => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        // ==================================================================================================== Method

        // =========================================================================== ??????????

        void ICollection<KeyValueData<TKey, TValue>>.Add(KeyValueData<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null || ContainKeys(key))
            {
                return;
            }

            try
            {
                //Keys.Add(key);

                //Values.Add(value);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void Clear()
        {
            //Keys.Clear();

            //Values.Clear();
        }

        bool ICollection<KeyValueData<TKey, TValue>>.Contains(KeyValueData<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainKeys(TKey item)
        {
            return Keys.Contains(item);
        }

        public bool ContainValues(TValue item)
        {
            return Values.Contains(item);
        }

        void ICollection<KeyValueData<TKey, TValue>>.CopyTo(KeyValueData<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValueData<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValueData<TKey, TValue>> IEnumerable<KeyValueData<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator();
        }

        // =========================================================================== Enumerator

        public struct Enumerator : IEnumerator<KeyValueData<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
        {
            public KeyValueData<TKey, TValue> Current => throw new NotImplementedException();

            DictionaryEntry IDictionaryEnumerator.Entry => throw new NotImplementedException();

            object IDictionaryEnumerator.Key => throw new NotImplementedException();

            object IDictionaryEnumerator.Value => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose() { }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }

        // =========================================================================== KeyCollection

        [Serializable] public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            // ==================================================================================================== Field

            // =========================================================================== ??????????

            [SerializeField] private TKey[] _keys;

            // ==================================================================================================== Property

            // =========================================================================== ??????????

            public int Count => throw new NotImplementedException();

            bool ICollection<TKey>.IsReadOnly => throw new NotImplementedException();

            bool ICollection.IsSynchronized => throw new NotImplementedException();

            object ICollection.SyncRoot => throw new NotImplementedException();

            // ==================================================================================================== Method

            // =========================================================================== ??????????

            void ICollection<TKey>.Add(TKey item)
            {
                throw new NotImplementedException();
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TKey item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public bool Remove(TKey item)
            {
                throw new NotImplementedException();
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator();
            }

            // =========================================================================== Enumerator

            public struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
            {
                TKey IEnumerator<TKey>.Current => throw new NotImplementedException();

                object IEnumerator.Current => throw new NotImplementedException();

                public void Dispose() { }

                public bool MoveNext()
                {
                    throw new NotImplementedException();
                }

                void IEnumerator.Reset()
                {
                    throw new NotImplementedException();
                }
            }
        }

        // =========================================================================== ValueCollection

        [Serializable] public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {    
            // ==================================================================================================== Field

            // =========================================================================== ??????????

            [SerializeField] private TValue[] _values;

            // ==================================================================================================== Property

            // =========================================================================== ??????????

            public int Count => throw new NotImplementedException();

            bool ICollection<TValue>.IsReadOnly => throw new NotImplementedException();

            bool ICollection.IsSynchronized => throw new NotImplementedException();

            object ICollection.SyncRoot => throw new NotImplementedException();

            // ==================================================================================================== Method

            // =========================================================================== ??????????

            public void Add(TValue item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TValue item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public bool Remove(TValue item)
            {
                throw new NotImplementedException();
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator();
            }

            // =========================================================================== Enumerator

            public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
            {
                public TValue Current => throw new NotImplementedException();

                object IEnumerator.Current => throw new NotImplementedException();

                public void Dispose() { }

                public bool MoveNext()
                {
                    throw new NotImplementedException();
                }

                void IEnumerator.Reset()
                {
                    throw new NotImplementedException();
                }
            }
        }
    }

    // ==================================================================================================== KeyValueData

    [Serializable] public struct KeyValueData<TKey, TValue>
    {
        // ==================================================================================================== Field

        // =========================================================================== ??????????

        private TKey _key;

        private TValue _value;

        // ==================================================================================================== Property

        // =========================================================================== ??????????

        public TKey Key
        {
            get
            {
                return _key;
            }
        }

        public TValue Value
        {
            get
            {
                return _value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        public KeyValueData(TKey key, TValue value)
        {
            _key = key;

            _value = value;
        }

        // =========================================================================== ??????????

        public override string ToString()
        {
            return $"[{Key}, {Value}]";
        }
    }
}

// ==================================================================================================== ??????????

// =========================================================================== ??????????