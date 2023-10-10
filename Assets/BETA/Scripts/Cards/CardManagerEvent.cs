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

        [SerializeField, TitleGroup("애플리케이션 관련 이벤트")]
        private EventDispatcher _onGameQuit;

        // ======================================================= Game

        [SerializeField, TitleGroup("게임 관련 이벤트")]
        private EventDispatcher _onGameStart;

        [SerializeField, TitleGroup("게임 관련 이벤트")]
        private EventDispatcher _onGameEnd;

        // ======================================================= Battle

        [SerializeField, TitleGroup("배틀 관련 이벤트")]
        private EventDispatcher _onBattleStart;

        // ======================================================= Card

        [SerializeField, TitleGroup("카드 관련 이벤트")]
        private EventDispatcher _onCardArrange;

        [SerializeField, TitleGroup("카드 관련 이벤트")]
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
