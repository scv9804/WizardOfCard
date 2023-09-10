using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== RuntimeData

    [Serializable]
    public abstract class RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [FoldoutGroup("개체 데이터")]
        public string InstanceID;

        [FoldoutGroup("개체 데이터")]
        public int SerialID;

        // =========================================================================== General

        [FoldoutGroup("일반 데이터")]
        public string Name;
    } 
}
