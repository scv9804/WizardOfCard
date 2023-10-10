using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== GameManagerEvent

    public class GameManagerEvent : SerializedMonoBehaviour
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

        // ======================================================= Stage

        [SerializeField, TitleGroup("스테이지 관련 이벤트")]
        private EventDispatcher _onStageStart;

        [SerializeField, TitleGroup("스테이지 관련 이벤트")]
        private EventDispatcher _onStageEnd;

        // ======================================================= Battle

        [SerializeField, TitleGroup("배틀 관련 이벤트")]
        private EventDispatcher _onBattleStart;

        [SerializeField, TitleGroup("배틀 관련 이벤트")]
        private EventDispatcher _onBattleEnd;

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

        // ======================================================= Stage

        public EventDispatcher OnStageStart
        {
            get => _onStageStart;

            private set => _onStageStart = value;
        }

        public EventDispatcher OnStageEnd
        {
            get => _onStageEnd;

            private set => _onStageEnd = value;
        }

        // ======================================================= Battle

        public EventDispatcher OnBattleStart
        {
            get => _onBattleStart;

            private set => _onBattleStart = value;
        }

        public EventDispatcher OnBattleEnd
        {
            get => _onGameEnd;

            private set => _onGameEnd = value;
        }
    }
}
