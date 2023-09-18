using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Porting
{
    // ==================================================================================================== OverlayTileComponents

    public sealed class OverlayTileComponents : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Component

        // ================================================== SpriteRenderer

        [FoldoutGroup("��������Ʈ ������")]
        public SpriteRenderer TileRenderer;

        [FoldoutGroup("��������Ʈ ������")]
        public SpriteRenderer ArrowRenderer;
    } 
}
