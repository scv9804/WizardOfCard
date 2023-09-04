using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Graphics
{
    // ==================================================================================================== TransparencySortHelper

    [HideMonoScript] [RequireComponent(typeof(Camera))]
    public class TransparencySortHelper : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Graphic

        [FoldoutGroup("Camera")] 
        public Camera Camera;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            if (Camera == null)
            {
                Camera = GetComponent<Camera>();
            }
        }

        // =========================================================================== Graphic

        [Button] [FoldoutGroup("Transparency Sort Option")]
        public void SetTransparencySortOption(TransparencySortMode mode, Vector3 axis)
        {
            Camera.transparencySortMode = mode;
            Camera.transparencySortAxis = axis;
        }
    }
}

// Isometric Mode = TransparencySortMode.CustomAxis, ( 0.0f, 1.0f, -0.26f )