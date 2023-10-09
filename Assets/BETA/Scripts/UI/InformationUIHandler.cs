using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TacticsToolkit;

using TMPro;

using UnityEngine.UI;

namespace BETA.UI
{
    // ==================================================================================================== InformationUIHandler

    public class InformationUIHandler : UIHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Component

        // ======================================================= Image

        [SerializeField, TitleGroup("이미지")]
        private Image _healthImage;

        // ======================================================= Text

        [SerializeField, TitleGroup("텍스트")]
        private TMP_Text _healthTMP;

        [SerializeField, TitleGroup("텍스트")]
        private TMP_Text _moneyTMP;

        //[SerializeField, TitleGroup("텍스트")]
        //private TMP_Text _turnTMP;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            Refresh();
        }

        // =========================================================================== UI

        public override void Refresh()
        {
            if (EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            var health = EntityManager.Instance.StatsContainer.Health.statValue;
            var currentHealth = EntityManager.Instance.StatsContainer.CurrentHealth.statValue;

            var money = EntityManager.Instance.Money;

            SetHealthFillAmount(currentHealth, health);
            SetHealthTMP(currentHealth);

            SetMoneyTMP(money);

            #region void SetHealthFillAmount(float current, float max);

            void SetHealthFillAmount(float current, float max)
            {
                if (_healthImage == null)
                {
                    return;
                }

                _healthImage.fillAmount = current / max;
            }

            #endregion

            #region void SetHealthTMP(float current);

            void SetHealthTMP(float current)
            {
                if (_healthTMP == null)
                {
                    return;
                }

                _healthTMP.text = current.ToString();
            }

            #endregion

            #region void SetMoneyTMP(int money);

            void SetMoneyTMP(int money)
            {
                if (_moneyTMP == null)
                {
                    return;
                }

                _moneyTMP.text = money.ToString();
            }

            #endregion
        }

        // =========================================================================== GameEvent

        //public void OnPlayerHealthChanged(GameObject character)
        //{
        //    var entity = GetComponent<Entity>();

        //    if (entity.teamID == 1)
        //    {
        //        Refresh();
        //    }
        //}
    }
}
