using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    public class EventController : MonoBehaviour
    {
        // ==================================================================================================== Method

        // =========================================================================== CardManager

        // ================================================== Card

        public void CardManager_OnDrawButtonPressed()
        {
            StartCoroutine(CardManager.Instance.Draw(1, (card) =>
            {
                var cardObject = CardManager.Instance.Visualize(card);

                CardManager.Instance.CardObjects.Add(CardManager.HAND, cardObject);
            }));
        }

        // ================================================== GameEvent

        public void CardManager_OnBattleEnd()
        {
            CardManager.Instance.OnBattleEnd();
        }

        public void CardManager_OnTurnStart(GameObject character)
        {
            CardManager.Instance.OnTurnStart(character);
        }

        public void CardManager_OnActionButtonPressed()
        {
            CardManager.Instance.OnActionButtonPressed();
        }

        public void CardManager_OnActionButtonCanceled(string name)
        {
            CardManager.Instance.OnActionButtonCanceled();
        }
    }
}