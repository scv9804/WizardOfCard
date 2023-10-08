using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using System;
using System.Linq;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//using XSSLG;
using UnityEngine.Events;

namespace WIP
{
    // ==================================================================================================== CardManager

    public class CardManager : MonoSingleton<CardManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        // =========================================================================== CardObject

        public event EventObserver OnCardArrange; // 고쳐야 할게 너무 많다...

        // ================================================== Instance

        [Header("선택 중인 카드")]
        [SerializeField] private CardObject _selected;

        // =========================================================================== CardManager

        // ================================================== BETA

        ////////////////////////////////////////////////// BETA
        private Dictionary<KeyCode, Action> _inputs;
        ////////////////////////////////////////////////// BETA

        // 마나를 여기서 관리해야 하나

        // ================================================== BattleMgr

        //[Header("배틀 매니저")]
        //[SerializeField] private XSBattleMgr _battleMgr;

        // ================================================== State

        [Header("카드매니저 상태")]
        [SerializeField] private CardManagerState _state = CardManagerState.CanUse;

        // ================================================== Module

        private CardManagerCostModule _costModule = new CardManagerCostModule();

        // ================================================== Data

        [Header("데이터")]
        [SerializeField] private CardManagerData _data = new CardManagerData();

        // ================================================== Resource

        //[Header("카드 프리팹")]
        //[SerializeField] private GameObject _cardPrefab;

        [Header("카드 프리팹")]
        [SerializeField] private CardObject _cardPrefab;

        [Header("데이터베이스")]
        [SerializeField] private CardDatabase _database;

        [Header("규칙 설정")]
        [SerializeField] private CardSettings _settings;

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

        // =========================================================================== CardManager

        // ================================================== Pile

        public CardOwnedPile Owned
        {
            get
            {
                return _data.Owned;
            }
        }

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

        //public GameObject CardPrefab
        //{
        //    get
        //    {
        //        return _cardPrefab;
        //    }

        //    private set
        //    {
        //        _cardPrefab = value;
        //    }
        //}

        public CardObject CardPrefab
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

        protected override void Awake()
        {
            base.Awake();

            // <Scene, LoadSceneMode> sceneLoaded;

            SceneManager.sceneLoaded -= OnSceneWasLoaded;
            SceneManager.sceneLoaded += OnSceneWasLoaded;

            ////////////////////////////////////////////////// BETA
            _inputs = new Dictionary<KeyCode, Action>()
            {
                { KeyCode.Z, () =>
                {
                    string instanceID = GameManager.Instance.Allocate(InstanceType.Card);
                    int index = UnityEngine.Random.Range(0, 2);

                    StartCoroutine(Acquire(Card.Create(instanceID, index), null));
                }},

                { KeyCode.X, () =>
                {
                    //StartCoroutine(Draw(null, Deck.Cards.LastOrDefault()));

                    StartCoroutine(Draw(null));
                }},

                { KeyCode.C, () =>
                {
                    Deck.Suffle();
                }},

                { KeyCode.V, () =>
                {
                    Refill();
                }}
            };
            ////////////////////////////////////////////////// BETA
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }

        private void Update()
        {
            ////////////////////////////////////////////////// BETA
            foreach (var input in _inputs)
            {
                if (Input.GetKeyDown(input.Key))
                {
                    input.Value();
                }
            }
            ////////////////////////////////////////////////// BETA
        }

        // ================================================== Scene

        //private void OnLevelWasLoaded(int level)
        //{
        //    if (TurnManager.Inst.isCombatScene && Instance == this)
        //    {
        //        StartCoroutine(GameSetting());
        //    }
        //}

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            //if (TurnManager.Inst.isCombatScene && Instance == this)
            //{
            //    StartCoroutine(GameSetting());
            //}

            StartCoroutine(GameSetting());
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
            if (Selected != cardObject || State < CardManagerState.CanUse || !Selected.IsUsable || Selected.Pile != Hand || !TurnManager.Inst.myTurn)
            {
                return;
            }

