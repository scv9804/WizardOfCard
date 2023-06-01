using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace WIP
{
    [Serializable] public class CardModel
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("�ν��Ͻ� ID")]
        [SerializeField, JsonProperty("InstanceID")] private string _instanceID;

        [Header("�ø��� ID")]
        [SerializeField, JsonProperty("SerialID")] private int _serialID;

        // =========================================================================== Status

        [Header("�̸�")]
        [SerializeField, JsonProperty("Name")] private Data<string> _name = new Data<string>();

        [Header("���")]
        [SerializeField, JsonProperty("Cost")] private Data<int> _cost = new Data<int>();

        [Header("Ű����")]
        [SerializeField, JsonProperty("Name")] private Data<CardKeyword> _keyword = new Data<CardKeyword>();

        [Header("����")]
        [SerializeField, JsonProperty("Cost")] private Data<string> _description = new Data<string>();

        // =========================================================================== Upgrade

        [Header("��ȭ Ƚ��")]
        [SerializeField, JsonProperty("Upgraded")] private Data<int> _upgraded = new Data<int>();

        // =========================================================================== Ability

        // ================================================== Data

        [Header("������")]
        [SerializeField, JsonProperty("Damage")] private List<Data<int>> _damage = new List<Data<int>>();

        [Header("����")]
        [SerializeField, JsonProperty("Shield")] private List<Data<int>> _shield = new List<Data<int>>();

        // ================================================== System

        [Header("�ݺ� Ƚ��")]
        [SerializeField, JsonProperty("Count")] private List<Data<int>> _count = new List<Data<int>>();

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        [JsonIgnore] public string InstanceID
        {
            get
            {
                return _instanceID;
            }

            set
            {
                _instanceID = value;
            }
        }

        [JsonIgnore] public int SerialID
        {
            get
            {
                return _serialID;
            }

            set
            {
                _serialID = value;
            }
        }

        // =========================================================================== Status

        [JsonIgnore] public Data<string> Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        [JsonIgnore] public Data<int> Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                _cost = value;
            }
        }

        [JsonIgnore] public Data<CardKeyword> Keyword
        {
            get
            {
                return _keyword;
            }

            set
            {
                _keyword = value;
            }
        }

        [JsonIgnore] public Data<string> Decription
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        // =========================================================================== Upgrade

        [JsonIgnore] public Data<int> Upgraded
        {
            get
            {
                return _upgraded;
            }

            set
            {
                _upgraded = value;
            }
        }
    }
}
