using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Delegates;
using BETA.Enums;
using BETA.Interfaces;

using Sirenix.OdinInspector;

using System;

using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Toggle;

namespace BETA
{
    // ==================================================================================================== CardObject

    public class CardObject : SerializedMonoBehaviour, ICardObject, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        private string _instanceID;

        // =========================================================================== CardObject

        private Vector3 _originPosition;

        private CardState _state = CardState.NONE;

        public Dictionary<string, CardEventSystems> Commands = new Dictionary<string, CardEventSystems>();

        // =========================================================================== Component

        // ================================================== Image

        [FoldoutGroup("이미지")]
        public Image FrameImage;

        [FoldoutGroup("이미지")]
        public Image ArtworkImage;

        // ================================================== Text

        [FoldoutGroup("텍스트")]
        public TMP_Text NameTMP;

        [FoldoutGroup("텍스트")]
        public TMP_Text CostTMP;

        [FoldoutGroup("텍스트")]
        public TMP_Text DescriptionTMP;

        // =========================================================================== Data

        public ICard Unit;

        // ==================================================================================================== Property

        // =========================================================================== Instance

        //public string InstanceID
        //{
        //    get
        //    {
        //        return _instanceID;
        //    }

        //    set
        //    {
        //        _instanceID = value;
        //    }
        //}

        public string InstanceID
        {
            get
            {
                return Unit.InstanceID;
            }
        }

        public int SerialID
        {
            get
            {
                return Unit.SerialID;
            }
        }

        // =========================================================================== CardObject

        public Vector3 OriginPosition
        {
            get
            {
                return _originPosition;
            }

            set
            {
                _originPosition = value;
            }
        }

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

        // =========================================================================== General

        public string Name
        {
            get
            {
                return Unit.Name;
            }
        }

        public Enums.CardType Type
        {
            get
            {
                return Unit.Type;
            }
        }

        public CardKeyword Keyword
        {
            get
            {
                return Unit.Keyword;
            }
        }

        public int Level
        {
            get
            {
                return Unit.Level;
            }
        }

        public int Cost
        {
            get
            {
                return Unit.Cost;
            }
        }

        public string Description
        {
            get
            {
                return Unit.Description;
            }
        }

        // =========================================================================== Component

        public Sprite FrameSprite
        {
            get
            {
                return Card.DataSet.FrameSprite[Type][Level];
            }
        }

        public Sprite ArtworkSprite
        {
            get
            {
                return Card.DataSet.ArtworkSprite[SerialID][Level];
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            //var command = Commands["ON_POINTER_ENTER"];

            //Delegates.Add("ON_POINTER_ENTER", Hand.OnPointerEnter);
        }

        private void Update()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            //DataManager.Instance.Unsubscribe(this);
        }

        // =========================================================================== EventSystems

        public void OnPointerEnter(PointerEventData eventData)
        {
            Commands["ON_POINTER_ENTER"]?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Commands["ON_POINTER_EXIT"]?.Invoke(this, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Commands["ON_POINTER_CLICK"]?.Invoke(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Commands["ON_BEGIN_DRAG"]?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Commands["ON_DRAG"]?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Commands["ON_END_DRAG"]?.Invoke(this, eventData);
        }

        // =========================================================================== Instance

        public void Refresh()
        {
            if (Unit == null)
            {
                return;
            }

            FrameImage.sprite = FrameSprite;
            ArtworkImage.sprite = ArtworkSprite;

            NameTMP.text = Name;
            CostTMP.text = Cost.ToString();
            DescriptionTMP.text = Description;
        }

        public void Destroy()
        {
            Unit = null;

            Destroy(gameObject);
        }

        // =========================================================================== UI

        public void SetParent(string name)
        {
            var parent = CardManager.Instance.CardObjectContainer[name]?.transform;

            transform.SetParent(parent);
        }

        //

        public static class Hand
        {
            //

            //

            public static void OnPointerEnter(CardObject cardObject, PointerEventData eventData)
            {
                cardObject.State = CardState.ON_POINTER_OVER;

                //CardManager.Instance.Current = cardObject;

                cardObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                "등록".Print();
            }
        }
    }
}
