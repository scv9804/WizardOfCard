using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TacticsToolkit;

namespace BETA
{
    // ==================================================================================================== CardManagerEventPort

    public sealed class CardManagerEventPort : SerializedMonoBehaviour
    {
        // ==================================================================================================== Method

        // =========================================================================== TEMP

        public void DrawOneCard()
        {
            StartCoroutine(CardManager.Instance.Draw(1, (card) =>
            {
                var cardObject = CardManager.Instance.Visualize(card);

                CardManager.Instance.CardObjects.Add(CardManager.HAND, cardObject);
            }));
        }

        // =========================================================================== GameEvent

        public void OnBattleEnd()
        {
            CardManager.Instance.OnBattleEnd();
        }

        public void OnActionButtonPressed()
        {
            CardManager.Instance.OnActionButtonPressed();
        }

        public void OnCancelActionButton(string name)
        {
            CardManager.Instance.OnActionButtonCanceled();
        }
    } 
}
