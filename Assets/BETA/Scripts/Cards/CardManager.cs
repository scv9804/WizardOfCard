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

        // ==================================================================================================== Field

        // =========================================================================== Card

        [FoldoutGroup("카드")]
        public Library<string, Card> Cards = new Library<string, Card>();

        // =========================================================================== CardObject

        [FoldoutGroup("카드 오브젝트")]
        public Library<string, CardObject> CardObjects = new Library<string, CardObject>();

        [FoldoutGroup("카드 오브젝트")]
        public Dictionary<string, GameObject> CardObjectContainer = new Dictionary<string, GameObject>();

        [FoldoutGroup("카드 오브젝트")]
        public CardObject Selected;

        [FoldoutGroup("카드 오브젝트")]
        public Dictionary<string, CardEventSystems> CardObjectCommands = new Dictionary<string, CardEventSystems>();

        // =========================================================================== GameEvent

        [FoldoutGroup("게임 이벤트")]
        public GameEventString OnAbilityCasted;

        [FoldoutGroup("게임 이벤트")]
        public GameEvent OnActionCancled;

        // =========================================================================== Data

        //[ShowInInspector] [HideReferenceObjectPicker]
        //private CardManagerData _data = new CardManagerData();

        // ==================================================================================================== Property

        // =========================================================================== Card

        //public Library<string, Card> Cards
        //{
        //    get
        //    {
        //        return _data.Cards;
        //    }
        //}

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            //OnGameStarted();

            SetCardUI();
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

                GameManager.OnGameStart -= OnGameStarted;
                GameManager.OnGameStart += OnGameStarted;
                GameManager.OnGameEnd -= OnGameEnded;
                GameManager.OnGameEnd += OnGameEnded;
                GameManager.OnBattleStart -= OnBattleStarted;
                GameManager.OnBattleStart += OnBattleStarted;
                GameManager.OnGameQuit -= OnGameQuited;
                GameManager.OnGameQuit += OnGameQuited;

                SetCategory((category) =>
                {
                    Cards.Add(category);
                }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);

                SetCategory((category) =>
                {
                    CardObjects.Add(category);
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

                GameManager.OnGameStart -= OnGameStarted;
                GameManager.OnBattleStart -= OnBattleStarted;
                GameManager.OnGameQuit -= OnGameQuited;
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
            cardObject.SetParent(category);

            Cards.Add(category, card);
            CardObjects.Add(category, cardObject);
        }

        public void Remove(string category, Card card, bool isEmptyCategoryDelete = true)
        {
            var cardObject = CardObjects[category].Find((target) =>
            {
                return target.Unit == card;
            });

            //cardObject.Log();

            Cards.Remove(category, card, isEmptyCategoryDelete);
            CardObjects.Remove(category, cardObject, isEmptyCategoryDelete);

            Destroy(cardObject.gameObject);
        }

        // =========================================================================== Scene

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            SetCardUI();

            //if (scene.buildIndex > 8) 
            //{
            //    OnBattleStarted();
            //}
        }

        private void OnGameQuited()
        {
            SetCategory((category) =>
            {
                //foreach (var card in Cards[category])
                //{
                //    card.InstanceID.Log();
                //}

                Cards.Clear();
            }, OWN, DECK, HAND, DISCARD, EXCLUDE);
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

            //Cards.Remove(DECK, card, false);
            //Cards.Add(HAND, card);

            Remove(DECK, card, false);
            Add(HAND, card);

            callback?.Invoke(card);

            CardArrange(DECK);
            CardArrange(HAND);
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

            //Cards.Remove(DISCARD, card, false);
            //Cards.Add(DECK, card);

            Remove(DISCARD, card, false);
            Add(DECK, card);

            callback?.Invoke(card);

            CardArrange(DISCARD);
            CardArrange(DECK);
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
            OnActionCancled.Raise();

            Selected = cardObject;

            OnAbilityCasted.Raise(Selected.Unit.Name);
        }

        // =========================================================================== CardObject

        //public void OwnCardArrange()
        //{

        //}

        //public void DeckCardArrange()
        //{
        //    var deck = CardObjectContainer[DECK]?.GetComponent<UI.HandCardUIHandler>();

        //    deck.Refresh();
        //}

        //public void HandCardArrange()
        //{
        //    var hand = CardObjectContainer[HAND]?.GetComponent<UI.HandCardUIHandler>();

        //    hand.Refresh();
        //}

        //public void DiscardCardArrange()
        //{

        //}

        public void CardArrange(string category)
        {
            if (!CardObjectContainer.ContainsKey(category))
            {
                return;
            }

            var cards = CardObjectContainer[category]?.GetComponent<UI.CardUIHandler>();

            cards.Refresh();
        }

        // =========================================================================== GameEvent

        private void OnGameStarted()
        {
            foreach (var serialID in GameManager.Instance.Configs.StartCardSerialID)
            {
                Cards.Add(OWN, new Card(null, serialID));
            }
        }

        private void OnGameEnded()
        {
            SetCategory((category) =>
            {
                Cards[category].Clear();
                CardObjects[category].Clear();
            }, OWN, DECK, HAND, DISCARD, EXCLUDE, SHOP, EVENT, REWARD, TEMPORARY);
        }

        public void OnBattleStarted()
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
                //var cardObject = Visualize(card);

                //CardObjects.Add(HAND, cardObject);
                //cardObject.SetParent(HAND);

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
                foreach (var cardObject in CardObjects[HAND])
                {
                    foreach (var command in CardObjectCommands)
                    {
                        cardObject.Commands[command.Key] = null;
                    }
                }
            }
            else
            {
                StartCoroutine(Draw(1, (card) =>
                {
                    //var cardObject = Visualize(card);

                    //CardObjects.Add(HAND, cardObject);
                    //cardObject.SetParent(HAND);
                }));

                foreach (var cardObject in CardObjects[HAND])
                {
                    //cardObject.State = CardState.NONE;

                    foreach (var command in CardObjectCommands)
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
        }

        public void ResetSelectedCardObject()
        {
            Destroy(Selected.gameObject);

            CardObjects.Remove(HAND, Selected, false);

            Selected = null;
        }

        // =========================================================================== UIEvent

        private void SetCardUI()
        {
            SetCategory((category) =>
            {
                CardObjectContainer[category] = GameObject.Find(category);
            }, OWN, DECK, HAND, DISCARD, EXCLUDE);

            var controller = GameObject.Find("UI Controller")?.GetComponent<UIController>();

            if (controller == null)
            {
                return;
            }

            foreach (var UIComponent in controller.CO)
            {
                CardObjectContainer[UIComponent.Key] = UIComponent.Value;
            }

            //CardObjectContainer[OWN] = controller?.CO[OWN];
            //CardObjectContainer[DECK] = controller?.CO[DECK];
            //CardObjectContainer[HAND] = controller?.CO[HAND];
            //CardObjectContainer[DISCARD] = controller?.CO[DISCARD];
            //CardObjectContainer[EXCLUDE] = controller?.CO[EXCLUDE];

            if (CardObjectContainer[OWN] != null)
            {
                SetOwnCardUI();
            }

            //if (CardObjectContainer[DECK] != null)
            //{
            //    SetOwnCardUI();
            //}

            //if (CardObjectContainer[DISCARD] != null)
            //{
            //    SetOwnCardUI();
            //}
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

                CardObjects.Add(OWN, cardObject);
                cardObject.SetParent(OWN);
                cardObject.Commands = commands;
            }

            CardArrange(OWN);
        }

        //public void SetDeckCardUI()
        //{
        //    var commands = new Dictionary<string, CardEventSystems>()
        //    {
        //        { "ON_POINTER_ENTER", null },
        //        { "ON_POINTER_EXIT", null },
        //        { "ON_POINTER_CLICK", null },
        //        { "ON_BEGIN_DRAG", null },
        //        { "ON_DRAG", null },
        //        { "ON_END_DRAG", null },
        //    };

        //    foreach (var card in Cards[DECK])
        //    {
        //        //var cardObject = Visualize(card);

        //        //CardObjects.Add(OWN, cardObject);
        //        //cardObject.SetParent(OWN);
        //        //cardObject.Commands = commands;
        //    }

        //    //var count = GameObject.Find("Count_TMP").GetComponent<TMPro.TMP_Text>();

        //    //count.text = Cards[OWN].Count.ToString();
        //}

        //public void SetDiscardCardUI()
        //{
        //    var commands = new Dictionary<string, CardEventSystems>()
        //    {
        //        { "ON_POINTER_ENTER", null },
        //        { "ON_POINTER_EXIT", null },
        //        { "ON_POINTER_CLICK", null },
        //        { "ON_BEGIN_DRAG", null },
        //        { "ON_DRAG", null },
        //        { "ON_END_DRAG", null },
        //    };

        //    foreach (var card in Cards[DISCARD])
        //    {
        //        //var cardObject = Visualize(card);

        //        //CardObjects.Add(OWN, cardObject);
        //        //cardObject.SetParent(OWN);
        //        //cardObject.Commands = commands;
        //    }

        //    //var count = GameObject.Find("Count_TMP").GetComponent<TMPro.TMP_Text>();

        //    //count.text = Cards[OWN].Count.ToString();
        //}

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

    public sealed class CardManagerJSON
    {

    }
}
