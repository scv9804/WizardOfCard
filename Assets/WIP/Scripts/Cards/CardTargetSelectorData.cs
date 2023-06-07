using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== CardTargetSelectorData

    [Serializable] public class CardTargetSelectorData
    {
        // ==================================================================================================== Field

        // =========================================================================== Area

        [Header("사거리")]
        [SerializeField] private int _range;

        [Header("범위")]
        [SerializeField] private List<Vector3> _radius;
        // Vector3

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

            set
            {
                _range = value;
            }
        }

        public List<Vector3> Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        // =========================================================================== Option

        public bool IsTargetable
        {
            get
            {
                return _isTargetable;
            }

            set
            {
                _isTargetable = value;
            }
        }

        public bool IsRandom
        {
            get
            {
                return _isRandom;
            }

            set
            {
                _isRandom = value;
            }
        }
    }

    // ==================================================================================================== CardTargetSelector

    [Serializable] public class CardTargetSelector
    {
        // ==================================================================================================== Field

        // =========================================================================== Area

        [Header("사거리")]
        [SerializeField] private Data<int> _range;

        [Header("범위")]
        [SerializeField] private Data<List<Vector3>> _radius;
        // Vector3

        // =========================================================================== Option

        [Header("설정 사용 여부")]
        [SerializeField] private Data<bool> _isTargetable;

        [Header("무작위 지정 여부")]
        [SerializeField] private Data<bool> _isRandom;

        // ==================================================================================================== Property

        // =========================================================================== Area

        public Data<int> Range
        {
            get
            {
                return _range;
            }

            set
            {
                _range = value;
            }
        }

        public Data<List<Vector3>> Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        // =========================================================================== Option

        public Data<bool> IsTargetable
        {
            get
            {
                return _isTargetable;
            }

            set
            {
                _isTargetable = value;
            }
        }

        public Data<bool> IsRandom
        {
            get
            {
                return _isRandom;
            }

            set
            {
                _isRandom = value;
            }
        }
    }
}
