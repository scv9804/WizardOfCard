using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== Library

    public class Library<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        // ==================================================================================================== Property

        // =========================================================================== Indexer

        public TValue this[TKey key, int index]
        {
            get
            {
                return this[key][index];
            }

            set
            {
                this[key][index] = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Collection

        public void Add(TKey key)
        {
            Add(key, new List<TValue>());
        }

        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                Add(key);
            }

            this[key].Add(value);
        }

        public bool Remove(TKey key, TValue value, bool isEmptyCategoryDelete = true)
        {
            var result = this[key].Remove(value);

            if (this[key].Count == 0 && isEmptyCategoryDelete)
            {
                Remove(key);
            }

            return result;
        }

        // =========================================================================== Library

        public int GetLibraryCount(TKey key)
        {
            return ContainsKey(key) ? this[key].Count : 0;
        }
    }
}
