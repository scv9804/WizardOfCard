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

        [FoldoutGroup("스프라이트 렌더러")]
        public SpriteRenderer TileRenderer;

        [FoldoutGroup("스프라이트 렌더러")]
        public SpriteRenderer ArrowRenderer;
    } 
}
