using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using Spine.Unity;

namespace BETA.Editor
{
    public class EntityActionImages : SerializedMonoBehaviour
    {
        public SkeletonDataAsset SkeletonDataAsset;

        public Sprite OnAttackSprite;
        public Sprite OnHitSprite;
    } 
}
