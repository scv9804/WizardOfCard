using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Linq;

using UnityEngine.EventSystems;

namespace WIP
{
    // ==================================================================================================== CardManager

    public class CardManager : MonoSingleton<CardManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        // ================================================== Deck

        [Header("덱 카드 리스트")]
        [SerializeField] private List<Card> _myDeckCards = new List<Card>();

        // ================================================== Hand

        [Header("손패 카드 리스트")]
        [SerializeField] private List<Card> _myHandCards = new List<Card>();

        // ================================================== Use

        [Header("사용 중 카드 리스트")]
        [SerializeField] private List<Card> _myUsedCards = new List<Card>();

        // ================================================== Cemetery

        [Header("묘지 카드 리스트")]
        [SerializeField] private List<Card> _myCemeteryCards = new List<Card>();

        // ================================================== Exile

        [Header("제외 카드 리스트")]
        [SerializeField] private List<Card> _myExiledCards = new List<Card>();

        // ================================================== Action

        // =========================================================================== CardObject

        // ================================================== Instance

        [Header("선택 중인 카드")]
        [SerializeField] private CardObject _selected;

        // ================================================== Deck

        // ================================================== Hand

        // ================================================== Use

        // ================================================== Cemetery

        // ================================================== Exile

        // ================================================== Action

        private EventHandler _onEnlarge;
        private EventHandler _onArrange;
        private EventHandler _onEmphasize;

        private EventHandler<CardRefreshEventArgs> _onRefresh;

        // =========================================================================== CardManager

        // ================================================== State

        [Header("카드매니저 상태")]
        [SerializeField] private CardManagerState _state = CardManagerState.CanUse;

        // ================================================== Resource

        [Header("카드 프리팹")]
        [SerializeField] private GameObject _cardPrefab;

        [Header("데이터베이스")]
        [SerializeField] private CardDatabase _database;

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

        // ================================================== Deck

        public List<Card> MyDeckCards
        {
            get
            {
                return _myDeckCards;
            }

            private set
            {
                _myDeckCards = value;
            }
        }

        // ================================================== Hand

        public List<Card> MyHandCards
        {
            get
            {
                return _myHandCards;
            }

            private set
            {
                _myHandCards = value;
            }
        }

        // ================================================== Use

        public List<Card> MyUsedCards
        {
            get
            {
                return _myUsedCards;
            }

            private set
            {
                _myUsedCards = value;
            }
        }

        // ================================================== Cemetery

        public List<Card> MyCemeteryCards
        {
            get
            {
                return _myCemeteryCards;
            }

            private set
            {
                _myCemeteryCards = value;
            }
        }

        // ================================================== Exile

        public List<Card> MyExiledCards
        {
            get
            {
                return _myExiledCards;
            }

            private set
            {
                _myExiledCards = value;
            }
        }

        // =========================================================================== CardObject

        // ================================================== Instance

