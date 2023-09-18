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

        [FoldoutGroup("��ü ������")]
        public string InstanceID;

        [FoldoutGroup("��ü ������")]
        public int SerialID = -1;

        // =========================================================================== General

        [FoldoutGroup("�Ϲ� ������")]
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
