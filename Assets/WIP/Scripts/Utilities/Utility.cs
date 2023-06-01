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

        [SerializeField, JsonProperty("UpdateNotes")] private List<DataUpdateNote<T>> _updateNotes = new List<DataUpdateNote<T>>(); // � ������ a�� ���� �� a�� �� ������ '����'�ϴ� DataUpdateNote ����Ʈ
        // interface�� �ۼ� �� �ε� �ø��� ��ü�� �������� ������ ���� ����
        // ���� Data<T> Ŭ���� ��� �� ���� �� ������ �ϳ��� ����Ʈ�� ��

        // ...�� �ǳ�...?

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