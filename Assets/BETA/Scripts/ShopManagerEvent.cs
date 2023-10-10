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

        [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
        private EventDispatcher _onStageStart;

        // ======================================================= Room

        [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
        private EventDispatcher<bool> _onShopEnter;

        // ======================================================= Card

        [SerializeField, TitleGroup("ī�� ���� �̺�Ʈ")]
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
