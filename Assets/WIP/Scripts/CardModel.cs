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

        [Header("이름")]
        [SerializeField, JsonIgnore] private string _name;

        [Header("비용")]
        [SerializeField, JsonIgnore] private int _cost;

        [Header("키워드")]
        [SerializeField, JsonIgnore] private CardKeyword _keyword;

        [Header("설명")]
        [SerializeField, JsonIgnore, TextArea(3, 5)] private string _description;

        // =========================================================================== Upgrade

        // Savable
        [Header("강화 횟수")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, Card.MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Power

        [Header("공격력")]
        [SerializeField, JsonIgnore] private int _attackPower;

        [Header("방어력")]
        [SerializeField, JsonIgnore] private int _defensePower;

        [Header("위력")]
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
