using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== ItemRuntimeData

    [Serializable]
    public sealed class ItemRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== General



        [FoldoutGroup("일반 데이터")] [MultiLineProperty(5)]
        public string Description;
    }
}