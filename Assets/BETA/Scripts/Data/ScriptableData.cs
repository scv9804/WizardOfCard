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

        [FoldoutGroup("�Ϲ� ������")]
        public string Name;
    }
}
