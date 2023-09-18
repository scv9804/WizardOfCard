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

        [FoldoutGroup("�ν��Ͻ� ����")]
        public int MaxInstanceCount;

        [FoldoutGroup("�ν��Ͻ� ����")]
        public string InstanceIDFormat;

        // =========================================================================== Card

        [FoldoutGroup("ī�� ����")]
        public int[] StartCardSerialID;

        [FoldoutGroup("ī�� ����")]
        public int MaxHandCount;

        // =========================================================================== Overlay

        [FoldoutGroup("�������� ����")]
        public Color MoveRangeColor;

        [FoldoutGroup("�������� ����")]
        public Color AttackRangeColor;

        [FoldoutGroup("�������� ����")]
        public Color BlockedTileColor;
    }
}
