using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace WIP
{
    // ==================================================================================================== CardModel

    [Serializable] public class CardModel
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        [Header("�̸�")]
        [SerializeField, JsonIgnore] private string _name;

        [Header("���")]
        [SerializeField, JsonIgnore] private int _cost;

        [Header("Ű����")]
        [SerializeField, JsonIgnore] private CardKeyword _keyword;

        [Header("����")]
        [SerializeField, JsonIgnore, TextArea(3, 5)] private string _description;

        // =========================================================================== Upgrade

        // Savable
        [Header("��ȭ Ƚ��")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, Card.MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Power

        [Header("���ݷ�")]
        [SerializeField, JsonIgnore] private int _attackPower;

        [Header("����")]
        [SerializeField, JsonIgnore] private int _defensePower;

        [Header("����")]
        [SerializeField, JsonIgnore] private int _enhancePower = 1;

        // =========================================================================== Skill

        //

        // =========================================================================== Buff

        // Savable
        //

        // ==================================================================================================== Property

        // =========================================================================== Status

        [JsonIgnore] public string Name
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

        [JsonIgnore] public int Cost
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

        [JsonIgnore] public CardKeyword Keyword
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

        [JsonIgnore] public string Description
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

        [JsonIgnore] public int Upgraded
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

        // =========================================================================== Power

        [JsonIgnore] public int AttackPower
        {
            get
            {
                return _attackPower;
            }

            set
            {
                _attackPower = value;
            }
        }

        [JsonIgnore] public int DefensePower
        {
            get
            {
                return _defensePower;
            }

            set
            {
                _defensePower = value;
            }
        }

        [JsonIgnore] public int EnhancePower
        {
            get
            {
                return _enhancePower;
            }

            set
            {
                _enhancePower = value;
            }
        }

        // =========================================================================== Skill

        //

        // =========================================================================== Buff

        //

        // ==================================================================================================== Method

        // =========================================================================== Power

        public int ApplyAttackPower(int value)
        {
            return ApplyEnhancePower(value + AttackPower);
        }

        public int ApplyDefensePower(int value)
        {
            return ApplyEnhancePower(value + DefensePower);
        }

        public int ApplyEnhancePower(int value)
        {
            return value * Math.Max(EnhancePower, 1);
        }
    }

    // ==================================================================================================== CardKeyword

    [Flags] public enum CardKeyword
    {
        None    = 0,

        Exile   = 1 << 0
    }
}