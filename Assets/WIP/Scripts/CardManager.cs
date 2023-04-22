using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine.EventSystems;

namespace WIP
{
    // ==================================================================================================== CardDelegate

    public delegate void CardDelegate(ICommand<Card, CardObject> command);

    // ==================================================================================================== CardManager

    public class CardManager : MonoSingleton<CardManager>
    {
        // ==================================================================================================== Field
         
        // =========================================================================== CardObject

        public const string CARD_GROUP_NAME = "===== Cards =====";
        public const string CARD_SELECTED_NAME = "===== Selected =====";

        public const float CARD_SPACING_MARGIN = 100.0f;

        [Header("선택 중 카드")]
        public CardObject Selected;

        // =========================================================================== Card

        // ================================================== Own

        [Header("보유 중 카드")]
        [SerializeField] private List<Card> _myOwnedCards = new List<Card>();

        // ================================================== Deck

        [Header("덱 내 카드")]
        [SerializeField] private List<Card> _myDeckCards = new List<Card>();

        // ================================================== Hand

        public const int MAX_HAND_SIZE = 9;

        [Header("손패 내 카드")]
        [SerializeField] private List<Card> _myHandCards = new List<Card>();

        // ================================================== Use

        [Header("사용 중 카드")]
        [SerializeField] private List<Card> _myUsingCards = new List<Card>();

        // ================================================== Cemetery

        [Header("묘지 내 카드")]
        [SerializeField] private List<Card> _myCemeteryCards = new List<Card>();

        // ================================================== Exiled

        [Header("제외된 카드")]
        [SerializeField] private List<Card> _myExiledCards = new List<Card>();

        // =========================================================================== CardManager

        // ================================================== State

        [Header("상태")]
        [SerializeField] public CardManagerState _state = CardManagerState.CanPointerOver | CardManagerState.CanDraging | CardManagerState.CanUsing;

        // ================================================== Resource

        private GameObject _cardPrefab;

        private CardDatabase _cardDatabase;

        // ================================================== Command

        public Dictionary<string, CardDelegate> Subscribers = new Dictionary<string, CardDelegate>();

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        protected override string Name
        {
            get
            {
                return "Card Manager";
            }
        }

        // =========================================================================== Card

        // ================================================== Own

        public ReadOnlyCollection<Card> MyOwnedCards
        {
            get
            {
                return _myOwnedCards.AsReadOnly();
            }
        }

        // ================================================== Deck

        public ReadOnlyCollection<Card> MyDeckCards
        {
            get
            {
                return _myDeckCards.AsReadOnly();
            }
        }

        // ================================================== Hand

        public ReadOnlyCollection<Card> MyHandCards
        {
            get
            {
                return _myHandCards.AsReadOnly();
            }
        }

        // ================================================== Use

        public ReadOnlyCollection<Card> MyUsingCards
        {
            get
            {
                return _myUsingCards.AsReadOnly();
            }
        }

        // ================================================== Cemetery

        public ReadOnlyCollection<Card> MyCemeteryCards
        {
            get
            {
                return _myCemeteryCards.AsReadOnly();
            }
        }

        // ================================================== Exiled

        public ReadOnlyCollection<Card> MyExiledCards
        {
            get
            {
                return _myExiledCards.AsReadOnly();
            }
        }

        // =========================================================================== CardManager

        // ================================================== State

        public bool CanPointerOver
        {
            get
            {
                return (_state & CardManagerState.CanPointerOver) != 0;
            }
        }

        public bool CanDraging
        {
            get
            {
                return (_state & CardManagerState.CanDraging) != 0;
            }
        }

        public bool CanUsing
        {
            get
            {
                return (_state & CardManagerState.CanUsing) != 0;
            }
        }

        public bool OnPointerOver
        {
            get
            {
                return (_state & CardManagerState.OnPointerOver) != 0;
            }
        }

        public bool OnDraging
        {
            get
            {
                return (_state & CardManagerState.OnDraging) != 0;
            }
        }

        public bool OnUsing
        {
            get
            {
                return (_state & CardManagerState.OnUsing) != 0;
            }
        }

        // ================================================== Resource

