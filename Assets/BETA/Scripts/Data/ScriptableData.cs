using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // ==================================================================================================== ScriptableData

    public abstract class ScriptableData : SerializedScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== General

        [FoldoutGroup("일반 데이터")]
        public string Name;
    }
}
