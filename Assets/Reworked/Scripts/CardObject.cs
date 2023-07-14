using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Reworked
{
    // ==================================================================================================== CardObject

    public partial class CardObject : MonoBehaviour, ICardObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("개체 ID")]
        [SerializeField] private string _instanceID;

        // =========================================================================== Component

        // ================================================== Image

        [Header("이미지")]
        [SerializeField] private Image _frameImage;
        [SerializeField] private Image _artworkImage;

        // ================================================== TextMeshPro

        [Header("텍스트")]
        [SerializeField] private TMP_Text _nameTMP;
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _descriptionTMP;

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

        // ==================================================================================================== Method

        // =========================================================================== Event

        //public void Awake()
        //{

        //}

        // =========================================================================== EventSystems

        // ================================================== Pointer

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        // =========================================================================== Instance

        public static CardObject Create(string instanceID)
        {
            return null;
        }

        // =========================================================================== Component

        // ================================================== TextMeshPro

        //public void RefreshName()
        //{
        //    _nameTMP.text = Card.Cache.Data[InstanceID].Name;
        //}
    }
}
