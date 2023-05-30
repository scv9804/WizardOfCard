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

        public int Count
        {
            get
            {
                return Cards.Count;
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

        [JsonIgnore] public List<CardObject> CardObjects
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
            CardManager.Instance.OnArrange += Arrange;

            Name = name;

            IsDisplay = isDisplay;
        }

        public void Dispose()
        {
            CardManager.Instance.OnArrange -= Arrange;
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
                for (int i = 0; i < Count; i++)
                {
                    SetCardObject(Cards[i].InstanceID);
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    CardObjects[i].Dispose();
                }

                CardObjects.Clear();
            }
        }

        // =========================================================================== Card

        public Card GetCard(string instanceID)
        {
            return Cards.Find((card) =>
            {
                return card.InstanceID == instanceID;
            });
        }

        public virtual void Add(Card card)
        {
            IsDisplayCallback(() =>
            {
                Cards.Add(card);
            }, 
            () =>
            {
                SetCardObject(card.InstanceID);
            });
        }

        public virtual void Remove(Card card)
        {
            int index = Cards.IndexOf(card);

            IsDisplayCallback(() =>
            {
                Cards.RemoveAt(index);
            },
            () =>
            {
                CardObjects[index].Dispose();

                CardObjects.RemoveAt(index);

                Arrange();
            });
        }

        // =========================================================================== CardObject

        protected virtual void SetCardObject(string instanceID)
        {
            CardObject cardObject = CardObject.Create(instanceID, this);

            CardObjects.Add(cardObject);

            Arrange();
        }

        // =========================================================================== Transform

        // ================================================== Sibling Index

        public string GetGroupName()
        {
            return Name;
        }

        // ================================================== Position

        public void Arrange()
        {
            for (int i = 0; i < CardObjects.Count; i++)
            {
                Vector3 position = GetPosition(CardObjects.Count, i);

                CardObjects[i].Arrange(position, i);
            }
        }

        protected abstract Vector3 GetPosition(int count, int index);
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

            for (int i = 0; i < Cards.Count; i++)
            {
                index = UnityEngine.Random.Range(i, Cards.Count);

                IsDisplayCallback(() =>
                {
                    (Cards[i], Cards[index]) = (Cards[index], Cards[i]);
                },
                () =>
                {
                    (CardObjects[i], CardObjects[index]) = (CardObjects[index], CardObjects[i]); // 이건 굳이 없어도 될거같기도
                });
            }
        }

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 4;

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
            float y = Screen.height / 4;

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
            float y = Screen.height / 4;

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
            float y = Screen.height / 4;

            return new Vector3(x, y, 0.0f);
        }
    }

    // ==================================================================================================== CardExiledPile

    [Serializable] public class CardrewardPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
            float y = Screen.height / 4;

            return new Vector3(x, y, 0.0f);
        }
    }
}
