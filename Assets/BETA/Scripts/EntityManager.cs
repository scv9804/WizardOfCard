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

        [FoldoutGroup("Ä³¸¯ÅÍ")]
        //public PlayerCharacterData PlayerData;
        public CharacterStats StatsContainer;

        public CharacterClass ClassData;

        public int Money;

        // ==================================================================================================== Method

        // =========================================================================== Event

        //private void Start()
        //{
        //    OnGameStarted();
        //}

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Entity Manager";

                DontDestroyOnLoad(gameObject);

                GameManager.OnGameStart -= OnGameStarted;
                GameManager.OnGameStart += OnGameStarted;
            }

            return isEmpty;
        }

        protected override bool Finalize()
        {
            var isEmpty = base.Finalize();

            if (!isEmpty)
            {
                GameManager.OnGameStart -= OnGameStarted;
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

        private void OnGameStarted()
        {
            CreatePlayerData();
        }

        public void OnTurnStart(GameObject character)
        {
            var entity = character.GetComponent<Entity>();

            if (entity.teamID == 1)
            {
                var maxMana = entity.GetStat(Stats.Mana).statValue;
                entity.GetStat(Stats.CurrentMana).ChangeStatValue(maxMana);
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
    }
}
