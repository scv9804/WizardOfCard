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

        [TableList] [FoldoutGroup("오브젝트 데이터")]
        public CardScriptableData[] Data;

        [FoldoutGroup("컴포넌트 데이터")]
        public Dictionary<CardType, Sprite[]> FrameSprite = new Dictionary<CardType, Sprite[]>();

        [FoldoutGroup("컴포넌트 데이터")]
        public Sprite[][] ArtworkSprite;

        [FoldoutGroup("프리팹 데이터")]
        public CardObject Prefab;
    } 
}
