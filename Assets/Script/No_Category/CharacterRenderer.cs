using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using Sirenix.OdinInspector;

using Spine.Unity;

using TacticsToolkit;

public class CharacterRenderer : SerializedMonoBehaviour
{
    //

    //

    public Entity Target;

    //

    public SpriteRenderer SpriteRenderer;

    public SkeletonAnimation SkeletonAnimation;

    //

    //

    public SkeletonDataAsset SkeletonDataAsset;

    //

    public Dictionary<string, Sprite> Motions = new Dictionary<string, Sprite>();

    //

    public float Distance;

    public bool IsPlayer;

    private void Start()
    {
        SetSkeletonAnimation(SkeletonDataAsset);
    }

    public void SetSkeletonAnimation(SkeletonDataAsset asset)
    {
        SkeletonAnimation.ClearState();

        SkeletonAnimation.skeletonDataAsset = asset;
        SkeletonAnimation.timeScale = 0.5f;

        SkeletonAnimation.Initialize(true);
    }

    //public void Motion(string name)
    //{
    //    if (Motions.ContainsKey(name))
    //    {
    //        StartCoroutine(Main());
    //    }

    //    IEnumerator Main()
    //    {
    //        SpriteRenderer.sprite = Motions[name];

    //        Flip(true);

    //        yield return new WaitForSeconds(0.5f);

    //        Flip(false);

    //        yield return null;
    //    }
    //}

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
