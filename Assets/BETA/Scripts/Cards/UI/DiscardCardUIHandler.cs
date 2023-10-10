using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using TMPro;

namespace BETA.UI
{
    // ==================================================================================================== DiscardCardUIHandler

    public class DiscardCardUIHandler : CardUIHandler
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

            var discard = CardManager.Instance.CardObjects[name];

            for (var i = 0; i < discard.Count; i++)
            {
                var cardObject = discard[i];

                cardObject.transform.SetParent(transform);
                cardObject.transform.SetSiblingIndex(i);
            }

            _countTMP.text = discard.Count.ToString();
        }
    }
}
