using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;

using UnityEngine.EventSystems;

namespace WIP
{
    // ==================================================================================================== Delegate

    public delegate void OnCardRefresh(Sprite frameSprite, Sprite artworkSprite, string name, int cost, string description);

    // ==================================================================================================== Card

    [Serializable] public class Card
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("ÀÎ½ºÅÏ½º ID")]
        [SerializeField, JsonProperty("InstanceID")] private string _instanceID;

        [Header("½Ã¸®¾ó ID")]
        [SerializeField, JsonProperty("SerialID")] private int _serialID;

        // =========================================================================== Upgrade

        [Header("°­È­ È½¼ö")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Delegate

        [JsonIgnore] private OnCardRefresh _onRefresh;

        // =========================================================================== Constant

        // ================================================== Upgrade

        public const int MAX_UPGRADE_LEVEL = 2;
        public const int MAX_STATUS_SIZE = MAX_UPGRADE_LEVEL + 1;

        // ================================================== Scale

        public const float HAND_CARD_SIZE = 0.2f;

        public const float ENLARGEMENT_CARD_SIZE = 2f;

        // ================================================== Sibling Index

        public const string DECK_GROUP_NAME = "===== Decks =====";
        public const string HAND_GROUP_NAME = "===== Hands =====";
        public const string DISCARD_GROUP_NAME = "===== Discards =====";
        public const string EXILED_GROUP_NAME = "===== Exiled =====";

        public const string SELECTED_GROUP_NAME = "===== Selected =====";

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        [JsonIgnore] public string InstanceID
        {
            get
            {
                return _instanceID;
            }

            set
            {
                _instanceID = value;
            }
        }

        [JsonIgnore] public int SerialID
        {
            get
            {
                return _serialID;
            }

            set
            {
                _serialID = value;
            }
        }

        // =========================================================================== Status

        [JsonIgnore] public string Name
        {
            get
            {
                return RefreshName(GetCardData());
            }
        }

        [JsonIgnore] public int Cost
        {
            get
            {
                return RefreshCost(GetCardData());
            }
        }

        [JsonIgnore] public CardKeyword Keyword
        {
            get
            {
                return RefreshKeyword(GetCardData());
            }
        }

        [JsonIgnore] public string Description
        {
            get
            {
                return RefreshDescription(GetCardData());
            }
        }

        // =========================================================================== Upgrade

        [JsonIgnore] public int Upgraded
        {
            get
            {
                return _upgraded;
            }

            private set
            {
                _upgraded = value;
            }
        }

        // =========================================================================== Power

        [JsonIgnore] public int AttackPower
        {
            get; set;
        }

        [JsonIgnore] public int DefensePower
        {
            get; set;
        }

        [JsonIgnore] public int EnhancePower
        {
            get; set;
        }

        // =========================================================================== Delegate

        public event OnCardRefresh OnRefresh
        {
            add
            {
                _onRefresh += value;
            }

            remove
            {
                _onRefresh -= value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Instance

        public static Card Create(string instanceID, int serialID)
        {
            var card = new Card();

            card.Initialize(instanceID, serialID);

            return card;
        }

        public void Initialize(string instanceID, int serialID)
        {
            InstanceID = instanceID;
            SerialID = serialID;

            ////////////////////////////////////////////////// BETA
            Upgrade();
            Upgrade();
            ////////////////////////////////////////////////// BETA

            CardManager.Instance.Refresh();
        }

        public void Dispose()
        {

        }

        // =========================================================================== Status

        private string RefreshName(CardData data)
        {
            string name = $"{data.Name} I";

            Utility.StringBuilder.Clear();
            Utility.StringBuilder.Append(name);

            for (int i = 0; i < Upgraded; i++)
            {
                Utility.StringBuilder.Append("I");
            }

            return Utility.StringBuilder.ToString();
        }

        private int RefreshCost(CardData data)
        {
            int cost = data.Cost[Upgraded];

            return cost;
        }

        private CardKeyword RefreshKeyword(CardData data)
        {
            CardKeyword keyword = data.Keyword[Upgraded];

            return keyword;
        }

        private string RefreshDescription(CardData data)
        {
            string description = data.HandlerData.GetDescription(data.Description[Upgraded], Upgraded);

            Utility.StringBuilder.Clear();
            Utility.StringBuilder.Append(description);

            if ((RefreshKeyword(data) & CardKeyword.Exile) != 0)
            {
                Utility.StringBuilder.Append("¸Á°¢\n");
            }

            Utility.StringBuilder.Replace("¸Á°¢", "<color=#ff88ff>¸Á°¢</color>");

            return Utility.StringBuilder.ToString();
        }

        // =========================================================================== Upgrade

        public void Upgrade()
        {
            Upgraded += 1;
        }

        // =========================================================================== Use

        public IEnumerator Use(CardTarget targets)
        {
            CardData data = GetCardData();

            ////////////////////////////////////////////////// BETA
            data.HandlerData.Execute(this, null);
            ////////////////////////////////////////////////// BETA

            for (int i = 0; i < targets.Targets.Count; i++)
            {
                data.HandlerData.Execute(this, targets.Targets[i]);
            }

            yield return null;
        }

        public void Refresh()
        {
            CardData data = GetCardData();

            Sprite frameSprite = data.FrameSprite[Upgraded];
            Sprite artworkSprite = data.FrameSprite[Upgraded];

            string name = RefreshName(data);
            int cost = RefreshCost(data);
            string description = RefreshDescription(data);

            _onRefresh?.Invoke(frameSprite, artworkSprite, name, cost, description);
        }

        // =========================================================================== Data

        private CardData GetCardData()
        {
            return CardManager.Instance.Database.Cards[SerialID];
        }
    }

    // ==================================================================================================== CardTarget

    public class CardTarget
    {
        // ==================================================================================================== Field

        // =========================================================================== Target

        private List<Entity> _targets = new List<Entity>();

        private bool _isActive;

        // ==================================================================================================== Property

        // =========================================================================== Target

        public List<Entity> Targets
        {
            get
            {
                return _targets;
            }

            set
            {
                _targets = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
            }
        }
    }

    // ==================================================================================================== CardKeyword

    [Flags] public enum CardKeyword
    {
        None    = 0,

        Exile   = 1 << 0
    }

    // ==================================================================================================== CardState

    public enum CardState
    {
        None,

        IsPointerOver,

        IsDrag,

        IsUse
    }
}

// Buff

// 1. Base Value
// 2. Player Buff Value
// 3. Card Buff Value