using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using TMPro;

namespace BETA.UI
{
    // ==================================================================================================== DeckCardUIHandler

    public class DeckCardUIHandler : CardUIHandler
    {
        //

        //

        //

        [SerializeField, TitleGroup("≈ÿΩ∫∆Æ")]
        private TMP_Text _countTMP;

        //

        //

        public override void Refresh()
        {
            //CardManager.Instance.CardObjects["DECK"].Find();

            //if (CardManager.Instance.IsHandCardEmpty())
            //{
            //    return;
            //}

            var deck = CardManager.Instance.CardObjects[name];

            for (var i = 0; i < deck.Count; i++)
            {
                var cardObject = deck[i];

                cardObject.transform.SetParent(transform);
                cardObject.transform.SetSiblingIndex(i);
            }

            _countTMP.text = deck.Count.ToString();
        }
    }
}
