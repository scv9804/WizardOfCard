using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    public class EventController : MonoBehaviour
    {
        // ==================================================================================================== Method

        // =========================================================================== GameManager

        public void GameManager_Quit()
        {
            GameManager.Instance.Quit();
        }

        public void GameManager_BattleEnd()
        {
            GameManager.Instance.BattleEnd();
        }

        // =========================================================================== CardManager

        // ================================================== Card

        public void CardManager_OnDrawButtonPressed()
        {
            StartCoroutine(CardManager.Instance.Draw(1, (card) =>
            {
                var cardObject = CardManager.Instance.Visualize(card);

                CardManager.Instance.CardObjects.Add(CardManager.HAND, cardObject);
                cardObject.SetParent(CardManager.HAND);
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

        public void CardManager_OnTurnEnd()
        {
            CardManager.Instance.OnTurnEnd();
        }

        public void CardManager_OnCardAbilityCasted(string name)
        {
            CardManager.Instance.OnCardAbilityCasted(name);
        }

        public void CardManager_OnActionButtonPressed()
        {
            CardManager.Instance.OnActionButtonPressed();
        }

        public void CardManager_OnActionButtonCanceled()
        {
            CardManager.Instance.OnActionButtonCanceled();
        }

        // =========================================================================== EntityManager

        // ================================================== GameEvent

        public void EntityManager_OnTurnStart(GameObject character)
        {
            EntityManager.Instance.OnTurnStart(character);
        }

        public void EntityManager_OnEntityDie(GameObject character)
        {
            EntityManager.Instance.OnEntityDie(character);
        }

        public void EntityManager_OnBattleEnd()
        {
            EntityManager.Instance.OnBattleEnd();
        }
    }
}