using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WIP
{
    // ==================================================================================================== CardPile

    [Serializable] public abstract class CardPile
    {
        // ==================================================================================================== Field

        // =========================================================================== Pile

        // =========================================================================== Card

        // ================================================== Instance

        [Header("카드 리스트")]
        [SerializeField, JsonProperty("Cards")] private List<Card> _cards = new List<Card>();

        // =========================================================================== CardObject

        // ================================================== Instance

        [Header("카드 오브젝트 리스트")]
        [SerializeField, JsonIgnore] public List<CardObject> _cardObjects = new List<CardObject>();

        [Header("카드 오브젝트 활성화 여부")]
        [SerializeField, JsonIgnore] private bool _isDisplay;

        // =========================================================================== Transform

        // ================================================== Sibling Index

        [Header("카드 그룹 이름")]
        [SerializeField, JsonProperty("Name")] private string _name;

        // ==================================================================================================== Property

        // =========================================================================== Indexer

        public Card this[int index]
        {
            get
            {
                return Cards[index];
            }
        }

        public Card this[string key]
        {
            get
            {
                return Cards.Find((card) =>
                {
                    return card.InstanceID == key;
                });
            }
        }

        // =========================================================================== Pile

        [JsonIgnore] public int Count
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

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public abstract void OnPointerEnter(PointerEventData eventData, CardObject cardObject);

        public abstract void OnPointerExit(PointerEventData eventData, CardObject cardObject);

        // ================================================== Drag

        public abstract void OnBeginDrag(PointerEventData eventData, CardObject cardObject);

        public abstract void OnDrag(PointerEventData eventData, CardObject cardObject);

        public abstract void OnEndDrag(PointerEventData eventData, CardObject cardObject);

        // =========================================================================== Pile

        public void Initialize(string name, bool isDisplay)
        {
            Name = name;

            CardManager.Instance.OnCardArrange += Arrange;

            Display(isDisplay);
        }

        public void Dispose()
        {
            CardManager.Instance.OnCardArrange -= Arrange;

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

        public virtual void Display(bool isDisplay)
        {
            IsDisplay = isDisplay;

            if (IsDisplay)
            {
                for (int i = 0; i < Count; i++)
                {
                    Instantiate(Cards[i]);
                }

                Refresh();
                Arrange(null);
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

        public virtual void Refresh()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].Refresh();
            }
        }

        public virtual void Clear()
        {
            Cards.Clear();

            int count = CardObjects.Count;

            for (int i = 0; i < count; i++)
            {
                CardObjects[i].Dispose();
            }

            CardObjects.Clear();
        }

        // =========================================================================== Card

        public virtual void Add(Card card)
        {
            IsDisplayCallback(() =>
            {
                Cards.Add(card);
            }, 
            () =>
            {
                Instantiate(card);
            });

            Refresh();
            Arrange(null);
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

            Refresh();
            Arrange(null);
        }

        public virtual void Destroy(Card card)
        {
            Remove(card);

            card.Dispose();
        }

        // =========================================================================== CardObject

        protected virtual CardObject Instantiate(Card card)
        {
            CardObject cardObject = CardObject.Create(card.InstanceID, this);

            cardObject.Pile = this;

            card.Subscribe(cardObject);

            CardObjects.Add(cardObject);

            return cardObject;
        }

        // =========================================================================== Transform

        // ================================================== Position

        public virtual void Arrange(IEventParameter parameter)
        {
            if (!IsDisplay)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                Vector3 position = GetPosition(Count, i); // 추후 Pos_Rot_Scale로 교체 필요함

                CardObjects[i].Arrange(position, i);
            }
        }

        protected abstract Vector3 GetPosition(int count, int index);
    }

    // ==================================================================================================== CardOwnedPile

    [Serializable] public class CardOwnedPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

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

    // ==================================================================================================== CardDeckPile

    [Serializable] public class CardDeckPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

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

            for (int i = 0; i < Count; i++)
            {
                index = UnityEngine.Random.Range(i, Count);

                IsDisplayCallback(() =>
                {
                    (Cards[i], Cards[index]) = (Cards[index], Cards[i]);
                },
                () =>
                {
                    (CardObjects[i], CardObjects[index]) = (CardObjects[index], CardObjects[i]); // 이건 굳이 없어도 될거 같긴 한데 숫자 맞춰주는게 정렬하기 편하니까 해둠

                    Arrange(null);
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

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        // =========================================================================== CardObject

        protected override CardObject Instantiate(Card card)
        {
            CardObject cardObject = base.Instantiate(card);

            CardComponents components = cardObject.GetComponent<CardComponents>();

            Button button = cardObject.gameObject.AddComponent<Button>();

            button.targetGraphic = components.FrameImage;
            button.onClick.AddListener(() => Buy(CardObjects.IndexOf(cardObject)));

            return cardObject;
        }

        public void Buy(int index)
        {
            Debug.Log($"까꿍 {index}");
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

    // ==================================================================================================== CardDiscardPile

    [Serializable] public class CardDiscardPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

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

    // ==================================================================================================== CardExiledPile

    [Serializable] public class CardExiledPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

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

    // ==================================================================================================== CardShopPile

    [Serializable] public class CardShopPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        // =========================================================================== CardObject

        protected override CardObject Instantiate(Card card)
        {
            CardObject cardObject = base.Instantiate(card);

            Button button = cardObject.gameObject.AddComponent<Button>();

            button.onClick.AddListener(() =>
            {
                Buy(Cards.IndexOf(card));
            });

            return cardObject;
        }

        public void Buy(int index)
        {
            Debug.Log($"{index} : 발동");
        }

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
        //    float x = (index * 2 - count + 1) * 50.0f + Screen.width / 2;
        //    float y = Screen.height / 8;

            return new Vector3(0, 0, 0.0f);
        }
    }

    // ==================================================================================================== CardRewardPile

    [Serializable] public class CardRewardPile : CardPile
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public override void OnPointerEnter(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnPointerExit(PointerEventData eventData, CardObject cardObject)
        {

        }

        // ================================================== Drag

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        // =========================================================================== CardObject

        protected override CardObject Instantiate(Card card)
        {
            CardObject cardObject = base.Instantiate(card);

            cardObject.gameObject.AddComponent<Button>();

            return cardObject;
        }

        // =========================================================================== Transform

        // ================================================== Position

        protected override Vector3 GetPosition(int count, int index)
        {
            // 위치 계산식

            return new Vector3();
        }
    }
}
