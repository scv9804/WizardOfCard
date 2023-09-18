using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;

using UnityEngine.EventSystems;

namespace BETA
{
    // ==================================================================================================== CardEventSystems

    [CreateAssetMenu(menuName = "BETA/Card/EventSystems")]
    public sealed class CardEventSystems : ScriptableEventSystems<CardObject>
    {
        // ==================================================================================================== Method

        // =========================================================================== EventSystems

        // ================================================== Own

        // ================================================== Deck

        // ================================================== Hand

        public void Hand_OnPointerEnter(CardObject cardObject, PointerEventData eventData)
        {
            cardObject.State = CardState.ON_POINTER_OVER;

            cardObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            "등록".Print();
        }

        public void Hand_OnPointerExit(CardObject cardObject, PointerEventData eventData)
        {
            cardObject.State = CardState.NONE;

            cardObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            "해제".Print();
        }

        public void Hand_OnPointerClick(CardObject cardObject, PointerEventData eventData)
        {

        }

        public void Hand_OnBeginDrag(CardObject cardObject, PointerEventData eventData)
        {
            cardObject.State = CardState.ON_DRAG;

            cardObject.OriginPosition = cardObject.transform.position;
        }

        public void Hand_OnDrag(CardObject cardObject, PointerEventData eventData)
        {
            cardObject.transform.position = eventData.position;
        }

        public void Hand_OnEndDrag(CardObject cardObject, PointerEventData eventData)
        {
            cardObject.State = CardState.NONE;

            if (cardObject.transform.position.y >= Screen.height * 0.5f)
            {
                CardManager.Instance.Play(cardObject);
            }
            else
            {
                cardObject.transform.position = cardObject.OriginPosition;
            }
        }

        // ================================================== Discard

        // ================================================== Exclude

        // ================================================== Shop
    }
}
