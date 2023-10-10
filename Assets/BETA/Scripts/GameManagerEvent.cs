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

        [SerializeField, TitleGroup("���ø����̼� ���� �̺�Ʈ")]
        private EventDispatcher _onGameQuit;

        // ======================================================= Game

        [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
        private EventDispatcher _onGameStart;

        [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
        private EventDispatcher _onGameEnd;

        // ======================================================= Stage

        [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
        private EventDispatcher _onStageStart;

        [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
        private EventDispatcher _onStageEnd;

        // ======================================================= Battle

        [SerializeField, TitleGroup("��Ʋ ���� �̺�Ʈ")]
        private EventDispatcher _onBattleStart;

        [SerializeField, TitleGroup("��Ʋ ���� �̺�Ʈ")]
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
