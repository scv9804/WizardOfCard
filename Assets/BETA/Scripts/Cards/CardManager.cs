using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;
using BETA.Singleton;
using BETA.UI;

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

        // ==================================================================================================== Field

        // =========================================================================== Card

        [FoldoutGroup("ī��")]
        public Library<string, Card> Cards = new Library<string, Card>();

        public bool IsPlayerTurn;

        // =========================================================================== CardObject

        [FoldoutGroup("ī�� ������Ʈ")]
        public Library<string, CardObject> CardObjects = new Library<string, CardObject>();

        [FoldoutGroup("ī�� ������Ʈ")]
        public Dictionary<string, GameObject> CardObjectContainer = new Dictionary<string, GameObject>();

        [FoldoutGroup("ī�� ������Ʈ")]
        public Dictionary<string, CardEventSystems> HandCardCommands = new Dictionary<string, CardEventSystems>();

        [FoldoutGroup("ī�� ������Ʈ")]
        public CardObject Selected;

        // =========================================================================== GameEvent

        [FoldoutGroup("���� �̺�Ʈ")]
        public GameEventString OnAbilityCasted;

        [FoldoutGroup("���� �̺�Ʈ")]
        public GameEvent OnActionCancled;

        [SerializeField, TitleGroup("ī��Ŵ��� �̺�Ʈ")]
        private CardManagerEvent _events;

        // ==================================================================================================== Property

        // =========================================================================== Card

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void OnEnable()
        {
            _events.OnGameQuit.Listener += OnGameQuit;

            _events.OnGameStart.Listener += OnGameStart;
            _events.OnGameEnd.Listener += OnGameEnd;

            _events.OnBattleStart.Listener += OnBattleStart;
        }

        private void OnDisable()
        {
            _events.OnGameQuit.Listener -= OnGameQuit;

            _events.OnGameStart.Listener -= OnGameStart;
            _events.OnGameEnd.Listener -= OnGameEnd;

            _events.OnBattleStart.Listener -= OnBattleStart;
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
                    CardObjects.Add(category);
                    CardObjectContainer.Add(category, null);
                }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);
            }

            return isEmpty;
        }

        protected override bool Finalize()
        {
            var isEmpty = base.Finalize();

            if (!isEmpty)
            {
                SceneManager.sceneLoaded -= OnSceneWasLoaded;
            }

            return isEmpty;
        }

        // =========================================================================== TEMP

        public CardObject Visualize(Card card)
        {
            var cardObject = Instantiate(Card.DataSet.Prefab);

            cardObject.Unit = card;

            card.OnDestroy += cardObject.Destroy;

            return cardObject;
        }

        public void Add(string category, Card card)
        {
            var cardObject = Visualize(card);
            //cardObject.SetParent(category);

            Cards.Add(category, card);
            CardObjects.Add(category, cardObject);
        }

        public void Remove(string category, Card card, bool isEmptyCategoryDelete = true)
        {
            var cardObject = CardObjects[category].Find((target) =>
            {
                return target.Unit == card;
            });

            Cards.Remove(category, card, isEmptyCategoryDelete);
            CardObjects.Remove(category, cardObject, isEmptyCategoryDelete);

            Destroy(cardObject?.gameObject);
        }

        // =========================================================================== Scene

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            SetCardUI();
        }

        private void OnGameQuit()
        {
            SetCategory((category) =>
            {
                Cards.Clear();
            }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);
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

        // ���� ���� �� isChecking�� ������ true!!
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

            //Cards.Remove(DECK, card, false);
            //Cards.Add(HAND, card);

            Remove(DECK, card, false);
            Add(HAND, card);

            callback?.Invoke(card);

            CardArrange(DECK);
            CardArrange(HAND);

            //CardArrange();
        }

        // ���� ���� �� isChecking�� ������ true!!
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

        // ���� ���� �� isChecking�� ������ true!!
        public void Recycle(Card card, Action<Card> callback, bool isChecking = true)
        {
            if (isChecking)
            {
                if (Conditions(IsDiscardedCardEmpty))
                {
                    return;
                }
            }

            //Cards.Remove(DISCARD, card, false);
            //Cards.Add(DECK, card);

            Remove(DISCARD, card, false);
            Add(DECK, card);

            callback?.Invoke(card);

            CardArrange(DISCARD);
            CardArrange(DECK);

            //CardArrange();
        }

        // ���� ���� �� isChecking�� ������ true!!
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
            OnActionCancled.Raise();

            Selected = cardObject;

            OnAbilityCasted.Raise(Selected.Unit.Name);
        }

        // =========================================================================== CardObject

        public void CardArrange(string category)
        {
            if (!CardObjectContainer.ContainsKey(category))
            {
                return;
            }

            var cards = CardObjectContainer[category]?.GetComponent<CardUIHandler>();

            cards.Refresh();
        }

        //public void CardArrange()
        //{
        //    _events.OnCardArrange.Launch();
        //}

        // =========================================================================== GameEvent

        private void OnGameStart()
        {
            foreach (var serialID in GameManager.Instance.Configs.StartCardSerialID)
            {
                Cards.Add(OWN, new Card(null, serialID));
            }

            SetOwnCardUI();
        }

        private void OnGameEnd()
        {
            SetCategory((category) =>
            {
                //foreach (var cardObject in CardObjects[category])
                //{
                //    Destroy(cardObject?.gameObject);
                //}

                Cards[category].Clear();
                CardObjects[category].Clear();
            }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);
        }

        public void OnBattleStart()
        {
            CardObjects[OWN].Clear();

            foreach (var card in Cards[OWN])
            {
                //Cards.Add(DECK, card);
                Add(DECK, card);
            }

            Shuffle();

            int index = 0;

            StartCoroutine(Draw(6, (card) =>
            {
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
            var entity = character.GetComponent<Entity>();

            if (entity.teamID != 1)
            {
                IsPlayerTurn = false;

                foreach (var cardObject in CardObjects[HAND])
                {
                    foreach (var command in HandCardCommands)
                    {
                        cardObject.Commands[command.Key] = null;
                    }
                }
            }
            else
            {
                IsPlayerTurn = true;

                StartCoroutine(Draw(1, null));

                foreach (var cardObject in CardObjects[HAND])
                {
                    //cardObject.State = CardState.NONE;

                    foreach (var command in HandCardCommands)
                    {
                        cardObject.Commands[command.Key] = command.Value;
                    }
                }
            }
        }

        public void OnTurnEnd()
        {
            foreach (var cardObject in CardObjects[HAND])
            {
                cardObject.State = CardState.UNABLE;
            }
        }

        public void OnCardAbilityCasted(string name)
        {
            Selected.gameObject.SetActive(false);
        }

        public void OnActionButtonPressed()
        {
            if (Selected != null)
            {
                var card = Selected.Unit as Card;

                //Cards.Remove(HAND, card, false);
                //Cards.Add(DISCARD, card);

                Remove(HAND, card, false);
                Add(DISCARD, card);

                ResetSelectedCardObject();
            }

            CardArrange(HAND);
            CardArrange(DISCARD);

            //CardArrange();
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

            CardArrange(HAND);

            //CardArrange();
        }

        public void ResetSelectedCardObject()
        {
            Destroy(Selected.gameObject);

            CardObjects.Remove(HAND, Selected, false);

            Selected = null;
        }

        public void CardBuy(CardObject cardObject)
        {
            _events.OnCardBuy.Launch(cardObject);
        }

        // =========================================================================== UIEvent

        private void SetCardUI()
        {
            var controller = GameObject.Find("Card UI Controller")?.GetComponent<UIController>();

            controller.Require(() =>
            {
                SetCategory((category) =>
                {
                    CardObjects[category].Clear();
                    CardObjectContainer[category] = null;
                }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);

                foreach (var conponent in controller.CO)
                {
                    CardObjectContainer[conponent.Key] = conponent.Value;
                }

                if (CardObjectContainer[OWN] != null)
                {
                    SetOwnCardUI();
                }

                if (CardObjectContainer[SHOP] != null)
                {
                    SetShopCardUI();
                }
            });
        }

        public void SetOwnCardUI()
        {
            var commands = new Dictionary<string, CardEventSystems>()
            {
                { "ON_POINTER_ENTER", null },
                { "ON_POINTER_EXIT", null },
                { "ON_POINTER_CLICK", null },
                { "ON_BEGIN_DRAG", null },
                { "ON_DRAG", null },
                { "ON_END_DRAG", null },
            };

            foreach (var card in Cards[OWN])
            {
                var cardObject = Visualize(card);

                cardObject.Commands = commands;

                CardObjects.Add(OWN, cardObject);
                cardObject.SetParent(OWN);
            }

            CardArrange(OWN);

            //CardArrange();
        }

        public void SetShopCardUI()
        {
            if (CardObjects[SHOP].Count > 0)
            {
                return;
            }

            foreach (var card in Cards[SHOP])
            {
                var cardObject = Visualize(card);

                CardObjects.Add(SHOP, cardObject);
                cardObject.SetParent(SHOP);
            }

            CardArrange(SHOP);
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

    // ==================================================================================================== CardManagerJSON

    public sealed class CardManagerJSON
    {

    }
}
