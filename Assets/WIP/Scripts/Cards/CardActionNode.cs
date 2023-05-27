using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardActionNode

    public abstract class CardActionNode : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        [Header("���� ����")]
        [SerializeField] private string _format;

        // =========================================================================== Action

        // ================================================== Base

        [Header("Ȱ��ȭ ����")]
        [SerializeField] private bool[] _isActive = new bool[Card.MAX_UPGRADE_LEVEL + 1];

        // ==================================================================================================== Property

        // =========================================================================== Status

        public string Format
        {
            get
            {
                return _format;
            }
        }

        // =========================================================================== Action

        // ================================================== Base

        public bool[] IsActive
        {
            get
            {
                return _isActive;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Status

        public abstract string GetDescription(string description, int upgraded);

        // =========================================================================== Action

        // ================================================== Base

        public abstract void Execute(Card card, CardHandler handler);
    }
}
