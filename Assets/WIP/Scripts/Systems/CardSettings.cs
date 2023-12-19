using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardSettings

    [CreateAssetMenu(menuName = "WIP/Card/Settings", fileName = "CardSettings")]
    public class CardSettings : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        [Header("�ִ� ���� ī�� ��")]
        [SerializeField] private int _maxHandCount;

        // =========================================================================== Transform

        // ================================================== Scale

        [Header("ī�������Ʈ ũ��")]
        [SerializeField] private float _defaultCardSize;
        [SerializeField] private float _enlargedCardSize;
        [SerializeField] private float _usedCardSize;

        // ==================================================================================================== Property

        // =========================================================================== Card

        public int MaxHandCount
        {
            get
            {
                return _maxHandCount;
            }
        }

        // =========================================================================== Transform

        // ================================================== Scale

        public float DefaultCardSize
        {
            get
            {
                return _defaultCardSize;
            }
        }

        public float EnlargedCardSize
        {
            get
            {
                return _enlargedCardSize;
            }
        }

        public float UsedCardSize
        {
            get
            {
                return _usedCardSize;
            }
        }
    }
}
