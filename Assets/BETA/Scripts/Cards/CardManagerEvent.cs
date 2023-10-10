using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== CardManagerEvent

    public class CardManagerEvent : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== EventDispatcher

        // ======================================================= Application

        [SerializeField, TitleGroup("���ø����̼� ���� �̺�Ʈ")]
        private EventDispatcher _onGameQuit;

        // ======================================================= Game

        [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
        private EventDispatcher _onGameStart;

        [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
        private EventDispatcher _onGameEnd;

        // ======================================================= Battle

        [SerializeField, TitleGroup("��Ʋ ���� �̺�Ʈ")]
        private EventDispatcher _onBattleStart;

        // ======================================================= Card

        [SerializeField, TitleGroup("ī�� ���� �̺�Ʈ")]
        private EventDispatcher _onCardArrange;

        [SerializeField, TitleGroup("ī�� ���� �̺�Ʈ")]
        private EventDispatcher<CardObject> _onCardBuy;

        // ==================================================================================================== Property

        // =========================================================================== EventDispatcher

        // ======================================================= Application

        public EventDispatcher OnGameQuit
        {
            get => _onGameQuit;

            private set => _onGameQuit = value;
        }

        // ======================================================= Game

        public EventDispatcher OnGameStart
        {
            get => _onGameStart;

            private set => _onGameStart = value;
        }

        public EventDispatcher OnGameEnd
        {
            get => _onGameEnd;

            private set => _onGameEnd = value;
        }

        // ======================================================= Battle

        public EventDispatcher OnBattleStart
        {
            get => _onBattleStart;

            private set => _onBattleStart = value;
        }

        // ======================================================= Card

        public EventDispatcher OnCardArrange
        {
            get => _onCardArrange;

            private set => _onCardArrange = value;
        }

        public EventDispatcher<CardObject> OnCardBuy
        {
            get => _onCardBuy;

            private set => _onCardBuy = value;
        }
    }
}
