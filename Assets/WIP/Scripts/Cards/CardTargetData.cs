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

        [Header("��Ÿ�")]
        [SerializeField] private int _range;

        [Header("����")]
        [SerializeField] private List<Vector3> _radius;

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
