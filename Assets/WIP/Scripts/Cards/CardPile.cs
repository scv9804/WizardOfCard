using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace WIP
{
    // ==================================================================================================== CardPile

    [Serializable] public abstract class CardPile
    {
        // ==================================================================================================== Field

        // =========================================================================== Pile

        // ================================================== Count

        private Data<int> _count = new Data<int>();

        // =========================================================================== Card

        // ================================================== Instance

        [Header("카드 리스트")]
        [SerializeField, JsonProperty("Cards")] private List<Card> _cards = new List<Card>();

        // =========================================================================== CardObject

        // ================================================== Instance

        [Header("카드 오브젝트 리스트")]
        [SerializeField, JsonIgnore] private List<CardObject> _cardObjects = new List<CardObject>();

        [Header("카드 오브젝트 활성화 여부")]
        [SerializeField, JsonIgnore] private bool _isDisplay;

        // =========================================================================== Transform

        // ================================================== Sibling Index

        [Header("카드 그룹 이름")]
        [SerializeField, JsonProperty("Name")] private string _name;

        // ==================================================================================================== Property

        // =========================================================================== Pile

        [JsonIgnore] public Data<int> Count
        {
            get
            {
                return _count;
            }

            private set
            {
                _count = value;
            }
        }

        // =========================================================================== Card

        // ================================================== Instance

        [JsonIgnore] public List<Card> Cards
        {
            get
            {
                return _cards;
            }
        }

        // =========================================================================== CardObject

        // ================================================== Instance

        [JsonIgnore] protected List<CardObject> CardObjects
        {
            get
            {
                return _cardObjects;
            }
        }

        [JsonIgnore] public bool IsDisplay
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

        // ================================================== Sibling Index

        [JsonIgnore] public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Pile

        public void Initialize(string name, bool isDisplay)
        {
            Name = name;

            Display(isDisplay);
        }

        public void Dispose()
        {
            Display(false);
        }

        protected void IsDisplayCallback(Action cards, Action cardObjects)
        {
            cards.Invoke();

            if (IsDisplay)
            {
                cardObjects.Invoke();
            }
        }

        public void Display(bool isDisplay)
        {
            IsDisplay = isDisplay;

            if (IsDisplay)
            {
                for (int i = 0; i < Count.Value; i++)
                {
                    SetCardObject(Cards[i]);
                }
            }
            else
            {
                for (int i = 0; i < Count.Value; i++)
                {
                    CardObjects[i].Dispose();
                }

                CardObjects.Clear();
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].Refresh();
            }
        }

        // =========================================================================== Card

        public List<Card> Choose(params Predicate<Card>[] match)
        {
            List<Card> result = Cards;

            for (int i = 0; i < match.Length; i++)
            {
                result = result.FindAll(match[i]);
            }

            return result;
        }

        public virtual void Add(Card card)
        {
            IsDisplayCallback(() =>
            {
                Cards.Add(card);
            }, 
            () =>
            {
                SetCardObject(card);
            });

            Count.Value += 1;

            Refresh();
            Arrange();
        }

        public virtual void Remove(Card card)
        {
            int index = Cards.IndexOf(card);

            if (index == -1)
            {
                return;
            }

            IsDisplayCallback(() =>
            {
                Cards.RemoveAt(index);
            },
            () =>
            {
                CardObjects[index].Dispose();

                CardObjects.RemoveAt(index);
            });

            Count.Value -= 1;

            Refresh();
            Arrange();
        }

        public virtual void Destroy(Card card)
        {
            Remove(card);

            card.Dispose();
        }

        public Card this[int index]
        {
            get
            {
                return new Card();
            }
        }

        public Card this[string key]
        {
            get
            {
                return new Card();
            }
        }

        // =========================================================================== CardObject

        protected virtual void SetCardObject(Card card)
        {
            CardObject cardObject = CardObject.Create(card.InstanceID, this);

            cardObject.Pile = this;

            card.Subscribe(cardObject);

            CardObjects.Add(cardObject);
        }

        // =========================================================================== Transform

        // ================================================== Position

        public void Arrange()
        {
            if (!IsDisplay)
            {
                return;
            }

            for (int i = 0; i < Count.Value; i++)
            {
                Vector3 position = GetPosition(Count.Value, i);

                CardObjects[i].Arrange(position, i);
            }
        }

        protected abstract Vector3 GetPosition(int count, int index);
    }

    // ==================================================================================================== CardOwnedPile

    [Serializable] public class CardOwnedPile
    {

    }

    // ==================================================================================================== CardDeckPile

    [Serializable] public class CardDeckPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Card

        public override void Add(Card card)
        {
            base.Add(card);

            Suffle();
        }

        public override void Remove(Card card)
        {
            base.Remove(card);
        }

        // =========================================================================== Deck

        public void Suffle()
        {
            int index;

            for (int i = 0; i < Count.Value; i++)
            {
                index = UnityEngine.Random.Range(i, Count.Value);

                IsDisplayCallback(() =>
                {
                    (Cards[i], Cards[index]) = (Cards[index], Cards[i]);
                },
                () =>
                {
                    (CardObjects[i], CardObjects[index]) = (CardObjects[index], CardObjects[i]); // 이건 굳이 없어도 될거 같긴 한데 숫자 맞춰주는게 정렬하기 편하니까 해둠

                    Arrange();
                });
            }
        }

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 8;

            return new Vector3(x, y, 0.0f);
        }
    }

    // ==================================================================================================== CardHandPile

    [Serializable] public class CardHandPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 8;

            return new Vector3(x, y, 0.0f);
        }
    }

    // ==================================================================================================== CardDiscardPile

    [Serializable] public class CardDiscardPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 8;

            return new Vector3(x, y, 0.0f);
        }
    }

    // ==================================================================================================== CardExiledPile

    [Serializable] public class CardExiledPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 8;

            return new Vector3(x, y, 0.0f);
        }
    }

    // ==================================================================================================== CardRewardPile

    [Serializable] public class CardRewardPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 8;

            return new Vector3(x, y, 0.0f);
        }
    }
}