        public CardObject Selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
            }
        }

        // ================================================== Action

        public event EventHandler OnEnlarge
        {
            add
            {
                _onEnlarge += value;
            }

            remove
            {
                _onEnlarge -= value;
            }
        }

        public event EventHandler OnArrange
        {
            add
            {
                _onArrange += value;
            }

            remove
            {
                _onArrange -= value;
            }
        }

        public event EventHandler OnEmphasize
        {
            add
            {
                _onEmphasize += value;
            }

            remove
            {
                _onEmphasize -= value;
            }
        }

        // =========================================================================== CardManager

        // ================================================== State

        public CardManagerState State
        {
            get
            {
                return _state;
            }

            private set
            {
                _state = value;
            }
        }

        // ================================================== Resource

        public GameObject CardPrefab
        {
            get
            {
                return _cardPrefab;
            }

            private set
            {
                _cardPrefab = value;
            }
        }

        public CardDatabase Database
        {
            get
            {
                return _database;
            }

            private set
            {
                _database = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        protected override void Awake()
        {
            base.Awake();

            LoadResource();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GetCard(Card.Create(GameManager.Instance.Allocate(InstanceType.Card), 0));
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Draw();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Suffle();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Recycle();
            }
        }

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != null || State < CardManagerState.CanPointerOver)
            {
                return;
            }

            Selected = cardObject;

            Selected.State = CardState.IsPointerOver;

            EnlargeAll();
            EmphasizeAll();
        }

        public void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || Selected.State != CardState.IsPointerOver)
            {
                return;
            }

            Selected.State = CardState.None;

            EnlargeAll();
            EmphasizeAll();

            Selected = null;
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || State < CardManagerState.CanUse || !Selected.IsUsable)
            {
                return;
            }

            Selected.State = CardState.IsDrag;

            EnlargeAll();
        }

        public void OnDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || State < CardManagerState.CanUse)
            {
                return;
            }

            Selected.Move(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject)
            {
                return;
            }

            if (State == CardManagerState.CanUse && Selected.transform.position.y > Screen.height / 2)
            {
                // TODO: Use Card
                //Destroy(Selected.gameObject);

                //_myHandCards.Remove(Selected.Card);

                Selected.Move(new Vector3(0.0f, 0.0f, 0.0f));

                Selected.State = CardState.IsUse;

                TryUse(Selected);

                ArrangeAll();
            }
            else
            {
                Selected.Move(Selected.OriginPosition);

                Selected.State = CardState.None;

                EmphasizeAll();
            }

            Selected = null;
        }

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        // =========================================================================== Card

        public Card GetCard(Card card)
        {
            MyDeckCards.Add(card);

            return card;
        }

        public void Draw()
        {
            if (MyDeckCards.Count == 0 || MyHandCards.Count == Card.MAX_HAND_COUNT || false)
            {
                return;
            }

            Card card = MyDeckCards.Last();

            MyDeckCards.Remove(card);
            MyHandCards.Add(card);

            CardObject.Create(card, new CardHandModule());

            ArrangeAll();
        }

        public void Recycle()
        {
            MyDeckCards.AddRange(MyCemeteryCards);

            MyCemeteryCards.Clear();
        }

        public void ReRoll()
        {
            int count = MyHandCards.Count;

            MyCemeteryCards.AddRange(MyHandCards);

            MyHandCards.Clear();

            for (int i = 0; i < count; i++)
            {
                Draw();
            }
        }

        public void Suffle()
        {
            int index;

            for (int i = 0; i < MyDeckCards.Count; i++)
            {
                index = UnityEngine.Random.Range(i, MyDeckCards.Count);

                (MyDeckCards[i], MyDeckCards[index]) = (MyDeckCards[index], MyDeckCards[i]);
            }
        }

        // ================================================== ????????

        public void TryUse(CardObject cardObject)
        {
            // TODO: Use Mana Cost

            CardTarget_Temp targets = new CardTarget_Temp();

            //OnCardTargetSet?.Invoke(targets);

            if (targets.PlayerTarget == null && targets.EnemyTargets.Count == 0 && false) // false는 임시로 걸어둠
            {
                Selected.Move(Selected.OriginPosition);

                Selected.State = CardState.None;

                EmphasizeAll();
            }
            else
            {
                Destroy(cardObject.gameObject);

                MyCemeteryCards.Add(cardObject.Card); // Temp
                MyHandCards.Remove(cardObject.Card);

                cardObject.Card.Use(targets);
            }

            Selected = null;
        }

        // =========================================================================== CardObject

        // ================================================== Action

        public void EnlargeAll()
        {
            _onEnlarge?.Invoke(this, EventArgs.Empty);
        }

        public void ArrangeAll()
        {
            _onArrange?.Invoke(this, EventArgs.Empty);
        }

        public void EmphasizeAll()
        {
            _onEmphasize?.Invoke(this, EventArgs.Empty);
        }

        public void RefreshAll()
        {
            _onRefresh?.Invoke(this, CardRefreshEventArgs.Empty);
        }

        // =========================================================================== CardManager

        // ================================================== Resource

        private void LoadResource()
        {
            CardPrefab = Resources.Load<GameObject>("Prefabs/Card");

            Database = Resources.Load<CardDatabase>("Data/CardDatabase");
        }
    }

    // ==================================================================================================== CardManagerState

    public enum CardManagerState
    {
        None,

        CanPointerOver,

        CanUse
    }

    // ==================================================================================================== CardLocation

    public enum CardLocation
    {
        None,

        Deck,

        Hand,

        Use,

        Cemetery,

        Exile
    }
}
