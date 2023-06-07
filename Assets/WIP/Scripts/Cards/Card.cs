using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Text;

namespace WIP
{
    // ==================================================================================================== Card

    [Serializable] public class Card
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("인스턴스 ID")]
        [SerializeField, JsonProperty("InstanceID")] private string _instanceID;

        [Header("시리얼 ID")]
        [SerializeField, JsonProperty("SerialID")] private int _serialID;

        // =========================================================================== Status

        // ================================================== Base

        [Header("이름")]
        [SerializeField, JsonProperty("Name")] private Data<string> _name = new Data<string>();

        [Header("비용")]
        [SerializeField, JsonProperty("Cost")] private Data<int> _cost = new Data<int>();

        [Header("키워드")]
        [SerializeField, JsonProperty("Name")] private Data<CardKeyword> _keyword = new Data<CardKeyword>();

        [Header("설명")]
        [SerializeField, JsonProperty("Cost")] private Data<string> _description = new Data<string>();

        // ================================================== Upgrade

        [Header("강화 횟수")]
        [SerializeField, JsonProperty("Upgraded")] private Data<int> _upgraded = new Data<int>();

        // ================================================== Asset

        [Header("프레임 스프라이트")]
        [SerializeField, JsonIgnore] private Data<Sprite> _frameSprite = new Data<Sprite>();

        [Header("아이콘 스프라이트")]
        [SerializeField, JsonIgnore] private Data<Sprite> _artworkSprite = new Data<Sprite>();

        // ================================================== Ability

        [Header("데미지")]
        [SerializeField, JsonProperty("Damage")] private List<Data<int>> _damage = new List<Data<int>>();

        [Header("쉴드")]
        [SerializeField, JsonProperty("Shield")] private List<Data<int>> _shield = new List<Data<int>>();

        // ================================================== System

        [Header("반복 횟수")]
        [SerializeField, JsonProperty("Count")] private List<Data<int>> _count = new List<Data<int>>();

        // =========================================================================== Data

        [Header("원본 데이터")]
        [SerializeField, JsonProperty("Count")] private CardData _data;

        // =========================================================================== StringBuilder

        [JsonIgnore] private StringBuilder _stringBuilder = new StringBuilder();

        // =========================================================================== Constant

        // ================================================== Upgrade

        public const int MAX_UPGRADE_LEVEL = 2;
        public const int MAX_STATUS_SIZE = MAX_UPGRADE_LEVEL + 1;

        // ================================================== Scale

        public const float HAND_CARD_SIZE = 0.25f;

        public const float ENLARGEMENT_CARD_SIZE = 1.6f;

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

        // ================================================== Base

        [JsonIgnore] public Data<string> Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
            }
        }

        [JsonIgnore] public Data<int> Cost
        {
            get
            {
                return _cost;
            }

            private set
            {
                _cost = value;
            }
        }

        [JsonIgnore] public Data<CardKeyword> Keyword
        {
            get
            {
                return _keyword;
            }

            private set
            {
                _keyword = value;
            }
        }

        [JsonIgnore] public Data<string> Description
        {
            get
            {
                return _description;
            }

            private set
            {
                _description = value;
            }
        }

        // ================================================== Upgrade

        [JsonIgnore] public Data<int> Upgraded
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

        // ================================================== Asset

        [JsonIgnore] public Data<Sprite> FrameSprite
        {
            get
            {
                return _frameSprite;
            }

            private set
            {
                _frameSprite = value;
            }
        }

        [JsonIgnore] public Data<Sprite> ArtworkSprite
        {
            get
            {
                return _artworkSprite;
            }

            private set
            {
                _artworkSprite = value;
            }
        }

        // ================================================== Ability

        [JsonIgnore] public int Damage
        {
            get
            {
                return _cost.Value;
            }

            private set
            {
                _cost.Value = value;
            }
        }

        // ================================================== System

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

        // =========================================================================== Data

        [JsonIgnore] public CardData Data
        {
            get
            {
                return _data;
            }

            private set
            {
                _data = value;
            }
        }

        [JsonIgnore, Obsolete] public CardHandlerData HandlerData
        {
            get
            {
                return Data.HandlerData;
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

            Data = CardManager.Instance.Database.Cards[SerialID];

            ////////////////////////////////////////////////// BETA
            Upgrade(2);
            ////////////////////////////////////////////////// BETA

            AddEventCallback();
        }

        public void Dispose()
        {
            RemoveEventCallback();

            Data = null;
        }

        public void AddEventCallback()
        {
            Upgraded.OnChange += RefreshFrameSprite;
            Upgraded.OnChange += RefreshArtworkSprite;

            Upgraded.OnChange += RefreshName;
            Upgraded.OnChange += RefreshCost;
            Upgraded.OnChange += RefreshKeyword;
            Upgraded.OnChange += RefreshDescription;

            Keyword.OnChange += RefreshDescription;
        }

        public void RemoveEventCallback()
        {
            Upgraded.OnChange -= RefreshFrameSprite;
            Upgraded.OnChange -= RefreshArtworkSprite;

            Upgraded.OnChange -= RefreshName;
            Upgraded.OnChange -= RefreshCost;
            Upgraded.OnChange -= RefreshKeyword;
            Upgraded.OnChange -= RefreshDescription;

            Keyword.OnChange -= RefreshDescription;
        }

        // =========================================================================== Status

        // ================================================== Base

        private void RefreshName(IEventParameter parameter)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append($"{Data.Name} I");

            for (int i = 0; i < Upgraded.Value; i++)
            {
                _stringBuilder.Append("I");
            }

            Name.Value = _stringBuilder.ToString();
        }

        private void RefreshCost(IEventParameter parameter)
        {
            int cost = Data.Cost[Upgraded.Value];

            //

            Cost.Value = cost;
        }

        private void RefreshKeyword(IEventParameter parameter)
        {
            CardKeyword keyword = Data.Keyword[Upgraded.Value];

            //

            Keyword.Value = keyword;
        }

        private void RefreshDescription(IEventParameter parameter)
        {
            _stringBuilder.Clear();

            if ((Keyword.Value & CardKeyword.Exile) != 0)
            {
                _stringBuilder.Append("망각\n");
            }

            string description = Data.HandlerData.GetDescription(Data.Description[Upgraded.Value], Upgraded.Value);

            _stringBuilder.Append(description);

            _stringBuilder.Replace("망각", "<color=#ff88ff>망각</color>");

            Description.Value = _stringBuilder.ToString();
        }

        // ================================================== Upgrade

        public void Upgrade(int upgrade)
        {
            Upgraded.Value = Math.Min(Upgraded.Value + upgrade, MAX_UPGRADE_LEVEL);
        }

        // ================================================== Asset

        private void RefreshFrameSprite(IEventParameter parameter)
        {
            FrameSprite.Value = Data.FrameSprite[Upgraded.Value];
        }

        private void RefreshArtworkSprite(IEventParameter parameter)
        {
            ArtworkSprite.Value = Data.ArtworkSprite[Upgraded.Value];
        }

        // =========================================================================== Use

        public IEnumerator Use(CardTarget targets)
        {
            ////////////////////////////////////////////////// BETA
            Data.HandlerData.Execute(this, null);
            ////////////////////////////////////////////////// BETA

            for (int i = 0; i < targets.Targets.Count; i++)
            {
                Data.HandlerData.Execute(this, targets.Targets[i]);
            }

            yield return null;
        }

        public void Refresh()
        {
            Upgraded.Value = Upgraded.Value;
        }

        // =========================================================================== Observer

        public void Subscribe(CardObject cardObject)
        {
            FrameSprite.OnChange += cardObject.RefreshFrameImage;
            ArtworkSprite.OnChange += cardObject.RefreshArtworkImage;

            Name.OnChange += cardObject.RefreshNameText;
            Cost.OnChange += cardObject.RefreshCostText;
            Description.OnChange += cardObject.RefreshDescriptionText;
        }

        public void Unsubscribe(CardObject cardObject)
        {
            FrameSprite.OnChange -= cardObject.RefreshFrameImage;
            ArtworkSprite.OnChange -= cardObject.RefreshArtworkImage;

            Name.OnChange -= cardObject.RefreshNameText;
            Cost.OnChange -= cardObject.RefreshCostText;
            Description.OnChange -= cardObject.RefreshDescriptionText;
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
// 3. Item Buff Value
// 4. Card Buff Value