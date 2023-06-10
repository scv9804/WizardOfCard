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
        [SerializeField] private bool _isUsable = true; // 이거는 위치 따라서 설정 ㄱㄴ할듯

        // =========================================================================== Pile

        [NonSerialized] private CardPile _pile;

        private EventObserver OnCardPointerEnter;

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

        // =========================================================================== Pile

        public CardPile Pile
        {
            get
            {
                return _pile;
            }

            set
            {
                _pile = value;
            }
        }

        // =========================================================================== Card

        public Card Card
        {
            get
            {
                return Pile[InstanceID];
            }
        }

        // =========================================================================== Component

        //public CardComponents Compo

        // ==================================================================================================== Method

        // =========================================================================== Event

        public void Update()
        {
            Enlarge();
            Emphasize();
        }

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

        public static CardObject Create(string instanceID, CardPile pile)
        {
            GameObject gameObject = Instantiate(CardManager.Instance.CardPrefab);

            var cardObject = gameObject.GetComponent<CardObject>();

            cardObject.Initialize(instanceID, pile);

            return cardObject;
        }

        public void Initialize(string instanceID, CardPile pile)
        {
            name = instanceID;

            InstanceID = instanceID;
            Pile = pile;
        }

        public void Dispose()
        {
            Destroy(gameObject); // 풀 매니저 만들면 수정여지 있음
        }

        // =========================================================================== Transform

        // ================================================== Scale

        public void Enlarge()
        {
            float size = State == CardState.IsPointerOver ? CardManager.Instance.Settings.EnlargedCardSize : CardManager.Instance.Settings.DefaultCardSize;

            transform.localScale = new Vector3(size, size, 1.0f);
        }

        // ================================================== Position

        public void Arrange(Vector3 position, int index)
        {
            SetOriginPosition(position);
            SetOriginSiblingIndex(index);

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

        public void Emphasize()
        {
            if (State > CardState.None)
            {
                transform.SetParent(GameObject.Find(Card.SELECTED_GROUP_NAME).transform);
            }
            else
            {
                transform.SetParent(GameObject.Find(Pile.Name).transform);

                transform.SetSiblingIndex(OriginSiblingIndex);
            }
        }

        private void SetOriginSiblingIndex(int index)
        {
            OriginSiblingIndex = index;
        }

        // =========================================================================== Component

        // ================================================== Image

        public void RefreshFrameImage(IEventParameter parameter)
        {
            parameter.Casting<IData<Sprite>>((frameSprite) =>
            {
                _components.FrameImage.sprite = frameSprite.Value;
            });
        }

        public void RefreshArtworkImage(IEventParameter parameter)
        {
            parameter.Casting<IData<Sprite>>((artworkSprite) =>
            {
                _components.ArtworkImage.sprite = artworkSprite.Value;
            });
        }

        // ================================================== Text

        public void RefreshNameText(IEventParameter parameter)
        {
            parameter.Casting<IData<string>>((name) =>
            {
                _components.NameTMP.text = name.Value;
            });
        }

        public void RefreshCostText(IEventParameter parameter)
        {
            parameter.Casting<IData<int>>((cost) =>
            {
                _components.CostTMP.text = cost.Value.ToString();
            });
        }

        public void RefreshDescriptionText(IEventParameter parameter)
        {
            parameter.Casting<IData<string>>((description) =>
            {
                _components.DescriptionTMP.text = description.Value;
            });
        }
    }

    // ==================================================================================================== CardRefreshEventArgs

    [Serializable] public class CardComponentData
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        public string _name;

        public int _cost;

        public string _description;

        // =========================================================================== Asset

        public Sprite _frameSprite;
        public Sprite _artworkSprite;

        // ==================================================================================================== Field

        // =========================================================================== Status

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                _cost = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        // =========================================================================== Asset

        public Sprite FrameSprite
        {
            get
            {
                return _frameSprite;
            }

            set
            {
                _frameSprite = value;
            }
        }

        public Sprite ArtworkSprite
        {
            get
            {
                return _artworkSprite;
            }

            set
            {
                _artworkSprite = value;
            }
        }
    }
}