using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    public class HierarchyHelper : MonoBehaviour
    {
        // ==================================================================================================== Field

        [Header("도움말")]
        public Help[] Helps;
    }
    
    [Serializable] public class Help
    {
        // ==================================================================================================== Field

        [Header("도움말 이름")]
        public string Name;

        [Header("도움말 내용")]
        [TextArea(5, 15)] public string Description;
    }
}
