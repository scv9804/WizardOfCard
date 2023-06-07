using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace WIP
{
    // ==================================================================================================== Utility

    public static class Utility
    {

    }

    // ==================================================================================================== Data

    [Serializable] public class Data<T> : IData<T>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [Header("데이터 값")]
        [SerializeField, JsonProperty("Value")] private T _value;

        // =========================================================================== Observer

        public event EventObserver OnChange;

        // ==================================================================================================== Property

        // =========================================================================== Data

        [JsonIgnore] public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;

                OnChange?.Invoke(this);
            }
        }
    }

    // ==================================================================================================== IData

    public interface IData<T> : IEventParameter
    {
        // ==================================================================================================== Property

        // =========================================================================== Data

        public T Value
        {
            get;
        }
    }
}