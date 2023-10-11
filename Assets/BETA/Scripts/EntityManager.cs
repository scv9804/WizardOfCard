using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Singleton;

using Sirenix.OdinInspector;

using System;

using TacticsToolkit;

namespace BETA
{
    // ==================================================================================================== EntityManager

    public sealed class EntityManager : SingletonMonoBehaviour<EntityManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Entity

        [FoldoutGroup("캐릭터")]
        //public PlayerCharacterData PlayerData;
        public CharacterStats StatsContainer;

        public CharacterClass ClassData;

        public int Money;

        // =========================================================================== EventDispatcher

        [SerializeField, TitleGroup("엔티티매니저 이벤트")]
        private EntityManagerEvent _events;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void OnEnable()
        {
            _events.OnGameStart.Listener += OnGameStart;
        }

        private void OnDisable()
        {
            _events.OnGameStart.Listener -= OnGameStart;
        }

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Entity Manager";

                DontDestroyOnLoad(gameObject);
            }

            return isEmpty;
        }

        // =========================================================================== Entity

        private void CreatePlayerData()
        {
            var stats = ScriptableObject.CreateInstance<CharacterStats>();

            stats.Health = new Stat(Stats.Health, ClassData.Health.baseStatValue, null);
            stats.Mana = new Stat(Stats.Mana, ClassData.Mana.baseStatValue, null);
            stats.Strenght = new Stat(Stats.Strenght, ClassData.Strenght.baseStatValue, null);
            stats.Endurance = new Stat(Stats.Endurance, ClassData.Endurance.baseStatValue, null);
            stats.Speed = new Stat(Stats.Speed, ClassData.Speed.baseStatValue, null);
            stats.Intelligence = new Stat(Stats.Intelligence, ClassData.Intelligence.baseStatValue, null);
            stats.MoveRange = new Stat(Stats.MoveRange, ClassData.MoveRange, null);
            stats.AttackRange = new Stat(Stats.AttackRange, ClassData.AttackRange, null);
            stats.CurrentHealth = new Stat(Stats.CurrentHealth, ClassData.Health.baseStatValue, null);
            stats.CurrentMana = new Stat(Stats.CurrentMana, ClassData.Mana.baseStatValue, null);
            stats.Shield = new Stat(Stats.Shield, 0, null);

            StatsContainer = stats;
        }

        // =========================================================================== GameEvent

        private void OnGameStart()
        {
            CreatePlayerData();

            Money = 0;

            SetMoney(100);
        }

        public void OnTurnStart(GameObject character)
        {
            var entity = character.GetComponent<Entity>();

            if (entity.teamID == 1)
            {
                //var maxMana = entity.GetStat(Stats.Mana).statValue;
                entity.RestoreMana();
            }

            entity.SetShield(0);
        }

        public void OnEntityDie(GameObject character)
        {
            var entity = character.GetComponent<Entity>();

            entity.OnEntityDie();
        }

        public void OnBattleEnd()
        {
            var player = GameObject.Find("Character 4(Clone)").GetComponent<CharacterManager>();

            //PlayerData = new PlayerCharacterData(player.statsContainer);

            player.SetShield(0);
        }

        public void SetMoney(int value)
        {
            Money += value;

            var moneyTMP = GameObject.Find("Money_TMP")?.GetComponent<TMPro.TMP_Text>();

            moneyTMP.Require(() =>
            {
                moneyTMP.text = Money.ToString();
            });
        }
    }
}
