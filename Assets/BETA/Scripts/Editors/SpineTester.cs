using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using Spine.Unity;

namespace BETA.Editor
{
    public class SpineTester : SerializedMonoBehaviour
    {
        public SkeletonAnimation SkeletonAnimation;
        public SkeletonDataAsset[] SkeletonDataAsset;

        [ShowInInspector] [ReadOnly]
        private int _index;

        private void Start()
        {
            SetSpineSkeletonAnimation();
        }

        public void SetSpineSkeletonAnimation()
        {
            SkeletonAnimation.ClearState();

            SkeletonAnimation.skeletonDataAsset = SkeletonDataAsset[_index];
            SkeletonAnimation.timeScale = 0.5f;

            SkeletonAnimation.Initialize(true);
        }

        [Button]
        public void IndexTo(int index)
        {
            _index = Mathf.Max(index, 0);
            _index = Mathf.Min(index, SkeletonDataAsset.Length - 1);

            SetSpineSkeletonAnimation();
        }

        [ButtonGroup]
        public void Previous()
        {
            _index = Mathf.Max(_index - 1, 0);

            SetSpineSkeletonAnimation();
        }

        [ButtonGroup]
        public void Next()
        {
            _index = Mathf.Min(_index + 1, SkeletonDataAsset.Length - 1);

            SetSpineSkeletonAnimation();
        }
    } 
}
