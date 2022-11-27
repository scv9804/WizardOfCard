using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineUtility : MonoBehaviour
{
    public static void EditorForceReloadSkeletonDataAssetAndComponent(SkeletonRenderer component)
    {
        if (component == null) return;

        // Clear all and reload.
        if (component.skeletonDataAsset != null)
        {
            foreach (AtlasAssetBase aa in component.skeletonDataAsset.atlasAssets)
            {
                if (aa != null) aa.Clear();
            }
            component.skeletonDataAsset.Clear();
        }
        component.skeletonDataAsset.GetSkeletonData(true);

        // 다시 초기화 시키기.
        EditorForceInitializeComponent(component);
    }

    public static void EditorForceInitializeComponent(SkeletonRenderer component)
    {
        if (component == null) return;
        if (!SkeletonDataAssetIsValid(component.SkeletonDataAsset)) return;
        component.Initialize(true);

#if BUILT_IN_SPRITE_MASK_COMPONENT
         SpineMaskUtilities.EditorAssignSpriteMaskMaterials(component);
#endif

        component.LateUpdate();
    }

    static bool SkeletonDataAssetIsValid(SkeletonDataAsset asset)
    {
        return asset != null && asset.GetSkeletonData(quiet: true) != null;
    }
}
