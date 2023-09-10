using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // ==================================================================================================== SpiriteTable
    
    [CreateAssetMenu(menuName = "BETA/Sprite")]
    public sealed class SpriteTable : SerializedScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Sprite

        [PreviewField]
        public Sprite[] Sprite;
    }
}
