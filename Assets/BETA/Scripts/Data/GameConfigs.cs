using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== GameConfigs

    [CreateAssetMenu(menuName = "BETA/Configs", fileName = "GameConfigs")]
    public sealed class GameConfigs : SerializedScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [FoldoutGroup("인스턴스 설정")]
        public int MaxInstanceCount;

        [FoldoutGroup("인스턴스 설정")]
        public string InstanceIDFormat;

        // =========================================================================== Card

        [FoldoutGroup("카드 설정")]
        public int[] StartCardSerialID;

        [FoldoutGroup("카드 설정")]
        public int MaxHandCount;

        // =========================================================================== Overlay

        [FoldoutGroup("오버레이 설정")]
        public Color MoveRangeColor;

        [FoldoutGroup("오버레이 설정")]
        public Color AttackRangeColor;

        [FoldoutGroup("오버레이 설정")]
        public Color BlockedTileColor;
    }
}
