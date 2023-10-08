using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardTargetData

    [CreateAssetMenu(menuName = "WIP/Card/TargetData", fileName = "_CardTargetData")]
    public class CardTargetData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Area

        [Header("사거리")]
        [SerializeField] private int _range;

        [Header("범위")]
        [SerializeField] private List<Vector3> _radius;

        // =========================================================================== Option

        [Header("설정 사용 여부")]
        [SerializeField] private bool _isTargetable;

        [Header("무작위 지정 여부")]
        [SerializeField] private bool _isRandom;

        // ==================================================================================================== Property

        // =========================================================================== Area

        public int Range
        {
            get
            {
                return _range;
            }
        }

        public List<Vector3> Radius
        {
            get
            {
                return _radius;
            }
        }

        // =========================================================================== Option

        public bool IsTargetable
        {
            get
            {
                return _isTargetable;
            }
        }

        public bool IsRandom
        {
            get
            {
                return _isRandom;
            }
        }
    }
}
