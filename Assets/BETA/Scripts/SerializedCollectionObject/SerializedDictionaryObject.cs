using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== SerializedDictionaryObject

    public class SerializedDictionaryObject<TKey, TValue, TData> : ScriptableObject where TData : SerializedDictionaryData<TKey, TValue>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [Header("데이터")]
        [SerializeField] private TData[] _data;

        // ==================================================================================================== Method

        // =========================================================================== Data

        public Dictionary<TKey, TValue> Deserialize()
        {
            var dictionary = new Dictionary<TKey, TValue>();

            for (int i = 0; i < _data.Length; i++)
            {
                var data = _data[i].Deserialize();

                dictionary.Add(data.Key, data.Value);
            }

            return dictionary;
        }
    }

    // ==================================================================================================== SerializedDictionaryData

    public abstract class SerializedDictionaryData<TKey, TValue>
    {
        // ==================================================================================================== Property

        // =========================================================================== Data

        protected abstract TKey Key
        {
            get;
        }

        protected abstract TValue Value
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Data

        public KeyValuePair<TKey, TValue> Deserialize()
        {
            return new KeyValuePair<TKey, TValue>(Key, Value);
        }
    }
}
