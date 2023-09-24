using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

using Sirenix.OdinInspector;

//using Spine.Unity;

using System;

using TacticsToolkit;

namespace BETA
{
    // ==================================================================================================== Entity

    public sealed class Entity : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== Legacy

        [FoldoutGroup("������ ����")]
        public List<AbilityContainer> abilitiesForUse;

        [FoldoutGroup("������ ����")]
        public int teamID = 0;

        [FoldoutGroup("������ ����")]
        public CharacterClass characterClass;

        [FoldoutGroup("������ ����")]
        public int initiativeValue;

        [FoldoutGroup("������ ����")]
        public bool isAlive = true;

        [FoldoutGroup("������ ����")]
        public bool isActive;

        [FoldoutGroup("������ ����")]
        public GameEvent endTurn;

        [FoldoutGroup("������ ����")]
        public int previousTurnCost = -1;

        [FoldoutGroup("������ ����")]
        private bool isTargetted = false;

        [FoldoutGroup("������ ����")]
        public SpriteRenderer myRenderer;

        //public SpriteRenderer SpriteRenderer;
        //public SpineRenderer SpineRenderer;

        public GameEventGameObject EntityDie;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            SpawnCharacter();
        }

        // =========================================================================== Lagecy

        public void SpawnCharacter()
        {
            SetAbilityList();
            //SetStats();

            //initiativeValue = Mathf.RoundToInt(initiativeBase / GetStat(Stats.Speed).statValue);
        }

        public void SetAbilityList()
        {
            abilitiesForUse = new List<AbilityContainer>();

            foreach (var ability in characterClass.abilities)
            {
                abilitiesForUse.Add(new AbilityContainer(ability));
            }
        }

        public void UpdateInitiative(int turnValue)
        {
            initiativeValue += Mathf.RoundToInt(turnValue / 10 + 1);

            previousTurnCost = turnValue;
        }

        public void SetTargeted(bool focused = false)
        {
            isTargetted = focused;

            if (isAlive)
            {
                if (isTargetted)
                {
                    myRenderer.color = new Color(1, 0, 0, 1);
                }
                else
                {
                    myRenderer.color = new Color(1, 1, 1, 1);
                }
            }
        }

        public void TakeDamage(int damage, bool ignoreDefence = false)
        {
            //int damageToTake = ignoreDefence ? damage : CalculateDamage(damage);

            //if (damageToTake > 0)
            //{
            //    statsContainer.CurrentHealth.statValue -= damageToTake;
            //    CameraShake.Shake(0.125f, 0.1f);
            //    UpdateCharacterUI();

            //    if (GetStat(Stats.CurrentHealth).statValue <= 0)
            //    {
            //        isAlive = false;
            //        StartCoroutine(Die());
            //        UnlinkCharacterToTile();

            //        if (isActive)
            //            endTurn.Raise();
            //    }
            //}
        }

        public void HealEntity(int value)
        {
            //statsContainer.CurrentHealth.statValue += value;
            //UpdateCharacterUI();
        }

        //private int CalculateDamage(int damage)
        //{
        //    float percentage = (((float)GetStat(Stats.Endurance).statValue / (float)damage) * 100) / 2;

        //    percentage = percentage > 75 ? 75 : percentage;

        //    int damageToTake = damage - Mathf.CeilToInt((float)(percentage / 100f) * (float)damage);
        //    return damageToTake;
        //}
    }
}
