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

        [Header("��Ÿ�")]
        [SerializeField] private int _range;

        [Header("����")]
        [SerializeField] private List<Vector3> _radius;
        // Vector3

        // =========================================================================== Option

        [Header("���� ��� ����")]
        [SerializeField] private bool _isTargetable;

        [Header("������ ���� ����")]
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

        [Header("��Ÿ�")]
        [SerializeField] private Data<int> _range;

        [Header("����")]
        [SerializeField] private Data<List<Vector3>> _radius;
        // Vector3

        // =========================================================================== Option

        [Header("���� ��� ����")]
        [SerializeField] private Data<bool> _isTargetable;

        [Header("������ ���� ����")]
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
