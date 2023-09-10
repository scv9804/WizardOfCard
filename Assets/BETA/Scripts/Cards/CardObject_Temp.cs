using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

using Sirenix.OdinInspector;

using TacticsToolkit;

using UnityEngine.EventSystems;

namespace BETA
{
    // ==================================================================================================== CardObject_Temp

    public sealed class CardObject_Temp : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Component

        public CardObjectComponents Components;

        // =========================================================================== ?????????

        public SpriteTable FrameSprites;
        public SpriteTable ArtworkSprites;

        // =========================================================================== ????

        private Vector3 _originalPosition;

        // =========================================================================== ??????

        public GameEventString castAbility;

        public Ability Ability;

        // ==================================================================================================== Method

        // =========================================================================== Event

        //public void OnMouseEnter()
        //{
        //    "¹ÝÂ¦".Print();
        //}

        public void Start()
        {
            Components.FrameImage.sprite = FrameSprites.Sprite[2];
            Components.ArtworkImage.sprite = ArtworkSprites.Sprite[2];

            Components.NameTMP.text = Ability.Name;
            Components.CostTMP.text = Ability.cost.ToString();
            Components.DescriptionTMP.text = Ability.Desc;
        }

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
            //if (eventData.dragging)
            //{
            //    return;
            //}

            //"µþ".Print();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            "±ï".Print();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            "Å¬¸¯".Print();
        }

        // ================================================== Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.position.y >= Screen.height * 0.5f)
            {
                castAbility.Raise(Ability.Name);

                Destroy(gameObject);
            }
            else
            {
                transform.position = _originalPosition;
            }
        }
    }
}
