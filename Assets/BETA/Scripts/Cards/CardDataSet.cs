using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // =========================================================================== DataSet

    [CreateAssetMenu(menuName = "BETA/Card/DataSet", fileName = "CardDataSet")]
    public sealed class CardDataSet : DataSet
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [TableList] [FoldoutGroup("������Ʈ ������")]
        public CardScriptableData[] Data;

        [FoldoutGroup("������Ʈ ������")]
        public Dictionary<CardType, Sprite[]> FrameSprite = new Dictionary<CardType, Sprite[]>();

        [FoldoutGroup("������Ʈ ������")]
        public Sprite[][] ArtworkSprite;

        [FoldoutGroup("������ ������")]
        public CardObject Prefab;
    } 
}
