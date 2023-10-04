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

        [FoldoutGroup("일반 데이터")]
        public Enums.CardType Type;

        [FoldoutGroup("일반 데이터")]
        public int Level;

        [FoldoutGroup("일반 데이터")]
        public CardKeyword Keyword;

        [FoldoutGroup("일반 데이터")]
        public int Cost;

        [FoldoutGroup("일반 데이터")] [MultiLineProperty(5)]
        public string Description;

        // =========================================================================== Ability

        //[FoldoutGroup("효과 데이터")]
        //public int Damage;

        //[FoldoutGroup("효과 데이터")]
        //public int Shield;

        //[FoldoutGroup("효과 데이터")]
        //public int Heal;

        //[FoldoutGroup("효과 데이터")]
        //public int Draw;

        //[FoldoutGroup("효과 데이터")]
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
