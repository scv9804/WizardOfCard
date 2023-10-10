using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== UIManagerEvent

    public class UIManagerEvent : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== EventDispatcher

        // ======================================================= Card

        [SerializeField, TitleGroup("ī�� ���� �̺�Ʈ")]
        private EventDispatcher _onCardArrange;

        // ==================================================================================================== Property

        // =========================================================================== EventDispatcher

        // ======================================================= Card

        public EventDispatcher OnCardArrange
        {
            get => _onCardArrange;

            private set => _onCardArrange = value;
        }
    }
}
