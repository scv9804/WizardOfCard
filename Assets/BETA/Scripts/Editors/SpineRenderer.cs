using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using Spine.Unity;

namespace BETA.Editor
{
    public class SpineRenderer : SerializedMonoBehaviour
    {
        public SkeletonAnimation SkeletonAnimation;

        public void SetSkeletonAnimation(SkeletonDataAsset asset)
        {
            SkeletonAnimation.ClearState();

            SkeletonAnimation.skeletonDataAsset = asset;
            SkeletonAnimation.timeScale = 0.5f;

            SkeletonAnimation.Initialize(true);
        }
    }
}