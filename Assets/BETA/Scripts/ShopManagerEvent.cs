using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== ShopManagerEvent

    public class ShopManagerEvent : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== EventDispatcher

        // ======================================================= Stage

        [SerializeField, TitleGroup("스테이지 관련 이벤트")]
        private EventDispatcher _onStageStart;

        // ======================================================= Room

        [SerializeField, TitleGroup("레벨 관련 이벤트")]
        private EventDispatcher<bool> _onShopEnter;

        // ======================================================= Card

        [SerializeField, TitleGroup("카드 관련 이벤트")]
        private EventDispatcher<CardObject> _onCardBuy;

        // ==================================================================================================== Property

        // =========================================================================== EventDispatcher

        // ======================================================= Stage

        public EventDispatcher OnStageStart
        {
            get => _onStageStart;

            private set => _onStageStart = value;
        }

        // ======================================================= Room

        public EventDispatcher<bool> OnShopEnter
        {
            get => _onShopEnter;

            private set => _onShopEnter = value;
        }

        // ======================================================= Card

        public EventDispatcher<CardObject> OnCardBuy
        {
            get => _onCardBuy;

            private set => _onCardBuy = value;
        }
    }
}
