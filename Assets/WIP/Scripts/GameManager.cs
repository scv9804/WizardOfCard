using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace WIP
{
    // ==================================================================================================== GameManager

    public class GameManager : MonoSingleton<GameManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        public const string INSTANCE_ID_FORMAT = "D6";

        // =========================================================================== GameManager

        // ================================================== Data

        [Header("µ•¿Ã≈Õ")]
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
            Utility.StringBuilder.Clear();

            switch (type)
            {
                case InstanceType.Boss:
                    Utility.StringBuilder.Append("D");
                    break;
                case InstanceType.Card:
                    Utility.StringBuilder.Append("C");
                    break;
                case InstanceType.Enemy:
                    Utility.StringBuilder.Append("E");
                    break;
                case InstanceType.Item:
                    Utility.StringBuilder.Append("I");
                    break;
                case InstanceType.Player:
                    Utility.StringBuilder.Append("P");
                    break;
            }

            Utility.StringBuilder.Append(_data.Allocated.ToString(INSTANCE_ID_FORMAT));

            _data.Allocated += 1;

            return Utility.StringBuilder.ToString();
        }
    }

    // ==================================================================================================== GameManagerData

    [Serializable]
    public class GameManagerData
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [SerializeField, JsonProperty("Allocated")] private int _allocated = 0;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        [JsonIgnore]
        public int Allocated
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