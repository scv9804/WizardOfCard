using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Linq;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using XSSLG;

namespace WIP
{
    // ==================================================================================================== CardManager

    public class CardManager : MonoSingleton<CardManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        // =========================================================================== CardObject

        public event EventObserver OnCardArrange; // ���ľ� �Ұ� �ʹ� ����...

        // ================================================== Instance

        [Header("���� ���� ī��")]
        [SerializeField] private CardObject _selected;

        // =========================================================================== CardManager

        // ================================================== BETA

        ////////////////////////////////////////////////// BETA
        private Dictionary<KeyCode, Action> _inputs;
        ////////////////////////////////////////////////// BETA

        // ������ ���⼭ �����ؾ� �ϳ�

        // ================================================== BattleMgr

        [Header("��Ʋ �Ŵ���")]
        [SerializeField] private XSBattleMgr _battleMgr;

        // ================================================== State

        [Header("ī��Ŵ��� ����")]
        [SerializeField] private CardManagerState _state = CardManagerState.CanUse;

        // ================================================== Module

        private CardManagerCostModule _costModule = new CardManagerCostModule();

        // ================================================== Data

        [Header("������")]
        [SerializeField] private CardManagerData _data = new CardManagerData();

        // ================================================== Resource

        [Header("ī�� ������")]
        [SerializeField] private GameObject _cardPrefab;

        [Header("�����ͺ��̽�")]
        [SerializeField] private CardDatabase _database;

        [Header("��Ģ ����")]
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

        protected override void Awake()
        {
            base.Awake();

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

        private void OnLevelWasLoaded(int level)
        {
            if (TurnManager.Inst.isCombatScene && Instance == this)
            {
                StartCoroutine(GameSetting());
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

            FindBattleMgr();

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
                if (Deck.Count == 0 || Hand.Count == Settings.MaxHandCount || false) // false�� ���� ��ο� �Ұ� ����� ��...?
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

            FindBattleMgr();

            //if (!Selected.Card.Data.TargetSelf)
            if (Selected.Card.TargetData.IsTargetable)
            {
                IEnumerator select = _battleMgr?.SelectTarget(targets, Selected.Card.TargetData.Radius,
                    Selected.Card.TargetData.Range);
                // ������ :: ���� �� �������� �ϳ׿�... ���� �������� �̰�

                if (select != null)
                {
                    yield return StartCoroutine(select);
                }
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

        private void FindBattleMgr()
        {
            if (_battleMgr == null)
            {
                #region ONLY_UNITY_EDITOR :: "��Ʋ�Ŵ����� null�Դϴ�. �߰��ϰڽ��ϴ�."
#if UNITY_EDITOR
                Debug.Log("��Ʋ�Ŵ����� null�Դϴ�. �߰��ϰڽ��ϴ�.");
#endif 
                #endregion

                _battleMgr = GameObject.Find("main")?.GetComponent<XSBattleMgr>();
            }
        }

        // ================================================== Resource

        private void LoadData()
        {
            CardPrefab = Resources.Load<GameObject>("Prefabs/Card");

            Database = Resources.Load<CardDatabase>("Data/CardDatabase");

            Settings = Resources.Load<CardSettings>("Data/CardSettings");

            Preloading();
        }

        // ���� �ε� �� �ɷ��� �ӽ÷� ����
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

        [Header("����")]
        [SerializeField, JsonProperty("Own")] private CardOwnedPile _owned = new CardOwnedPile();

        [Header("��")]
        [SerializeField, JsonProperty("Deck")] private CardDeckPile _deck = new CardDeckPile();

        [Header("����")]
        [SerializeField, JsonProperty("Hand")] private CardHandPile _hand = new CardHandPile();

        [Header("���")]
        [SerializeField, JsonProperty("Discard")] private CardDiscardPile _discard = new CardDiscardPile();

        [Header("����")]
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