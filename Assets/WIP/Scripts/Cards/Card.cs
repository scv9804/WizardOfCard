using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;

using UnityEngine.EventSystems;

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

        // =========================================================================== Upgrade

        [Header("강화 횟수")]
        [SerializeField, JsonProperty("Upgraded"), Range(0, MAX_UPGRADE_LEVEL)] private int _upgraded = 0;

        // =========================================================================== Action

        [JsonIgnore] private EventHandler<CardRefreshEventArgs> _onRefresh;

        // =========================================================================== Constant

        // ================================================== Upgrade

        public const int MAX_UPGRADE_LEVEL = 2;
        public const int MAX_STATUS_SIZE = MAX_UPGRADE_LEVEL + 1;

        // ================================================== Scale

        public const float DEFAULT_CARD_SIZE = 0.25f;
        public const float ENLARGEMENT_CARD_SIZE = 0.4f;

        // ================================================== Sibling Index

        public const string HAND_GROUP_NAME = "===== Hands =====";
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

        // =========================================================================== Action

        public event EventHandler<CardRefreshEventArgs> OnRefresh
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

            card.InstanceID = instanceID;
            card.SerialID = serialID;

            card.Initialize();

            //////////////////////////////////////////////////
            card.Upgrade();
            card.Upgrade();
            //////////////////////////////////////////////////
            
            return card;
        }

        public void Initialize()
        {
            AddEventHandler();

            CardManager.Instance.RefreshAll();
        }

        public void Dispose()
        {
            RemoveEventHandler();
        }

        private void AddEventHandler()
        {
            CardManager.Instance.OnRefresh += Refresh;
        }

        private void RemoveEventHandler()
        {
            CardManager.Instance.OnRefresh -= Refresh;
        }

        // =========================================================================== Upgrade

        public void Upgrade()
        {
            Upgraded += 1;
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
                Utility.StringBuilder.Append("망각\n");
            }

            Utility.StringBuilder.Replace("망각", "<color=#ff88ff>망각</color>");

            return Utility.StringBuilder.ToString();
        }

        // =========================================================================== Use

        public IEnumerator Use(CardTarget targets)
        {
            CardData data = GetCardData();

            //////////////////////////////////////////////////
            data.HandlerData.Execute(this, null);
            //////////////////////////////////////////////////

            for (int i = 0; i < targets.Targets.Count; i++)
            {
                data.HandlerData.Execute(this, targets.Targets[i]);
            }

            yield return null;
        }

        private void Refresh()
        {
            // Refresh card status

            CardData data = GetCardData();

            // 설계 잘못함 다시 수정 필요

            CardRefreshEventArgs eventArgs = new CardRefreshEventArgs();

            eventArgs.FrameSprite = data.FrameSprite[Upgraded];
            eventArgs.ArtworkSprite = data.ArtworkSprite[Upgraded];

            eventArgs.Name = RefreshName(data);
            eventArgs.Cost = RefreshCost(data);
            eventArgs.Description = RefreshDescription(data);

            _onRefresh?.Invoke(this, eventArgs);
        }

        // =========================================================================== Data

        private CardData GetCardData()
        {
            return CardManager.Instance.Database.Cards[SerialID];
        }
    }

    // ==================================================================================================== CardHandler

    public class CardHandModule : ICardLocationModule
    {
        // ==================================================================================================== Property

        // =========================================================================== Location

        public List<Card> Cards
        {
            get
            {
                return CardManager.Instance.MyHandCards;
            }
        }

        // =========================================================================== Transform

        // ================================================== Sibling Index

        public string GroupName
        {
            get
            {
                return Card.HAND_GROUP_NAME;
            }
        }

        // ==================================================================================================== Property

        // =========================================================================== Transform

        // ================================================== Scale

        public float GetSize(CardState state)
        {
            return state == CardState.IsPointerOver ? Card.ENLARGEMENT_CARD_SIZE : Card.DEFAULT_CARD_SIZE;
        }

        // ================================================== Position

        public (int Count, int Index) GetElement(Card card)
        {
            return (CardManager.Instance.MyHandCards.Count, CardManager.Instance.MyHandCards.IndexOf(card));
        }

        public Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 4;

            return new Vector3(x, y, 0.0f);
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

    // ==================================================================================================== ICardLocationModule

    public interface ICardLocationModule
    {
        // ==================================================================================================== Property

        // =========================================================================== Location

        public List<Card> Cards
        {
            get;
        }

        // =========================================================================== Transform

        // ================================================== Sibling Index

        public string GroupName
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Scale

        public float GetSize(CardState state);

        // ================================================== Position

        public (int Count, int Index) GetElement(Card card);

        public Vector3 GetPosition(int count, int index);
    }

    // ==================================================================================================== ICardHandler

    public interface ICardHandler
    {

    }

    // ==================================================================================================== ICardStateModule

    public interface ICardStateModule
    {

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