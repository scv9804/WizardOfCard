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
        public int SerialID = -1;

        // =========================================================================== General

        [FoldoutGroup("일반 데이터")]
        public string Name;

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        protected RuntimeData() { }

        public RuntimeData(string instanceID, int serialID)
        {
            InstanceID = instanceID;
            SerialID = serialID;
        }
    }
}
