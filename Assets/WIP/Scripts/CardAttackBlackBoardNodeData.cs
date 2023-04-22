using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;

namespace WIP
{
    // ==================================================================================================== CardAttackBlackBoardNodeData

    [CreateAssetMenu(menuName = "WIP/Card/BlackBoard/Attack", fileName = "_AttackBlackBoard")]
    public class CardAttackBlackBoardNodeData : CardBlackBoardNodeData
    {
        // ==================================================================================================== Field

        // =========================================================================== BlackBoard

        // ================================================== Attack

        [Header("单固瘤")]
        [SerializeField] private List<int> _damage = new List<int>(Card.MAX_UPGRADE_LEVEL + 1);

        [Header("单固瘤 可记")]
        [SerializeField] private List<CardDamageApplyOption> _damageOption = new List<CardDamageApplyOption>(Card.MAX_UPGRADE_LEVEL + 1);

        // ==================================================================================================== Property

        // =========================================================================== BlackBoard

        // ================================================== Attack

        public ReadOnlyCollection<int> Damage
        {
            get
            {
                return _damage.AsReadOnly();
            }
        }

        public ReadOnlyCollection<CardDamageApplyOption> DamageOption
        {
            get
            {
                return _damageOption.AsReadOnly();
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== BlackBoard

        // ================================================== Base

        public override void Create()
        {
            throw new System.NotImplementedException();
        }
    }

    // ==================================================================================================== CardAttackBlackBoardNode

    public class CardAttackBlackBoardNode : CardBlackBoardNode
    {
        // ==================================================================================================== Field

        // =========================================================================== BlackBoard

        // ================================================== Attack

        public List<int> Damage = new List<int>();

        public List<CardDamageApplyOption> DamageOption = new List<CardDamageApplyOption>();

        public List<int> AttackPower = new List<int>();
        public List<int> EnhancePower = new List<int>();
    }

    // ==================================================================================================== CardDamageOption

    [Flags] public enum CardDamageApplyOption
    {
        None            = 0,

        // ================================================== Power

        AttackPower     = 1 << 0,

        EnhancePower    = 1 << 1,

        // ================================================== Effect

        Shield          = 1 << 2
    }
}
