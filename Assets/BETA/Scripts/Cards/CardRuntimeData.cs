using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== CardRuntimeData

    [Serializable]
    public sealed class CardRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== General

        [FoldoutGroup("�Ϲ� ������")]
        public Enums.CardType Type;

        [FoldoutGroup("�Ϲ� ������")]
        public int Level;

        [FoldoutGroup("�Ϲ� ������")]
        public CardKeyword Keyword;

        [FoldoutGroup("�Ϲ� ������")]
        public int Cost;

        [FoldoutGroup("�Ϲ� ������")] [MultiLineProperty(5)]
        public string Description;

        // =========================================================================== Ability

        //[FoldoutGroup("ȿ�� ������")]
        //public int Damage;

        //[FoldoutGroup("ȿ�� ������")]
        //public int Shield;

        //[FoldoutGroup("ȿ�� ������")]
        //public int Heal;

        //[FoldoutGroup("ȿ�� ������")]
        //public int Draw;

        //[FoldoutGroup("ȿ�� ������")]
        //public int Count;

        //

        //

        private CardRuntimeData() : base() { }

        public CardRuntimeData(string instanceID, int serialID) : base(instanceID, serialID) { } 

        //public override void Refresh(CardScriptableData data)
        //{
        //    base.Refresh(data);
        //}
    }
}
