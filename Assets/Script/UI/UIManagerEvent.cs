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

        [SerializeField, TitleGroup("카드 관련 이벤트")]
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
