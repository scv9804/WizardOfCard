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
        public int SerialID;

        // =========================================================================== General

        [FoldoutGroup("�Ϲ� ������")]
        public string Name;
    } 
}
