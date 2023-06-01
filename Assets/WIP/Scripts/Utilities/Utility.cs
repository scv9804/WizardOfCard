using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Text;

namespace WIP
{
    // ==================================================================================================== Utility

    public static class Utility
    {
        // ==================================================================================================== Field

        // =========================================================================== StringBuilder

        public static StringBuilder StringBuilder = new StringBuilder();
    }

    // ==================================================================================================== Data

    [Serializable] public class Data<T>
    {
        // ==================================================================================================== Field

        // =========================================================================== Value

        [SerializeField, JsonProperty("Value")] private T _value;

        // =========================================================================== UpdateNote

        [SerializeField, JsonProperty("UpdateNotes")] private List<DataUpdateNote<T>> _updateNotes = new List<DataUpdateNote<T>>(); // 어떤 데이터 a가 있을 때 a의 값 변동에 '관여'하는 DataUpdateNote 리스트
        // interface로 작성 후 로딩 시마다 개체의 버프에서 꺼내올 수도 있음
        // 최초 Data<T> 클래스 사용 시 원본 값 설정용 하나가 리스트에 들어감

        // ...안 되네...?

        // =========================================================================== Observer

        [JsonIgnore] private Action<T> _onChange;

        // ==================================================================================================== Property

        // =========================================================================== Value

        [JsonIgnore] public T Value
        {
            get
            {
                return _value;
            }

            private set
            {
                _value = value;

                _onChange?.Invoke(_value);
            }
        }

        // =========================================================================== UpdateNote

        //[JsonIgnore] public List<DataUpdateNote<T>> UpdateNotes
        //{
        //    get
        //    {
        //        return _updateNotes;
        //    }
        //}

        // =========================================================================== Observer

        public event Action<T> OnChange
        {
            add
            {
                _onChange += value;
            }

            remove
            {
                _onChange -= value;
            }
        }
    }

    [Serializable] public class DataUpdateNote<T>
    {
        // ==================================================================================================== Field

        // =========================================================================== Value

        [SerializeField, JsonProperty("Value")] private T _value;

        // ==================================================================================================== Property

        // =========================================================================== Value

        [JsonIgnore] public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
    }
}