        public CardDatabase CardDatabase
        {
            get
            {
                return _cardDatabase;
            }

            private set
            {
                _cardDatabase = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        // ================================================== Life Cycle

        protected override void Awake()
        {
            base.Awake();

            LoadResources();
        }

        private void Update()
        {
            InputFunction(Input.GetKeyDown(KeyCode.Space), TempCreateCardSet);
        }

        // ================================================== Pointer

        public void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {
            if (!(OnDraging && !cardObject.IsSelected))
            {
                Selected = cardObject;

                cardObject.SetState(CardState.OnPointerOver, true);

                SetState(CardManagerState.OnPointerOver, true);
            }
        }

        public void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {
            if (!(OnDraging && !cardObject.IsSelected))
            {
                Selected = null;

                cardObject.SetState(CardState.OnPointerOver, false);

                SetState(CardManagerState.OnPointerOver, false);
            }
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (CanDraging && cardObject.IsPlayable)
            {
                cardObject.SetState(CardState.OnPointerOver, false);
                cardObject.SetState(CardState.OnDraging, true);

                SetState(CardManagerState.OnPointerOver, false);
                SetState(CardManagerState.OnDraging, true);
            }
        }

        public void OnDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (CanDraging && cardObject.IsPlayable)
            {
                cardObject.MoveTo(eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (CanDraging && cardObject.IsPlayable)
            {
                cardObject.MoveTo(cardObject.OriginalPosition);

                cardObject.SetState(CardState.OnDraging, false);

                SetState(CardManagerState.OnDraging, false);
            }
        }

        // =========================================================================== CardObject

        // ================================================== Instance

        public CardObject CreateCardObject(string instanceID)
        {
            GameObject gameObject = Instantiate(_cardPrefab);

            var cardObject = gameObject.GetComponent<CardObject>();
            cardObject.InstanceID = instanceID;

            cardObject.OnCreate();

            return cardObject;
        }

        // ================================================== Command

        public void Refresh(string instanceID)
        {
            Subscribers[instanceID].Invoke(new CardRefreshCommand());
        }

        public void Arrange(string instanceID)
        {
            Subscribers[instanceID].Invoke(new CardArrangeCommand());
        }

        // ================================================== Hand

        public void AllHandCards(Action<string> callback)
        {
            for (int i = 0; i < MyHandCards.Count; i++)
            {
                callback(MyHandCards[i].InstanceID);
            }
        }

        // =========================================================================== Card

        // ================================================== Instance

        public Card CreateCard(string instanceID, int serialID)
        {
            var card = new Card()
            {
                InstanceID = instanceID,
                SerialID = serialID
            };

            card.OnCreate();

            return card;
        }

        // ================================================== Own

        public void AddOwnedCard(Card card)
        {
            _myOwnedCards.Add(card);
        }

        // ================================================== Deck

        public void AddDeckCard(Card card)
        {
            _myDeckCards.Add(card);
        }

        public void RemoveDeckCard(Card card)
        {
            _myOwnedCards.Remove(card);
        }

        // ================================================== Hand

        public void AddHandCard(Card card)
        {
            _myHandCards.Add(card);
        }

        public void Draw()
        {
            if (_myDeckCards.Count is 0 || MyHandCards.Count is MAX_HAND_SIZE)
            {
                return;
            }

            // TODO: Card Search Option
            Card card = _myDeckCards.Last();

            Move(_myDeckCards, _myHandCards, card);

            CreateCardObject(card.InstanceID);
        }

        // ================================================== Move

        private void Move(List<Card> from, List<Card> to, Card card)
        {
            from.Remove(card);

            to.Add(card);
        }

        // =========================================================================== CardManager

        // ================================================== State

        public void SetState(CardManagerState state, bool isActive)
        {
            if (isActive)
            {
                _state |= state;
            }
            else
            {
                _state &= ~state;
            }
        }

        // ================================================== Resource

        private void LoadResources()
        {
            _cardPrefab = Resources.Load<GameObject>("Prefabs/Card");

            CardDatabase = Resources.Load<CardDatabase>("Data/CardDatabase");
        }

        // =========================================================================== Temp

        private void InputFunction(bool input, Action callback)
        {
            if (input)
            {
                callback();
            }
        }

        private void TempCreateCardSet()
        {
            string instanceID = InstanceAllocator.Allocate(InstanceType.Card);

            Card card = CreateCard(instanceID, 0);

            AddOwnedCard(card);
            AddDeckCard(card);

            Draw();
        }
    }

    // ==================================================================================================== CardManagerState

    [Flags] public enum CardManagerState
    {
        Nothing         = 0,

        CanPointerOver  = 1 << 0,

        CanDraging      = 1 << 1,

        CanUsing        = 1 << 2,

        OnPointerOver   = 1 << 3,

        OnDraging       = 1 << 4,

        OnUsing         = 1 << 5
    }
}
