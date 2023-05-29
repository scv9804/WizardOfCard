using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.EventSystems;

namespace WIP
{
    // ==================================================================================================== CardObject

    public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("인스턴스 ID")]
        [SerializeField] private string _instanceID;

        // =========================================================================== Card

        private Card _card;

        // =========================================================================== Transform

        // ================================================== Position

        [Header("원래 위치")]
        [SerializeField] private Vector3 _originPosition;

        // ================================================== Sibling Index

        [Header("원래 하이어라키 순서")]
        [SerializeField] private int _originSiblingIndex;

        // =========================================================================== State

        [Header("상태")]
        [SerializeField] private CardState _state = CardState.None;

        [Header("사용 가능 여부")]
        [SerializeField] private bool _isUsable = true;

        // =========================================================================== Module

        private ICardLocationModule _locationModule;

        private ICardObjectHandler _handler;

        // =========================================================================== Component

        [Header("컴포넌트")]
        [SerializeField] private CardComponents _components;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        public string InstanceID
        {
            get
            {
                return _instanceID;
            }

            set
            {
                _instanceID = value;
            }
        }

        // =========================================================================== Module

        public Card Card
        {
            get
            {
                return _card;
            }

            set
            {
                _card = value;
            }
        }

        // =========================================================================== Transform

        // ================================================== Position

        public Vector3 OriginPosition
        {
            get
            {
                return _originPosition;
            }

            private set
            {
                _originPosition = value;
            }
        }

        // ================================================== Sibling Index

        public int OriginSiblingIndex
        {
            get
            {
                return _originSiblingIndex;
            }

            private set
            {
                _originSiblingIndex = value;
            }
        }

        // =========================================================================== State

        public CardState State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        public bool IsUsable
        {
            get
            {
                return _isUsable;
            }

            set
            {
                _isUsable = value;
            }
        }

        // =========================================================================== Module

        public ICardLocationModule LocationModule
        {
            get
            {
                return _locationModule;
            }

            set
            {
                _locationModule = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== EventSystem

        // ================================================== Pointer

        public void OnPointerEnter(PointerEventData eventData)
        {
            CardManager.Instance.OnPointerEnter(eventData, this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CardManager.Instance.OnPointerExit(eventData, this);
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            CardManager.Instance.OnBeginDrag(eventData, this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            CardManager.Instance.OnDrag(eventData, this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CardManager.Instance.OnEndDrag(eventData, this);
        }

        // =========================================================================== Instance

        public static CardObject Create(Card card, ICardLocationModule locationModule)
        {
            GameObject gameObject = Instantiate(CardManager.Instance.CardPrefab);

            gameObject.name = card.InstanceID;

            var cardObject = gameObject.GetComponent<CardObject>();

            cardObject.InstanceID = card.InstanceID;
            cardObject.Card = card;
            cardObject.LocationModule = locationModule;

            //
            cardObject.Initialize();

            card.OnRefresh += new EventHandler<CardRefreshEventArgs>(cardObject.Refresh);

            return cardObject;
        }

        public void Initialize()
        {
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();

            Destroy(gameObject);
        }

        private void Subscribe()
        {
            CardManager.Instance.OnEnlarge += new Action(Enlarge);
            CardManager.Instance.OnArrange += new Action(Arrange);
            CardManager.Instance.OnEmphasize += new Action(Emphasize);

            Card.OnRefresh += new EventHandler<CardRefreshEventArgs>(Refresh);

            CardManager.Instance.EnlargeAll();
            CardManager.Instance.ArrangeAll();
            CardManager.Instance.EmphasizeAll();

            CardManager.Instance.RefreshAll();
        }

        private void Unsubscribe()
        {
            CardManager.Instance.OnEnlarge -= new Action(Enlarge);
            CardManager.Instance.OnArrange -= new Action(Arrange);
            CardManager.Instance.OnEmphasize -= new Action(Emphasize);

            Card.OnRefresh -= new EventHandler<CardRefreshEventArgs>(Refresh);
        }

        // =========================================================================== Transform

        // ================================================== Scale

        private void Enlarge()
        {
            float size = LocationModule.GetSize(State);

            transform.localScale = new Vector3(size, size, 1.0f);
        }

        // ================================================== Position

        private void Arrange()
        {
            var element = LocationModule.GetElement(Card);

            Vector3 position = LocationModule.GetPosition(element.Count, element.Index);

            SetOriginPosition(position);
            SetOriginSiblingIndex(element.Index);

            if (State < CardState.IsDrag)
            {
                Move(OriginPosition);
            }
        }

        private void SetOriginPosition(Vector3 position)
        {
            OriginPosition = position;
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }

        // ================================================== Sibling Index

        private void Emphasize()
        {
            if (State > CardState.None)
            {
                transform.SetParent(GameObject.Find(Card.SELECTED_GROUP_NAME).transform);
            }
            else
            {
                transform.SetParent(GameObject.Find(LocationModule.GroupName).transform);

                transform.SetSiblingIndex(OriginSiblingIndex);
            }
        }

        private void SetOriginSiblingIndex(int index)
        {
            OriginSiblingIndex = index;
        }

        // =========================================================================== Component

        private void Refresh(object sender, CardRefreshEventArgs e)
        {
            _components.FrameImage.sprite = e.FrameSprite;
            _components.ArtworkImage.sprite = e.ArtworkSprite;

            _components.NameTMP.text = e.Name;
            _components.CostTMP.text = e.Cost.ToString();
            _components.DescriptionTMP.text = e.Description;
        }
    }

    // ==================================================================================================== CardObjectHandler

    //public class CardObjectHandler : ICardObjectHandler
    //{

    //}

    // ==================================================================================================== CardRefreshEventArgs

    [Serializable] public class CardRefreshEventArgs : EventArgs
    {
        // ==================================================================================================== Field

        // =========================================================================== Argument

        public new static readonly CardRefreshEventArgs Empty = default;

        // =========================================================================== Status

        public string Name;

        public int Cost;

        public string Description;

        // =========================================================================== Asset

        public Sprite FrameSprite;
        public Sprite ArtworkSprite;
    }

    // ==================================================================================================== ICardObjectHandler

    public interface ICardObjectHandler
    {
        // ==================================================================================================== Property

        // =========================================================================== Location

        public List<Card> Cards
        {
            get;
        }

        // =========================================================================== Transform

        // ================================================== Sibling Index

        public string GroupName
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Transform

        // ================================================== Scale

        public float GetSize(CardState state);

        // ================================================== Position

        public (int Count, int Index) GetElement(Card card);

        public Vector3 GetPosition(int count, int index);
    }
}