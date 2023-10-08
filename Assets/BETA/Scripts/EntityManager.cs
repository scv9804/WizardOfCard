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
        public PlayerCharacterData PlayerData;
        public CharacterStats StatsContainer;

        public CharacterClass ClassData;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            OnGameStarted();
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
        }

        public void OnEntityDie(GameObject character)
        {
            var entity = character.GetComponent<Entity>();

            entity.OnEntityDie();
        }

        public void OnBattleEnd()
        {
            var player = GameObject.Find("Character 4(Clone)").GetComponent<CharacterManager>();

            PlayerData = new PlayerCharacterData(player.statsContainer);
        }
    }

    public class PlayerCharacterData
    {
        //

        //

        public int MaxHealth;
        public int CurrentHealth;

        public int MaxMana;
        public int CurrentMana;

        public int Strength;
        public int Intelligence;

        public int Endurance;

        public int Speed;

        public int MoveRange;
        public int AttackRange;

        //

        //

        public PlayerCharacterData() { }

        public PlayerCharacterData(CharacterStats stats)
        {
            MaxHealth = stats.getStat(Stats.Health).statValue;
            CurrentHealth = stats.getStat(Stats.CurrentHealth).statValue;

            MaxMana = stats.getStat(Stats.Mana).statValue;
            CurrentMana = stats.getStat(Stats.CurrentMana).statValue;

            Strength = stats.getStat(Stats.Strenght).statValue;
            Intelligence = stats.getStat(Stats.Intelligence).statValue;

            Endurance = stats.getStat(Stats.Endurance).statValue;

            Speed = stats.getStat(Stats.Speed).statValue;

            MoveRange = stats.getStat(Stats.MoveRange).statValue;
            AttackRange = stats.getStat(Stats.AttackRange).statValue;
        }

        public void SetStats(CharacterStats stats)
        {
            stats.getStat(Stats.Health).ChangeStatValue(MaxHealth);
            stats.getStat(Stats.CurrentHealth).ChangeStatValue(CurrentHealth);

            stats.getStat(Stats.Mana).ChangeStatValue(MaxMana);
            stats.getStat(Stats.CurrentMana).ChangeStatValue(CurrentMana);

            stats.getStat(Stats.Strenght).ChangeStatValue(Strength);
            stats.getStat(Stats.Intelligence).ChangeStatValue(Intelligence);

            stats.getStat(Stats.Endurance).ChangeStatValue(Endurance);

            stats.getStat(Stats.Speed).ChangeStatValue(Speed);

            stats.getStat(Stats.MoveRange).ChangeStatValue(MoveRange);
            stats.getStat(Stats.AttackRange).ChangeStatValue(AttackRange);
        }
    }
}
