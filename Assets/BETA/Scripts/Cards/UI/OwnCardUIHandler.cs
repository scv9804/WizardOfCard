using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

using TMPro;

namespace BETA.UI
{
    // ==================================================================================================== OwnCardUIHandler

    public class OwnCardUIHandler : CardUIHandler
    {
        //

        //

        //

        [SerializeField, TitleGroup("카드 데이터")]
        private Dictionary<string, CardEventSystems> _ownCardCommands = new Dictionary<string, CardEventSystems>();

        //

        [SerializeField, TitleGroup("텍스트")]
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

            var own = CardManager.Instance.CardObjects[name];

            for (var i = 0; i < own.Count; i++)
            {
                var cardObject = own[i];

                cardObject.Commands = _ownCardCommands;

                cardObject.transform.SetParent(transform);
                cardObject.transform.SetSiblingIndex(i);
            }

            _countTMP.text = own.Count.ToString();
        }
    }
}
