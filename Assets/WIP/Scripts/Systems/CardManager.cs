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

        // ================================================== Delegate

        private Action _onRefresh;

        // =========================================================================== CardObject

        // ================================================== Instance

        [Header("선택 중인 카드")]
        [SerializeField] private CardObject _selected;

        // ================================================== Delegate

        private Action _onArrange;

        // =========================================================================== CardManager

        // 마나를 여기서 관리해야 하나

        // ================================================== State

        [Header("카드매니저 상태")]
        [SerializeField] private CardManagerState _state = CardManagerState.CanUse;

        // ================================================== Module

        private CardManagerCostModule _costModule = new CardManagerCostModule();

        // ================================================== Data

        [Header("데이터")]
        [SerializeField] private CardManagerData _data = new CardManagerData();

        // ================================================== Resource

        [Header("카드 프리팹")]
        [SerializeField] private GameObject _cardPrefab;

        [Header("데이터베이스")]
        [SerializeField] private CardDatabase _database;

        [Header("규칙 설정")]
        [SerializeField] private CardSettings _settings;

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        [Header("배틀 매니저")]
        private XSSLG.XSBattleMgr battleMgr;

        protected override string Name
        {
            get
            {
                return "Card Manager";
            }
        }

        // =========================================================================== Card

        // ================================================== Delegate

        public event Action OnRefresh
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

        // ================================================== Delegate

        public event Action OnArrange
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

        // =========================================================================== CardManager

        // ================================================== Pile

        public CardDeckPile Deck
        {
            get
            {
                return _data.Deck;
            }
        }

        public CardHandPile Hand
        {
            get
            {
                return _data.Hand;
            }
        }

        public CardDiscardPile Discard
        {
            get
            {
                return _data.Discard;
            }
        }

        public CardExiledPile Exiled
        {
            get
            {
                return _data.Exiled;
            }
        }

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

        // ================================================== Module

        public CardManagerCostModule CostModule
        {
            get
            {
                return _costModule;
            }

            private set
            {
                _costModule = value;
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

        public CardSettings Settings
        {
            get
            {
                return _settings;
            }

            private set
            {
                _settings = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Update()
        {
            ////////////////////////////////////////////////// BETA
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GetCard(Card.Create(GameManager.Instance.Allocate(InstanceType.Card), 0));
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Draw(null);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Deck.Suffle();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                int count = Discard.Count;

                for (int i = 0; i < count; i++)
                {
                    StartCoroutine(Recycle(null));
                }
            }
            ////////////////////////////////////////////////// BETA
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
        }

        public void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || Selected.State != CardState.IsPointerOver)
            {
                return;
            }

            Selected.State = CardState.None;

            Selected = null;
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || State < CardManagerState.CanUse || !Selected.IsUsable || Selected.Pile != Hand)
            {
                return;
            }

            CostModule.Cost = Selected.GetCard().Cost;
            CostModule.Estimate();

            if (CostModule.IsEnough)
            {
                Selected.State = CardState.IsDrag;
            }
        }

        public void OnDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || State < CardManagerState.CanUse || Selected.Pile != Hand)
            {
                return;
            }

            Selected.Move(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || Selected.Pile != Hand)
            {
                return;
            }

            if (State == CardManagerState.CanUse && Selected.transform.position.y > Screen.height / 2)
            {
                //Selected.Move(Vector3.zero);

                Selected.State = CardState.IsUse;

                StartCoroutine(SetCardTarget());
            }
            else
            {
                Selected.Move(Selected.OriginPosition);

                Selected.State = CardState.None;
            }

            Selected = null;
        }

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);

            LoadData();
            _data.Initialize();
        }

        // =========================================================================== Card

        public Card GetCard(Card card)
        {
            Deck.Add(card);

            return card;
        }

        public Card Draw(Action<Card> callback)
        {
            Card card;

            if (Deck.Cards.Count > 0 && Hand.Cards.Count < Settings.MaxHandCount && true)
            {
                card = Deck.Cards.Last();

                Deck.Remove(card);
                Hand.Add(card);

                callback?.Invoke(card);

                Arrange();
            }
            else
            {
                card = null;
            }

            return card;
        }

        public IEnumerator Recycle(Action<Card> callback)
        {
            yield return ProcessManager.Instance.AddTask(null, Main());

            IEnumerator Main()
            {
                int index = UnityEngine.Random.Range(0, Discard.Count);

                Card card = Discard.Cards[index];

                Discard.Remove(card);
                Deck.Add(card);

                callback?.Invoke(card);

                yield return null;
            }
        }

        // ================================================== ????????

        private IEnumerator SetCardTarget()
        {
            CardTarget targets = new CardTarget();

            ////////////////////////////////////////////////// BETA
            //targets.IsActive = true;
            ////////////////////////////////////////////////// BETA

            Selected.enabled = false;

            if (battleMgr == null)
			{
#if UNITY_EDITOR
                Debug.Log("배틀매니저가 null입니다. 추가하겠습니다.");
#endif
                battleMgr = GameObject.Find("main").GetComponent<XSSLG.XSBattleMgr>();
            }
           
            yield return StartCoroutine(battleMgr.SelectTraget(targets));
          
            if (!targets.IsActive)
            {
                Selected.enabled = true;

                Selected.State = CardState.None;

                Selected.Move(Selected.OriginPosition);
            }
            else
            {
                StartCoroutine(Use(Selected.GetCard(), targets));
            }

            Selected = null;
        }

        public IEnumerator Use(Card card, CardTarget targets)
        {
            yield return ProcessManager.Instance.AddTask(Prework(), Main());

            IEnumerator Prework()
            {
                CostModule.Execute();

                yield return null;
            }

            IEnumerator Main()
            {
                Hand.Remove(card);

                yield return StartCoroutine(card.Use(targets));

                if ((card.Keyword & CardKeyword.Exile) != 0)
                {
                    Exiled.Add(card);
                }
                else
                {
                    Discard.Add(card);
                }

                yield return null;
            }
        }

        // ================================================== Action

        public void Refresh()
        {
            _onRefresh?.Invoke();
        }

        // =========================================================================== CardObject

        public void Arrange()
        {
            _onArrange.Invoke();
        }

        // =========================================================================== CardManager

        // ================================================== Resource

        private void LoadData()
        {
            CardPrefab = Resources.Load<GameObject>("Prefabs/Card");

            Database = Resources.Load<CardDatabase>("Data/CardDatabase");

            Settings = Resources.Load<CardSettings>("Data/CardSettings");
        }
    }

    // ==================================================================================================== CardManagerData

    [Serializable] public class CardManagerData
    {
        // ==================================================================================================== Field

        // =========================================================================== Pile

        [Header("덱")]
        [SerializeField, JsonProperty("Deck")] private CardDeckPile _deck = new CardDeckPile();

        [Header("손패")]
        [SerializeField, JsonProperty("Hand")] private CardHandPile _hand = new CardHandPile();

        [Header("사용")]
        [SerializeField, JsonProperty("Discard")] private CardDiscardPile _discard = new CardDiscardPile();

        [Header("제외")]
        [SerializeField, JsonProperty("Exiled")] private CardExiledPile _exiled = new CardExiledPile();

        // ==================================================================================================== Property

        // =========================================================================== Pile

        [JsonIgnore] public CardDeckPile Deck
        {
            get
            {
                return _deck;
            }
        }

        [JsonIgnore] public CardHandPile Hand
        {
            get
            {
                return _hand;
            }
        }

        [JsonIgnore] public CardDiscardPile Discard
        {
            get
            {
                return _discard;
            }
        }

        [JsonIgnore] public CardExiledPile Exiled
        {
            get
            {
                return _exiled;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Instance

        public void Initialize()
        {
            Deck.Initialize(Card.DECK_GROUP_NAME, false);
            Hand.Initialize(Card.HAND_GROUP_NAME, true);
            Discard.Initialize(Card.DISCARD_GROUP_NAME, false);
            Exiled.Initialize(Card.EXILED_GROUP_NAME, false);
        }
    }

    // ==================================================================================================== CardManagerState

    public enum CardManagerState
    {
        None,

        CanPointerOver,

        CanUse
    }
}