using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace TacticsToolkit.UI
{
    public class UIManager : MonoBehaviour
    {
        private List<Button> actionButtons;

        //public GameObject Hands;

        public TMP_Text CostTMP;

        // Start is called before the first frame update
        void Awake()
        {
            actionButtons = GetComponentsInChildren<Button>().ToList();
        }

        //If it's a character, enable all the UI. If it's an Enemy, disable all the UI.
        public void StartNewCharacterTurn(GameObject activeCharacter)
        {
            var entity = activeCharacter.GetComponent<Entity>();

            if (entity.teamID == 1)
            {
                EnableUI();

                //var maxMana = entity.GetStat(Stats.Mana).statValue;
                //entity.GetStat(Stats.CurrentMana).ChangeStatValue(maxMana);

                RefreshManaUI();
            }
            else
            {
                DisableUI();
            }
        }

        //Enable all the buttons.
        public void EnableUI()
        {
            foreach (var item in actionButtons)
            {
                item.interactable = true;
            }

            //foreach (var cardObject in Hands.GetComponentsInChildren<Image>())
            //{
            //    cardObject.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //}
        }

        //Disable all the buttons. 
        public void DisableUI()
        {
            foreach (var item in actionButtons)
            {
                item.interactable = false;
            }

            //foreach (var cardObject in Hands.GetComponentsInChildren<Image>())
            //{
            //    cardObject.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            //}
        }

        //Cancel an action and reenable the button. 
        public void CancelActionState(string actionButton)
        {
            var button = actionButtons.Where(x => x.GetComponentInChildren<Text>().text == actionButton).First();
            button.interactable = true;
        }

        public void RefreshManaUI()
        {
            var entity = GameObject.Find("Character 4(Clone)")?.GetComponent<Entity>();

            var mana = entity.GetStat(Stats.Mana).statValue;
            var currentMana = entity.GetStat(Stats.CurrentMana).statValue;

            CostTMP.text = $"{currentMana}/{mana}";
        }
    }
}
