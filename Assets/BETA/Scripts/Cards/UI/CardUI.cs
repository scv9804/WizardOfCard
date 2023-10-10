using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.UI
{
    // ==================================================================================================== CardUI

    public class CardUI : UIHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== EventDispatcher

        [SerializeField, TitleGroup("카드UI 이벤트")]
        private CardUIEvent _events;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void OnEnable()
        {
            _events.OnCardArrange.Listener += Refresh;
        }

        private void OnDisable()
        {
            
        }

        //

        public override void Refresh()
        {

        }

        public void Arrange()
        {

        }
    }

    // ==================================================================================================== CardUIEvent

    public class CardUIEvent : SerializedMonoBehaviour
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
