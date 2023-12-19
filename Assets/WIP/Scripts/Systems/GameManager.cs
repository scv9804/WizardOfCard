using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Text;

namespace WIP
{
    // ==================================================================================================== GameManager

    public class GameManager : MonoSingleton<GameManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        public const string INSTANCE_ID_FORMAT = "D6";

        // =========================================================================== StringBuilder

        private StringBuilder _stringBuilder = new StringBuilder();

        // =========================================================================== GameManager

        // ================================================== GameMode

        private GameMode _gameMode = GameMode.Battle;

        // ================================================== Data

        [Header("데이터")]
        [SerializeField] private GameManagerData _data = new GameManagerData();

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        protected override string Name
        {
            get
            {
                return "Game Manager";
            }
        }

        // ================================================== GameMode

        public GameMode GameMode
        {
            get
            {
                return _gameMode;
            }

            private set
            {
                _gameMode = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        // ================================================== Life Cycle

        protected override void Awake()
        {
            base.Awake();
        }

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        // =========================================================================== Identifier

        public string Allocate(InstanceType type)
        {
            _stringBuilder.Clear();

            switch (type)
            {
                case InstanceType.Boss:
                    _stringBuilder.Append("D");
                    break;
                case InstanceType.Card:
                    _stringBuilder.Append("C");
                    break;
                case InstanceType.Enemy:
                    _stringBuilder.Append("E");
                    break;
                case InstanceType.Item:
                    _stringBuilder.Append("I");
                    break;
                case InstanceType.Player:
                    _stringBuilder.Append("P");
                    break;
            }

            _stringBuilder.Append(_data.Allocated.ToString(INSTANCE_ID_FORMAT));

            _data.Allocated += 1;

            return _stringBuilder.ToString();
        }
    }

    // ==================================================================================================== GameManagerData

    [Serializable] public class GameManagerData
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("할당 예정 ID")]
        [SerializeField, JsonProperty("Allocated")] private int _allocated = 0;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        [JsonIgnore] public int Allocated
        {
            get
            {
                return _allocated;
            }

            set
            {
                _allocated = value;
            }
        }
    }

    // ==================================================================================================== GameMode

    public enum GameMode
    {
        None,

        WorldMap,

        Battle,

        Event
    }

    // ==================================================================================================== InstanceType

    public enum InstanceType
    {
        Boss,

        Card,

        Enemy,

        Item,

        Player
    }
}