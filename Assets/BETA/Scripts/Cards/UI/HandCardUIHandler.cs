using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

namespace BETA.UI
{
    // ==================================================================================================== HandCardUIHandler

    public class HandCardUIHandler : CardUIHandler
    {
        //

        //

        public override void Refresh()
        {
            //CardManager.Instance.CardObjects["DECK"].Find();

            //if (CardManager.Instance.IsHandCardEmpty())
            //{
            //    return;
            //}

            var hand = CardManager.Instance.CardObjects[name];

            if (hand.Count == 0)
            {
                return;
            }

            var center = hand.Count * -0.5f + 0.5f;
            var spacing = 125.0f - hand.Count * 5.0f;

            for (var i = hand.Count - 1; i > -1; i--)
            {
                var cardObject = hand[i];

                if (cardObject.State >= CardState.ON_POINTER_OVER)
                {
                    cardObject.transform.SetSiblingIndex(i + GameManager.Instance.Configs.MaxHandCount);
                }
                else
                {
                    cardObject.transform.SetSiblingIndex(i);
                }

                if (cardObject.State < CardState.ON_DRAG)
                {
                    cardObject.transform.localPosition = new Vector3((center + i) * spacing, 0.0f, 0.0f);
                }
            }
        }
    }
}
