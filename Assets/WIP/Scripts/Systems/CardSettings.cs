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

        [Header("최대 손패 카드 수")]
        [SerializeField] public int _maxHandCount;

        // ==================================================================================================== Property

        // =========================================================================== Card

        public int MaxHandCount
        {
            get
            {
                return _maxHandCount;
            }
        }
    }
}
