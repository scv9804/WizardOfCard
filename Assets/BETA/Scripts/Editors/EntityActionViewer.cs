using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using Sirenix.OdinInspector;

using Spine.Unity;

namespace BETA.Editor
{
    public class EntityActionViewer : SerializedMonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public SpineRenderer SpineRenderer;

        public EntityActionImages[] Resource;

        private Vector3 _originalPosition;

        [ShowInInspector] [ReadOnly]
        private int _index;

        private void Start()
        {
            SpineRenderer.SetSkeletonAnimation(Resource[_index].SkeletonDataAsset);
        }

        [Button]
        public void IndexTo(int index)
        {
            _index = Mathf.Max(index, 0);
            _index = Mathf.Min(index, Resource.Length - 1);

            SpineRenderer.SetSkeletonAnimation(Resource[_index].SkeletonDataAsset);
        }

        [ButtonGroup("Move Index")]
        public void Previous()
        {
            _index = Mathf.Max(_index - 1, 0);

            SpineRenderer.SetSkeletonAnimation(Resource[_index].SkeletonDataAsset);
        }

        [ButtonGroup("Move Index")]
        public void Next()
        {
            _index = Mathf.Min(_index + 1, Resource.Length - 1);

            SpineRenderer.SetSkeletonAnimation(Resource[_index].SkeletonDataAsset);
        }

        [ButtonGroup("Motion Invoke")]
        public void Attack()
        {
            StartCoroutine(Main());

            IEnumerator Main()
            {
                DOTween.Kill(transform);
                transform.position = _originalPosition;

                SpriteRenderer.sprite = Resource[_index].OnAttackSprite;

                _originalPosition = transform.position;

                var movement = _index == 7 ? 0.5f : -0.5f;

                var direction = transform.rotation.y == 1.0f ? -1 : 1;

                transform.DOMove(_originalPosition + new Vector3(movement * direction, 0, 0), 0.15f);

                yield return new WaitForSeconds(0.1f);

                SpriteRenderer.gameObject.SetActive(true);
                SpineRenderer.gameObject.SetActive(false);

                yield return new WaitForSeconds(0.4f);

                transform.position = _originalPosition;

                SpriteRenderer.gameObject.SetActive(false);
                SpineRenderer.gameObject.SetActive(true);

                yield return null;
            }
        }

        [ButtonGroup("Motion Invoke")]
        public void Hit()
        {
            StartCoroutine(Main());

            IEnumerator Main()
            {
                DOTween.Kill(transform);
                transform.position = _originalPosition;

                SpriteRenderer.gameObject.SetActive(true);
                SpineRenderer.gameObject.SetActive(false);

                SpriteRenderer.sprite = Resource[_index].OnHitSprite;

                _originalPosition = transform.position;

                var movement = _index == 7 ? -0.5f : 0.5f;

                var direction = transform.rotation.y == 1.0f ? -1 : 1;

                transform.DOMove(_originalPosition + new Vector3(movement * direction, 0, 0), 0.15f);

                yield return new WaitForSeconds(0.25f);

                transform.position = _originalPosition;

                SpriteRenderer.gameObject.SetActive(false);
                SpineRenderer.gameObject.SetActive(true);

                yield return null;
            }
        }
    }
}