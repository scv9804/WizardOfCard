using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

namespace WIP
{
    // Abilities
    // ==================================================================================================== CardActionNode

    public abstract class CardActionNode : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        [Header("설명 포맷")]
        [SerializeField] private string _format;

        // =========================================================================== StringBuilder

        private StringBuilder _stringBuilder = new StringBuilder();

        // =========================================================================== Action

        // ================================================== Base

        [Header("활성화 여부")]
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

        // =========================================================================== StringBuilder

        protected StringBuilder StringBuilder
        {
            get
            {
                return _stringBuilder;
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
