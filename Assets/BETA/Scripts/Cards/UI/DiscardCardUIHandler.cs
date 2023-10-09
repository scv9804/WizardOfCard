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

        [SerializeField, TitleGroup("ÅØ½ºÆ®")]
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

            for (var i = discard.Count - 1; i > -1; i--)
            {
                var cardObject = discard[i];

                cardObject.transform.SetSiblingIndex(i);
            }

            _countTMP.text = discard.Count.ToString();
        }
    }
}
