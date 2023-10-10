using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ==================================================================================================== EntityManagerEvent

namespace BETA
{
    public class EntityManagerEvent : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== EventDispatcher

        // ======================================================= Game

        [SerializeField, TitleGroup("게임 관련 이벤트")]
        private EventDispatcher _onGameStart;

        // ==================================================================================================== Property

        // =========================================================================== EventDispatcher

        // ======================================================= Game

        public EventDispatcher OnGameStart
        {
            get => _onGameStart;

            private set => _onGameStart = value;
        }
    } 
}