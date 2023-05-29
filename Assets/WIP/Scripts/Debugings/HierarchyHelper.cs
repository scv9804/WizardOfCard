using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    public class HierarchyHelper : MonoBehaviour
    {
        // ==================================================================================================== Field

        [Header("����")]
        public Help[] Helps;
    }
    
    [Serializable] public class Help
    {
        // ==================================================================================================== Field

        [Header("���� �̸�")]
        public string Name;

        [Header("���� ����")]
        [TextArea(5, 15)] public string Description;
    }
}
