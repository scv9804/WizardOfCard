using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== SerializedListObject

    public class SerializedListObject<T, TData> : ScriptableObject where TData : SerializedListData<T>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [Header("데이터")]
        [SerializeField] private TData[] _data;

        // ==================================================================================================== Method

        // =========================================================================== Data

        public List<T> Deserialize()
        {
            var list = new List<T>();

            for (int i = 0; i < _data.Length; i++)
            {
                var data = _data[i].Deserialize();

                list.Add(data);
            }

            return list;
        }
    }

    // ==================================================================================================== SerializedListData

    public abstract class SerializedListData<T>
    {
        // ==================================================================================================== Property

        // =========================================================================== Data

        protected abstract T Value
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Data

        public T Deserialize()
        {
            return Value;
        }
    }
}
