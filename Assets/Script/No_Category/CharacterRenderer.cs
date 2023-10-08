using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using Sirenix.OdinInspector;

using Spine.Unity;

using TacticsToolkit;

public class CharacterRenderer : SerializedMonoBehaviour
{
    // ================================================================================ Field

    // ============================================================ Entity

    public Entity Target;

    // ============================================================ Renderer

    public SpriteRenderer SpriteRenderer;

    public SkeletonAnimation SkeletonAnimation;

    // ============================================================ Data

    // ======================================== Entity

    public float Distance;

    public bool IsPlayer;

    // ======================================== Spine

    public SkeletonDataAsset SkeletonDataAsset;

    // ======================================== Sprite

    public Dictionary<string, Sprite> Motions = new Dictionary<string, Sprite>();

    // ================================================================================ Method

    // ============================================================ Event

    private void Start()
    {
        SetSkeletonAnimation(SkeletonDataAsset);
    }

    // ============================================================ Renderer

    public void SetSkeletonAnimation(SkeletonDataAsset asset)
    {
        SkeletonAnimation.ClearState();

        SkeletonAnimation.skeletonDataAsset = asset;
        SkeletonAnimation.timeScale = 0.5f;

        SkeletonAnimation.Initialize(true);
    }

    public bool IsMotionContains(string name)
    {
        return Motions.ContainsKey(name);
    }

    public void SetMotion(string name)
    {
        if (IsMotionContains(name))
        {
            SpriteRenderer.sprite = Motions[name];
        }
    }

    public void Flip(bool isMain)
    {
        SkeletonAnimation.gameObject.SetActive(isMain);
        SpriteRenderer.gameObject.SetActive(!isMain);
    }
}
