using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;
using BETA.Singleton;

using Sirenix.OdinInspector;

using System;
using System.Linq;

using TacticsToolkit;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace BETA
{
    // ==================================================================================================== CardManager

    public sealed class CardManager : SingletonMonoBehaviour<CardManager>
    {
        // ==================================================================================================== Constance

        // =========================================================================== Card

        public const string OWN = "OWN";
        public const string DECK = "DECK";
        public const string HAND = "HAND";
        public const string DISCARD = "DISCARD";
        public const string EXCLUDE = "EXCLUDE";

        public const string SHOP = "SHOP";
        public const string EVENT = "EVENT";
        public const string REWARD = "REWARD";

        public const string TEMPORARY = "TEMPORARY";

        // =========================================================================== GameEvent

        public const string EVENT_PORT = "CardManagerEventPort";

        // ==================================================================================================== Field

        // =========================================================================== Card

        // =========================================================================== CardObject

        [FoldoutGroup("카드 오브젝트")]
        public Library<string, CardObject> CardObjects = new Library<string, CardObject>();

        [FoldoutGroup("카드 오브젝트")]
        public Dictionary<string, GameObject> CardObjectContainer = new Dictionary<string, GameObject>();

        [FoldoutGroup("카드 오브젝트")]
        public CardObject Selected;

        // =========================================================================== GameEvent

        [FoldoutGroup("게임 이벤트")]
        public GameEventString OnAbilityCasted;

        // =========================================================================== Data

        [ShowInInspector] [HideReferenceObjectPicker]
        private CardManagerData _data = new CardManagerData();

        // ==================================================================================================== Property

        // =========================================================================== Card

        public Library<string, Card> Cards
        {
            get
            {
                return _data.Cards;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            OnGameStarted();
        }

        private void Update()
        {
            var center = CardObjects[HAND].Count * -0.5f + 0.5f;

            for (var i = 0; i < CardObjects[HAND].Count; i++)
            {
                if (CardObjects[HAND, i].State >= CardState.ON_POINTER_OVER)
                {
                    continue;
                }

                CardObjects[HAND, i].transform.localPosition = new Vector3((center + i) * (125.0f - CardObjects[HAND].Count * 5.0f), 0.0f, 0.0f);
            }

            CardObjects[HAND].Sort((cardObject) =>
            {
                return cardObject.State >= CardState.ON_POINTER_OVER;
            });
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Card Manager";

                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded -= OnSceneWasLoaded;
                SceneManager.sceneLoaded += OnSceneWasLoaded;

                SetCategory((category) =>
                {
                    Cards.Add(category);
                }, OWN, DECK, HAND, DISCARD, EXCLUDE);

                SetCategory((category) =>
                {
                    CardObjects.Add(category);
                }, OWN, DECK, HAND, DISCARD, EXCLUDE);
            }

            return isEmpty;
        }

        // =========================================================================== TEMP

        public CardObject Visualize(Card card)
        {
            var cardObject = Instantiate(Card.DataSet.Prefab, CardObjectContainer[HAND].transform);

            cardObject.Unit = card;

            card.OnDestroy += cardObject.Destroy;

            return cardObject;
        }

        // =========================================================================== Scene

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            SetCategory((category) =>
            {
                CardObjectContainer[category] = GameObject.Find(category);
            }, OWN, DECK, HAND, DISCARD, EXCLUDE);

            if (scene.buildIndex > 20) 
            {
                OnBattleStarted();
            }
        }

        // =========================================================================== Card

        // ================================================== Library

        public bool IsDeckCardEmpty()
        {
            return Cards[DECK].Count == 0;
        }

        public bool IsHandCardEmpty()
        {
            return Cards[HAND].Count == 0;
        }

        public bool IsHandCardFull()
        {
            return Cards[HAND].Count == GameManager.Instance.Configs.MaxHandCount;
        }

        public bool IsDiscardedCardEmpty()
        {
            return Cards[DISCARD].Count == 0;
        }

        public bool IsExcludedCardEmpty()
        {
            return Cards[EXCLUDE].Count == 0;
        }

        // ================================================== Action

        // 최초 진입 시 isChecking는 무조건 true!!
        public IEnumerator Draw(Card card, Action<Card> callback, bool isChecking = true)
        {
            if (isChecking)
            {
                if (IsDeckCardEmpty())
                {
                    Recycle(Cards[DISCARD].Count, null);
                }

                if (Conditions(IsDeckCardEmpty, IsHandCardFull) || false)
                {
                    yield break;
                }
            }

            Cards.Remove(DECK, card, false);
            Cards.Add(HAND, card);

            callback?.Invoke(card);
        }

        // 최초 진입 시 isChecking는 무조건 true!!
        public IEnumerator Draw(int count, Action<Card> callback, bool isChecking = true)
        {
            if (isChecking)
            {
                if (IsDeckCardEmpty())
                {
                    Recycle(Cards[DISCARD].Count, null);
                }

                if (Conditions(IsDeckCardEmpty, IsHandCardFull) || false)
                {
                    yield break;
                }
            }

            var maximum = Math.Min(Cards[DECK].Count, GameManager.Instance.Configs.MaxHandCount - Cards[HAND].Count);

            count = Math.Min(count, maximum);

            for (var i = 0; i < count; i++)
            {
                var card = Cards[DECK].LastOrDefault();

                yield return StartCoroutine(Draw(card, callback, false));

                yield return new WaitForSeconds(0.1f);
            }
        }

        // 최초 진입 시 isChecking는 무조건 true!!
        public void Recycle(Card card, Action<Card> callback, bool isChecking = true)
        {
            if (isChecking)
            {
                if (Conditions(IsDiscardedCardEmpty))
                {
                    return;
                }
            }

            Cards.Remove(DISCARD, card, false);
            Cards.Add(DECK, card);

            callback?.Invoke(card);
        }

        // 최초 진입 시 isChecking는 무조건 true!!
        public void Recycle(int count, Action<Card> callback, bool isChecking = true)
        {
            if (isChecking)
            {
                if (Conditions(IsDiscardedCardEmpty))
                {
                    return;
                }
            }

            count = Math.Min(count, Cards[DISCARD].Count);

            for (var i = 0; i < count; i++)
            {
                var index = UnityEngine.Random.Range(0, Cards[DISCARD].Count);

                var card = Cards[DISCARD, index];

                Recycle(card, callback , false);
            }
        }

        public void Shuffle()
        {
            if (Conditions(IsDeckCardEmpty))
            {
                return;
            }

            for (var i = 0; i < Cards[DECK].Count; i++)
            {
                var index = UnityEngine.Random.Range(0, Cards[DECK].Count);

                (Cards[DECK, i], Cards[DECK, index]) = (Cards[DECK, index], Cards[DECK, i]);
            }
        }

        public void Play(CardObject cardObject)
        {
            if (Selected != null)
            {
                OnActionButtonCanceled();
            }

            Selected = cardObject;

            //var scriptableData = DataManager.Instance.GetDataSet<CardDataSet>().Data[cardObject.SerialID];

            //OnAbilityCasted.Raise(scriptableData.Name);

            OnAbilityCasted.Raise(Selected.Unit.Name);

            Selected.gameObject.SetActive(false);
        }

        // =========================================================================== GameEvent
        private void OnGameStarted()
        {
            foreach (var serialID in GameManager.Instance.Configs.StartCardSerialID)
            {
                Cards.Add(OWN, new Card(DataManager.Instance.Allocate(), serialID));
            }
        }
        public void OnBattleStarted()
        {
            foreach (var card in Cards[OWN])
            {
                Cards.Add(DECK, card);
            }

            Shuffle();

            int index = 0;

            StartCoroutine(Draw(6, (card) =>
            {
                CardObjects.Add(HAND, Visualize(card));

                if (index < 3)
                {
                    card.Upgrade(2);
                }

                index += 1;
            }));
        }

        public void OnBattleEnd()
        {
            SetCategory((category) =>
            {
                foreach (var cardObject in CardObjects[category])
                {
                    Destroy(cardObject.gameObject);
                }

                CardObjects[category].Clear();
                Cards[category].Clear();

            }, DECK, HAND, DISCARD, EXCLUDE);
        }

        public void OnTurnStart(GameObject character)
        {
            var entity = character.GetComponent<TacticsToolkit.Entity>();

            if (entity.teamID != 1)
            {
                return;
            }

            StartCoroutine(Draw(1, (card) =>
            {
                CardObjects.Add(HAND, Visualize(card));
            }));
        }

        public void OnCardAbilityCasted(int serialID)
        {
            OverlayController.Instance.ClearTiles(null);

            // 대충 카드 스킬 효과 가져오는 코드

            // 대충 마나 맞으면 효과 범위 보여준다는 코드
        }

        public void OnActionButtonPressed()
        {
            if (Selected == null)
            {
                return;
            }

            var card = Selected.Unit as Card;

            Cards.Remove(HAND, card, false);
            Cards.Add(DISCARD, card);

            ResetSelectedCardObject();
        }

        public void OnActionButtonCanceled()
        {
            if (Selected == null)
            {
                return;
            }

            Selected.gameObject.SetActive(true);

            Selected.transform.position = Selected.OriginPosition;
            Selected.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            Selected = null;
        }

        public void ResetSelectedCardObject()
        {
            Destroy(Selected.gameObject);

            CardObjects.Remove(HAND, Selected, false);

            Selected = null;
        }

        // =========================================================================== Utility

        public void SetCategory(Action<string> action, params string[] categories)
        {
            foreach (var category in categories)
            {
                action.Invoke(category);
            }
        }

        private bool Conditions(params Func<bool>[] matches)
        {
            var result = true;

            foreach (var match in matches)
            {
                result = match.Invoke();
            }

            return result;
        } 
    }

    // ==================================================================================================== CardManagerData

    public sealed class CardManagerData
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        public Library<string, Card> Cards = new Library<string, Card>();
    }
}
