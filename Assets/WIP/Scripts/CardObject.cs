using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Text;

using TMPro;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WIP
{
    // ==================================================================================================== CardObject

    public class CardObject : MonoBehaviour, IUnitObject, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Identifier

        [Header("인스턴스 ID")]
        [SerializeField] private string _instanceID;

        // =========================================================================== Transform

        // ================================================== Scale

        public const float DEFAULT_CARD_SIZE = 0.25f;
        public const float ENLARGEMENT_CARD_SIZE = 0.4f;

        // ================================================== Position

        private Vector3 _originalPosition;

        // ================================================== Sibling Index

        private int _originalSiblingIndex;

        // =========================================================================== State

        [Header("상태")]
        [SerializeField] private CardState _state = CardState.Playable;

        // =========================================================================== Component

        // ================================================== Image

        [Header("이미지 컴포넌트")]
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _artworkImage;

        // ================================================== Text

        [Header("텍스트 컴포넌트")]
        [SerializeField] private TMP_Text _nameTMP;
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _descriptionTMP;

        // ==================================================================================================== Property

        // =========================================================================== Data

        // ================================================== Identifier

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

        public Vector3 OriginalPosition
        {
            get
            {
                return _originalPosition;
            }

            private set
            {
                _originalPosition = value;
            }
        }

        // ================================================== Sibling Index

        public int OriginalSiblingIndex
        {
            get
            {
                return _originalSiblingIndex;
            }

            private set
            {
                _originalSiblingIndex = value;
            }
        }

        // =========================================================================== State

        public bool IsSelected
        {
            get
            {
                return CardManager.Instance.Selected == this;
            }
        }

        public bool IsPlayable
        {
            get
            {
                return (_state & CardState.Playable) != 0;
            }
        }

        public bool OnPointerOver
        {
            get
            {
                return (_state & CardState.OnPointerOver) != 0;
            }
        }

        public bool OnDraging
        {
            get
            {
                return (_state & CardState.OnDraging) != 0;
            }
        }

        public bool OnUsing
        {
            get
            {
                return (_state & CardState.OnUsing) != 0;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        // ================================================== Life Cycle

        private void Update()
        {
            Enlarge();
            Emphasize();
        }

        // ================================================== Pointer

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPlayableCallBack(() =>
            {
                CardManager.Instance.OnPointerEnter(eventData, this);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPlayableCallBack(() =>
            {
                CardManager.Instance.OnPointerExit(eventData, this);
            });
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsPlayableCallBack(() =>
            {
                CardManager.Instance.OnBeginDrag(eventData, this);
            });
        }

        public void OnDrag(PointerEventData eventData)
        {
            IsPlayableCallBack(() =>
            {
                CardManager.Instance.OnDrag(eventData, this);
            });
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsPlayableCallBack(() =>
            {
                CardManager.Instance.OnEndDrag(eventData, this);
            });
        }

        // =========================================================================== Transform

        // ================================================== Scale

        private void Enlarge()
        {
            float size = OnPointerOver && !OnDraging && IsSelected ? ENLARGEMENT_CARD_SIZE : DEFAULT_CARD_SIZE;

            transform.localScale = new Vector3(size, size, 1.0f);
        }

        // ================================================== Position

        public void Arrange(int count, int index)
        {
            IsPlayableCallBack(() =>
            {
                OriginalSiblingIndex = index;

                float x = (index * 2 - count) * CardManager.CARD_SPACING_MARGIN / 2 + Screen.width / 2;
                float y = Screen.height / 4;

                OriginalPosition = new Vector3(x, y, 0.0f);

                if (!OnDraging)
                {
                    MoveTo(OriginalPosition);
                }
            });
        }

        public void MoveTo(Vector3 position)
        {
            // TODO: Convert DoTween Sequence
            transform.position = position;
        }

        // ================================================== Sibling Index

        public void Emphasize()
        {
            IsPlayableCallBack(() =>
            {
                bool selected = OnPointerOver || OnDraging;

                string parentName = selected ? CardManager.CARD_SELECTED_NAME : CardManager.CARD_GROUP_NAME;

                transform.SetParent(GameObject.Find(parentName).transform);

                if (!selected)
                {
                    transform.SetSiblingIndex(OriginalSiblingIndex);
                }
            });
        }

        // =========================================================================== State

        public void SetState(CardState state, bool isActive)
        {
            if (isActive)
            {
                _state |= state;
            }
            else
            {
                _state &= ~state;
            }
        }

        private void IsPlayableCallBack(Action callback)
        {
            if (IsPlayable)
            {
                callback();
            }
        }

        // =========================================================================== Component

        public void Refresh(Card card)
        {
            _frameImage.sprite = card.FrameSprite;
            _artworkImage.sprite = card.ArtworkSprite;

            _nameTMP.text = card.Name;
            _costTMP.text = card.Cost.ToString();
            _descriptionTMP.text = card.Description;
        }

        // =========================================================================== Instance

        public void OnCreate()
        {
            CardManager.Instance.Subscribers[InstanceID] += Receive;

            CardManager.Instance.AllHandCards(CardManager.Instance.Refresh);
            CardManager.Instance.AllHandCards(CardManager.Instance.Arrange);
        }

        public void OnRemove()
        {
            CardManager.Instance.Subscribers[InstanceID] -= Receive;

            CardManager.Instance.AllHandCards(CardManager.Instance.Refresh);
            CardManager.Instance.AllHandCards(CardManager.Instance.Arrange);
        }

        // =========================================================================== Command

        private void Receive(ICommand<Card, CardObject> command)
        {
            command.View = this;

            command.Invoke();
        }
    }

    // ==================================================================================================== Card

    [Serializable] public class Card : IUnit
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Model

        [Header("현재 데이터")]
        [SerializeField, JsonProperty("CurrentData")] private CardCurrentData _currentData = new CardCurrentData();

        [Header("원본 데이터")]
        [SerializeField, JsonIgnore] private CardOriginalData _originalData;

        [Header("구현 데이터")]
        [SerializeField, JsonIgnore] private CardHandlerData _handlerData;

        // ================================================== Upgraded

        public const int MAX_UPGRADE_LEVEL = 2;

        // =========================================================================== Asset

        // ================================================== Model

        [Header("에셋 데이터")]
        [SerializeField, JsonIgnore] private CardAssetData _assetData;

        // =========================================================================== StringBuilder

        private StringBuilder _stringBuilder = new StringBuilder();

        // ==================================================================================================== Property

        // =========================================================================== Data

        // ================================================== Identifier

        [JsonIgnore] public string InstanceID
        {
            get
            {
                return _currentData.InstanceID;
            }

            set
            {
                _currentData.InstanceID = value;
            }
        }

        [JsonIgnore] public int SerialID
        {
            get
            {
                return _currentData.SerialID;
            }

            set
            {
                _currentData.SerialID = value;
            }
        }

        // ================================================== Base

        [JsonIgnore] public string Name
        {
            get
            {
                return GetName(_originalData.Name);
            }
        }

        [JsonIgnore] public int Cost
        {
            get
            {
                return Math.Max(_originalData.Cost[Upgraded] + _currentData.Cost, 0);
            }
        }

        [JsonIgnore] public bool IsExile
        {
            get
            {
                return _originalData.IsExile[Upgraded];
            }
        }

        [JsonIgnore] public string Description
        {
            get
            {
                return GetDescription(_originalData.Description[Upgraded]);
            }
        }

        // ================================================== Upgrade

        [JsonIgnore] public int Upgraded
        {
            get
            {
                return _currentData.Upgraded;
            }

            private set
            {
                _currentData.Upgraded = value;
            }
        }

        // =========================================================================== Asset

        // ================================================== Component

        [JsonIgnore] public Sprite FrameSprite
        {
            get
            {
                return _assetData.FrameSprite[Upgraded];
            }
        }

        [JsonIgnore] public Sprite ArtworkSprite
        {
            get
            {
                return _assetData.ArtworkSprite[Upgraded];
            }
        }

        // ================================================== Effect

        [JsonIgnore] public Sprite AttackEffectSprite
        {
            get
            {
                return _assetData.AttackEffectSprite;
            }
        }

        [JsonIgnore] public Sprite HitEffectSprite
        {
            get
            {
                return _assetData.HitEffectSprite;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Instance

        public void OnCreate()
        {
            // Initializing

            // TODO: Take Apart OnLoaded Method
            LoadResources();

            CardManager.Instance.Subscribers.Add(InstanceID, Receive);
        }

        public void OnRemove()
        {
            CardManager.Instance.Subscribers.Remove(InstanceID);
        }

        // =========================================================================== Command

        private void Receive(ICommand<Card, CardObject> command)
        {
            command.Controller = this;
        }

        // =========================================================================== Data

        // ================================================== Model

        private void LoadResources()
        {
            CardDatabase database = CardManager.Instance.CardDatabase;

            _originalData = database.Originals[SerialID];

            _assetData = database.Assets[SerialID];

            _handlerData = database.Handlers[SerialID];
        }

        // ================================================== Base

        private string GetName(string name)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append($"{name} I");

            for (int i = 0; i < Upgraded; i++)
            {
                _stringBuilder.Append("I");
            }

            return _stringBuilder.ToString();
        }

        private string GetDescription(string description)
        {
            _stringBuilder.Clear();

            if (IsExile)
            {
                _stringBuilder.Append("망각\n");
            }

            // TODO: Card Handler Description Assemble
            _stringBuilder.Append(description);

            _stringBuilder.Replace("망각", "<color=#ff88ff>망각</color>");

            return _stringBuilder.ToString();
        }

        // ================================================== Upgraded

        public void Upgrade()
        {
            Upgraded = Math.Min(Upgraded + 1, MAX_UPGRADE_LEVEL);

            CardManager.Instance.Refresh(InstanceID);
        }
    }

    // ==================================================================================================== CardCurrentData

    [Serializable] public class CardCurrentData
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("인스턴스 ID")]
        public string InstanceID;

        [Header("시리얼 ID")]
        public int SerialID;

        // =========================================================================== Base

        [Header("비용")]
        public int Cost;

        // =========================================================================== Upgrade

        [Header("강화 횟수")]
        [Range(0, Card.MAX_UPGRADE_LEVEL)] public int Upgraded = 0;

        // =========================================================================== BuffHandler

        // TODO: Card Buff Handler
    }

    // ==================================================================================================== CardState

    [Flags] public enum CardState
    {
        None            = 0,

        Playable        = 1 << 0,

        OnPointerOver   = 1 << 1,

        OnDraging       = 1 << 2,

        OnUsing         = 1 << 3
    }
}
