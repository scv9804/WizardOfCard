using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // =========================================================================== CardDataSet

    [CreateAssetMenu(menuName = "BETA/Card/DataSet", fileName = "CardDataSet")]
    public sealed class CardDataSet : DataSet
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Card

        [TableList] [FoldoutGroup("������Ʈ ������")]
        public CardScriptableData[] Data;

        // ================================================== Component

        [FoldoutGroup("������Ʈ ������")]
        public Dictionary<CardType, Sprite[]> FrameSprite = new Dictionary<CardType, Sprite[]>();

        [FoldoutGroup("������Ʈ ������")]
        public Sprite[][] ArtworkSprite;

        // ================================================== Prefab

        [FoldoutGroup("������ ������")]
        public CardObject Prefab;
    } 
}
