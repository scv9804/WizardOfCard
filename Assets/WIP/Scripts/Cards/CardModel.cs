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

        [Header("��ȭ Ƚ��")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, Card.MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Aggresive

        //[Header("������")]
        //[SerializeField, JsonIgnore] private int _damage;

        // =========================================================================== Defensive

        //[Header("����")]
        //[SerializeField, JsonIgnore] private int _shield;

        // =========================================================================== Power

        [Header("���ݷ�")]
        [SerializeField, JsonIgnore] private int _attackPower;

        [Header("����")]
        [SerializeField, JsonIgnore] private int _defensePower;

        [Header("���� ����")]
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

// Ÿ�� �� ���� ����

// CardTargetAreaDrawer

// 1. ���� ���� = n
// 2. ���� ���� = m

// Tile? input = ���콺 �ö��ִ� Ÿ�� ����

// �׳� �� list�� ����?

// bool isStartPlayerPostion >> ���� �� �÷��̾��� ��ġ���� ���

// ����

// bool isEast(temp) = return mouse.position.x > player.position.x
// bool isNorth(temp) = return mouse.position.y > player.position.y