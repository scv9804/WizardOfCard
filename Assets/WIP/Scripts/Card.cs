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

        // =========================================================================== CardModel

        [Header("데이터 모델")]
        [SerializeField, JsonProperty("Model")] private CardModel _model = new CardModel();

        // =========================================================================== Data

        [Header("원본 데이터")]
        [SerializeField, JsonIgnore] private CardData _data;

        // ================================================== Action

        [JsonIgnore] private EventHandler<CardRefreshEventArgs> _onRefresh;

        [JsonIgnore] private EventHandler _onDestroy;

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

        // ================================================== Hand

        public const int MAX_HAND_COUNT = 9;

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

        [JsonIgnore] public string Name
        {
            get
            {
                return _model.Name;
            }

            private set
            {
                _model.Name = value;
            }
        }

        [JsonIgnore] public int Cost
        {
            get
            {
                return _model.Cost;
            }

            private set
            {
                _model.Cost = value;
            }
        }

        [JsonIgnore] public CardKeyword Keyword
        {
            get
            {
                return _model.Keyword;
            }

            private set
            {
                _model.Keyword = value;
            }
        }

        [JsonIgnore] public string Description
        {
            get
            {
                return _model.Description;
            }

            private set
            {
                _model.Description = value;
            }
        }

        // ================================================== Upgraded

        [JsonIgnore] public int Upgraded
        {
            get
            {
                return _model.Upgraded;
            }

            private set
            {
                _model.Upgraded = value;
            }
        }

        // ================================================== Power

        [JsonIgnore] public int AttackPower
        {
            get
            {
                return _model.AttackPower;
            }

            private set
            {
                _model.AttackPower = value;
            }
        }

        [JsonIgnore] public int DefensePower
        {
            get
            {
                return _model.DefensePower;
            }

            private set
            {
                _model.DefensePower = value;
            }
        }

        [JsonIgnore] public int EnhancePower
        {
            get
            {
                return _model.EnhancePower;
            }

            private set
            {
                _model.EnhancePower = value;
            }
        }

        // ================================================== Action

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

        public event EventHandler OnDestroy
        {
            add
            {
                _onDestroy += value;
            }

            remove
            {
                _onDestroy -= value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== ??????

        public static Card Create(string instanceID, int serialID)
        {
            var card = new Card();

            card.InstanceID = instanceID;
            card.SerialID = serialID;

            card.OnInitialize();

            return card;
        }

        // =========================================================================== Instance

        public void OnInitialize()
        {
            _data = CardManager.Instance.Database.Cards[SerialID];
        }

        public void OnDispose()
        {

        }

        // =========================================================================== ????

        public void Use(CardTarget_Temp targets)
        {
            ICardSkillModel_Temp skillModel = _data.SkillData.Create(Upgraded);

            // TODO: Apply Buff Effects

            CardManager.Instance.StartCoroutine(_data.SkillData.Execute(targets, _model, skillModel));
        }

        private void Refresh(object sender, CardRefreshEventArgs e)
        {
            //
            //

            //
            //
            //

            _onRefresh?.Invoke(this, e);
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

        public float Size(CardState state)
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

        // ==================================================================================================== Property

        // =========================================================================== Transform

        // ================================================== Scale

        public float Size(CardState state);

        // ================================================== Position

        public (int Count, int Index) GetElement(Card card);

        public Vector3 GetPosition(int count, int index);
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