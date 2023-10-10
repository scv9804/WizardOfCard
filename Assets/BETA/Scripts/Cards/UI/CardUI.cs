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

        [SerializeField, TitleGroup("ī��UI �̺�Ʈ")]
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
