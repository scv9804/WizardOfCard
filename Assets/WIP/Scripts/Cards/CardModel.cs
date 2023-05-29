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

        [Header("강화 횟수")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, Card.MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Aggresive

        //[Header("데미지")]
        //[SerializeField, JsonIgnore] private int _damage;

        // =========================================================================== Defensive

        //[Header("쉴드")]
        //[SerializeField, JsonIgnore] private int _shield;

        // =========================================================================== Power

        [Header("공격력")]
        [SerializeField, JsonIgnore] private int _attackPower;

        [Header("방어력")]
        [SerializeField, JsonIgnore] private int _defensePower;

        [Header("최종 위력")]
        [SerializeField, JsonIgnore] private int _modifier = 1;

        // =========================================================================== Buff

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

        // =========================================================================== Aggresive

        //[JsonIgnore] public int Damage
        //{
        //    get
        //    {
        //        return _damage;
        //    }

        //    set
        //    {
        //        _damage = value;
        //    }
        //}

        // =========================================================================== Defensive

        //[JsonIgnore] public int Shield
        //{
        //    get
        //    {
        //        return _shield;
        //    }

        //    set
        //    {
        //        _shield = value;
        //    }
        //}

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

        [JsonIgnore] public int Modifier
        {
            get
            {
                return _modifier;
            }

            set
            {
                _modifier = value;
            }
        }

        // =========================================================================== Buff

        //
    }

    // ==================================================================================================== CardKeyword

    [Flags] public enum CardKeyword
    {
        None    = 0,

        Exile   = 1 << 0
    }
}

// 타겟 시 범위 설정

// CardTargetAreaDrawer

// 1. 범위 넓이 = n
// 2. 범위 높이 = m

// Tile? input = 마우스 올라가있는 타일 정보

// 그냥 다 list에 저장?

// bool isStartPlayerPostion >> 설정 시 플레이어의 위치에서 출발

// 방향

// bool isEast(temp) = return mouse.position.x > player.position.x
// bool isNorth(temp) = return mouse.position.y > player.position.y