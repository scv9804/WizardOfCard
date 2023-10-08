using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== CardScriptableData

    [CreateAssetMenu(menuName = "BETA/Card/Data", fileName = "_CardScriptableData")]
    public sealed class CardScriptableData : ScriptableData
    {
        // ==================================================================================================== Field

        // =========================================================================== General

        [FoldoutGroup("�Ϲ� ������")]
        public Enums.CardType Type;

        [FoldoutGroup("�Ϲ� ������")]
        public CardKeyword[] Keyword = new CardKeyword[3];

        [FoldoutGroup("�Ϲ� ������")]
        public int[] Cost = new int[3];

        [FoldoutGroup("�Ϲ� ������")] [MultiLineProperty(5)]
        public string[] Description = new string[3];
    }
}
