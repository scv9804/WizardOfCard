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

        // ================================================== Collection

        [Header("덱 카드 리스트")]
        [SerializeField] private List<Card> _myDeckCards = new List<Card>();

        [Header("손패 카드 리스트")]
        [SerializeField] private List<Card> _myHandCards = new List<Card>();

        [Header("묘지 카드 리스트")]
        [SerializeField] private List<Card> _myCemeteryCards = new List<Card>();

        [Header("제외 카드 리스트")]
        [SerializeField] private List<Card> _myExiledCards = new List<Card>();

        // Deck, Hand, Discard, Exiled

        // ================================================== Action

        private Action _onRefresh;

        // =========================================================================== CardObject

        // ================================================== Instance

        [Header("선택 중인 카드")]
        [SerializeField] private CardObject _selected;

        // ================================================== Action

        private Action _onEnlarge;
        private Action _onArrange;
        private Action _onEmphasize;

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

        protected override string Name
        {
            get
            {
                return "Card Manager";
            }
        }

        // =========================================================================== Card

        // ================================================== Collection

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

        // ================================================== Action

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

        // ================================================== Action

        public event Action OnEnlarge
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

        public event Action OnEmphasize
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

            LoadData();
        }

        private void Update()
        {
            //////////////////////////////////////////////////

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
                StartCoroutine(Recycle());
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(ProcessManager.Instance.Terminate()); // 이거 옮기기 귀찮아...
            }

            //////////////////////////////////////////////////
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

            CostModule.Cost = Selected.Card.Cost;
            CostModule.Estimate();

            if (CostModule.IsEnough)
            {
                Selected.State = CardState.IsDrag;

                EnlargeAll();
            }
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
                Selected.Move(new Vector3(0.0f, 0.0f, 0.0f));

                Selected.State = CardState.IsUse;

                SetCardTarget();

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

            Suffle();

            return card;
        }

        public Card Draw()
        {
            if (MyDeckCards.Count > 0 && MyHandCards.Count < Settings.MaxHandCount && true)
            {
                Card card = MyDeckCards.Last();

                MyDeckCards.Remove(card);
                MyHandCards.Add(card);

                CardObject.Create(card, new CardHandModule());

                ArrangeAll();

                return card;
            }
            else
            {
                return null;
            }
        }

        public IEnumerator Recycle()
        {
            yield return ProcessManager.Instance.AddTask(null, Main());

            IEnumerator Main()
            {
                if (MyCemeteryCards.Count > 0)
                {
                    MyDeckCards.AddRange(MyCemeteryCards);

                    // TODO: Destroy All Cemetery CardObjects

                    MyCemeteryCards.Clear();

                    Suffle();
                }

                yield return null;
            }
        }

        private void Suffle()
        {
            int index;

            for (int i = 0; i < MyDeckCards.Count; i++)
            {
                index = UnityEngine.Random.Range(i, MyDeckCards.Count);

                (MyDeckCards[i], MyDeckCards[index]) = (MyDeckCards[index], MyDeckCards[i]);
            }
        }

        // ================================================== ????????

        private void SetCardTarget()
        {
            CardTarget targets = new CardTarget();

            //////////////////////////////////////////////////
            targets.IsActive = true;
            //////////////////////////////////////////////////

            Selected.enabled = false;

            if (!targets.IsActive)
            {
                Selected.enabled = true;

                Selected.State = CardState.None;

                Selected.Move(Selected.OriginPosition);

                EmphasizeAll();
            }
            else
            {
                StartCoroutine(Use(Selected.Card, targets));

                Selected.Dispose();
            }

            Selected = null;
        }

        public IEnumerator Use(Card card, CardTarget targets)
        {
            yield return ProcessManager.Instance.AddTask(Prework(), Main());

            IEnumerator Prework()
            {
                CostModule.Execute();

                MyHandCards.Remove(card);

                yield return null;
            }

            IEnumerator Main()
            {
                yield return StartCoroutine(card.Use(targets));

                if ((card.Keyword & CardKeyword.Exile) != 0)
                {
                    MyExiledCards.Add(card);
                }
                else
                {
                    MyCemeteryCards.Add(card);
                }

                yield return null;
            }
        }

        // ================================================== Action

        public void RefreshAll()
        {
            _onRefresh?.Invoke();
        }

        // =========================================================================== CardObject

        // ================================================== Action

        public void EnlargeAll()
        {
            _onEnlarge?.Invoke();
        }

        public void ArrangeAll()
        {
            _onArrange?.Invoke();
        }

        public void EmphasizeAll()
        {
            _onEmphasize?.Invoke();
        }

        // =========================================================================== CardManager

        // ================================================== Process

        //public void AddTask(IEnumerator task) // 이거 턴 매니저나 게임 매니저나 태스크 매니저 3개 중 하나로 옮겨야 되나
        //{
        //    if (!_isRunning)
        //    {
        //        return;
        //    }

        //    Process.Add(task);

        //    StartCoroutine(task);
        //}

        //public IEnumerator WaitForProcess(string task) // string은 조만간 지울 예정
        //{
        //    int standby = Process.Count - 1;

        //    Debug.Log($"{task}의 처리 순번: {standby}");

        //    yield return new WaitUntil(() =>
        //    {
        //        return standby == Standby;
        //    });
        //}

        //public IEnumerator Terminate()
        //{
        //    _isRunning = false;

        //    yield return StartCoroutine(WaitForProcess("작업 대기열 초기화"));

        //    Process.Clear();

        //    Standby = 0;

        //    _isRunning = true;

        //    yield return null;
        //}

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

    }

    // ==================================================================================================== CardPile

    [Serializable] public class CardPile
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        [Header("카드 리스트")]
        [SerializeField, JsonProperty("Cards")] private List<Card> _cards = new List<Card>();

        // =========================================================================== CardObject

        [Header("카드 오브젝트 리스트")]
        [SerializeField, JsonIgnore] private List<CardObject> _cardObjects = new List<CardObject>();

        [Header("카드 오브젝트 활성화 여부")]
        [SerializeField, JsonIgnore] private bool _isDisplay;

        // =========================================================================== Transform

        // ================================================== Sibling Index

        [Header("카드 그룹 이름")]
        [SerializeField, JsonProperty("GroupName")] private string _groupName;

        // ================================================== Position

        private Func<int, int, float> _getX;
        private Func<int, int, float> _getY;

        // ==================================================================================================== Property

        // =========================================================================== Card

        public List<Card> Cards
        {
            get
            {
                return _cards;
            }
        }

        // =========================================================================== CardObject

        public List<CardObject> CardsObjects
        {
            get
            {
                return _cardObjects;
            }
        }

        public bool IsDisplay
        {
            get
            {
                return _isDisplay;
            }

            set
            {
                _isDisplay = value;
            }
        }

        // =========================================================================== Transform

        // ================================================== Position

        public event Func<int, int, float> GetX
        {
            add
            {
                _getX = value;
            }

            remove
            {
                _getX -= value;
            }
        }

        public event Func<int, int, float> GetY
        {
            add
            {
                _getY = value;
            }

            remove
            {
                _getY -= value;
            }
        }

        // ==================================================================================================== Method

        // ================================================== Position

        public (int Count, int Index) GetIndexElement(CardObject cardObject)
        {
            return (Cards.Count, CardsObjects.IndexOf(cardObject));
        }

        public Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 4;

            return new Vector3(x, y, 0.0f);
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