            CostModule.Cost = Selected.Card.Cost.Value;
            CostModule.Estimate();

            if (CostModule.IsEnough)
            {
                Selected.State = CardState.IsDrag;
            }
        }

        public void OnDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || State < CardManagerState.CanUse || Selected.Pile != Hand || !TurnManager.Inst.myTurn)
            {
                return;
            }

            Selected.Move(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {
            if (Selected != cardObject || Selected.Pile != Hand || !TurnManager.Inst.myTurn)
            {
                return;
            }

            if (State == CardManagerState.CanUse && Selected.transform.position.y > Screen.height / 2)
            {
                //Selected.Move(Vector3.zero);

                Selected.State = CardState.IsUse;

                StartCoroutine(Play());
            }
            else
            {
                Selected.Move(Selected.OriginPosition);

                Selected.State = CardState.None;

                Selected = null;
            }
        }

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);

            LoadData();

            Owned.Initialize(Card.DECK_GROUP_NAME, false);
            Deck.Initialize(Card.DECK_GROUP_NAME, false);
            Hand.Initialize(Card.HAND_GROUP_NAME, true);
            Discard.Initialize(Card.DISCARD_GROUP_NAME, false);
            Exiled.Initialize(Card.EXILED_GROUP_NAME, false);

            //FindBattleMgr();

            TurnManager.Inst.Temp_OnChangeTurn += OnTurnChanged;

            StartCoroutine(GameStart());
        }

        // =========================================================================== Card

        public IEnumerator Acquire(Card card, Action<Card> callback)
        {
            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                Deck.Add(card);

                callback?.Invoke(card);

                yield return null;
            }
        }

        public IEnumerator Draw(Action<Card> callback)
        {
            if (Deck.Count == 0)
            {
                yield return StartCoroutine(Refill());
            }

            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                if (Deck.Count == 0 || Hand.Count == Settings.MaxHandCount || false) // false는 추후 드로우 불가 디버프 용...?
                {
                    yield break;
                }

                Card card = Deck.Cards.LastOrDefault();

                Deck.Remove(card);
                Hand.Add(card);

                callback?.Invoke(card);

                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator Recycle(Action<Card> callback)
        {
            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                Card card = Discard.Cards.LastOrDefault();

                Debug.Log(card.InstanceID);

                Discard.Remove(card);
                Deck.Add(card);

                callback?.Invoke(card);

                yield return null;
            }
        }

        public IEnumerator Refill()
        {
            int count = Discard.Count;

            for (int i = 0; i < count; i++)
            {
                yield return StartCoroutine(Recycle(null));
            }
        }

        // ================================================== ????????

        public IEnumerator SetCardTarget(Action success, Action failed)
        {
            CardTarget targets = new CardTarget();

            ////////////////////////////////////////////////// BETA
            //targets.IsActive = false;
            ////////////////////////////////////////////////// BETA

            //FindBattleMgr();

            //if (!Selected.Card.Data.TargetSelf)
            if (Selected.Card.TargetData.IsTargetable)
            {
                //IEnumerator select = _battleMgr?.SelectTarget(targets, Selected.Card.TargetData.Radius,
                //    Selected.Card.TargetData.Range);
                // ㅈㅎㅇ :: 글자 수 어지럽긴 하네여... 언재 정리하지 이거

                //if (select != null)
                //{
                //    yield return StartCoroutine(select);
                //}
			}
			else
            {
                ////////////////////////////////////////////////// BETA
                targets.IsActive = true;
                ////////////////////////////////////////////////// BETA
            }

            if (targets.IsActive)
            {
                success?.Invoke();

                yield return StartCoroutine(Use(Selected.Card, targets));
            }
            else
            {
                failed?.Invoke();
            }

            yield return null;
        }

        public IEnumerator Use(Card card, CardTarget targets)
        {
            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                Hand.Remove(card);

                yield return StartCoroutine(card.Use(targets));

                if ((card.Keyword.Value & CardKeyword.Exile) != 0)
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

        // =========================================================================== CardObject

        // ================================================== ????????

        public IEnumerator Play()
        {
            yield return StartCoroutine(SetCardTarget(() =>
            {
                CostModule.Execute();
            }, () =>
            {
                Selected.State = CardState.None;

                Selected.Move(Selected.OriginPosition);

                CostModule.Clear();
            }));

            Selected = null;
        }

        public void Arrange()
        {
            OnCardArrange?.Invoke(null);
        }

        // =========================================================================== CardManager

        // ================================================== ????????

        public void OnBattleEnd()
        {
            Debug.Log($"Before:: Hands: {Hand.Cards.Count}, Discards: {Discard.Cards.Count}, Exiled: {Exiled.Cards.Count}");

            foreach (var card in Hand.Cards)
            {
                Deck.Add(card);
            }

            foreach (var card in Discard.Cards)
            {
                Deck.Add(card);
            }

            foreach (var card in Exiled.Cards)
            {
                Deck.Add(card);
            }

            Hand.Clear();
            Discard.Clear();
            Exiled.Clear();

            Debug.Log($"After:: Hands: {Hand.Cards.Count}, Discards: {Discard.Cards.Count}, Exiled: {Exiled.Cards.Count}");
        }

        // ================================================== BattleMgr

//        private void FindBattleMgr()
//        {
//            if (_battleMgr == null)
//            {
//                #region ONLY_UNITY_EDITOR :: "배틀매니저가 null입니다. 추가하겠습니다."
//#if UNITY_EDITOR
//                Debug.Log("배틀매니저가 null입니다. 추가하겠습니다.");
//#endif 
//                #endregion

//                _battleMgr = GameObject.Find("main")?.GetComponent<XSBattleMgr>();
//            }
//        }

        // ================================================== Resource

        private void LoadData()
        {
            CardPrefab = Resources.Load<CardObject>("Prefabs/Card");

            Database = Resources.Load<CardDatabase>("Data/CardDatabase");

            Settings = Resources.Load<CardSettings>("Data/CardSettings");

            Preloading();
        }

        // 최초 로딩 렉 걸려서 임시로 만듬
        private void Preloading()
        {
            Destroy(Instantiate(CardPrefab));
        }

        // ================================================== ??????

        public IEnumerator GameSetting()
        {
            yield return StartCoroutine(BattleStart());
        }

        public IEnumerator GameStart()
        {
            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(Acquire(Card.Create(GameManager.Instance.Allocate(InstanceType.Card), 0), null));
                    StartCoroutine(Acquire(Card.Create(GameManager.Instance.Allocate(InstanceType.Card), 1), null));

                    yield return null;
                }
            }
        }

        public IEnumerator BattleStart()
        {
            yield return ProcessManager.Instance.AddTask(Main());

            // ================================================== Main

            IEnumerator Main()
            {
                Hand.Display(true);

                for (int i = 0; i < 4; i++)
                {
                    StartCoroutine(Draw(null));
                }

                yield return null;
            }
        }

        public void OnTurnChanged(IEventParameter parameter)
        {
            parameter.Casting<TurnEventParameter>((turnEvent) =>
            {
                if (turnEvent.IsMyTurn)
                {
                    StartCoroutine(Draw(null));
                }
                else
                {

                }
            });
        }
    }

    // ==================================================================================================== CardManagerData

    [Serializable] public class CardManagerData
    {
        // ==================================================================================================== Field

        // =========================================================================== Pile

        [Header("보유")]
        [SerializeField, JsonProperty("Own")] private CardOwnedPile _owned = new CardOwnedPile();

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

        [JsonIgnore] public CardOwnedPile Owned
        {
            get
            {
                return _owned;
            }
        }

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
    }

    // ==================================================================================================== CardManagerState

    public enum CardManagerState
    {
        None,

        CanPointerOver,

        CanUse
    }
}