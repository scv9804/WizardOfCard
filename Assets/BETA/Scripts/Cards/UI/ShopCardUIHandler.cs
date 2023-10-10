using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using TMPro;

namespace BETA.UI
{
    // ==================================================================================================== ShopCardUIHandler

    public class ShopCardUIHandler : CardUIHandler
    {
        //

        //

        //

        //[SerializeField, TitleGroup("텍스트")]
        //private TMP_Text _countTMP;

        [SerializeField, TitleGroup("상점")]
        private ShopManager _shop;

        //

        //

        public override void Refresh()
        {
            //CardManager.Instance.CardObjects["DECK"].Find();

            //if (CardManager.Instance.IsHandCardEmpty())
            //{
            //    return;
            //}

            //var shop = CardManager.Instance.CardObjects[name];

            //for (var i = 0; i < shop.Count; i++)
            //{
            //    var cardObject = shop[i];

            //    cardObject.transform.SetParent(transform);
            //    cardObject.transform.SetSiblingIndex(i);
            //}

            _shop.Refresh();

            //_countTMP.text = deck.Count.ToString();
        }
    }
}
