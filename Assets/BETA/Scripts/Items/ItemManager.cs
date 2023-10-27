using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;
using BETA.UI;

using Sirenix.OdinInspector;

using System;

using TacticsToolkit;

using UnityEngine.SceneManagement;

namespace BETA
{
    // ==================================================================================================== ItemManager

    public class ItemManager : SingletonMonoBehaviour<ItemManager>
    {
        //

        //

        [SerializeField, TitleGroup("아이템 슬롯")]
        private Dictionary<ItemType, bool> _isEquiped = new Dictionary<ItemType, bool>();

        //

        //

        [SerializeField, TitleGroup("게임 관련 이벤트")]
        private EventDispatcher _onGameStart;

        [SerializeField, TitleGroup("게임 관련 이벤트")]
        private EventDispatcher _onGameEnd;

        //

        [SerializeField, TitleGroup("전투 관련 이벤트")]
        private EventDispatcher _onBattleEnd;

        //

        [SerializeField, TitleGroup("턴 관련 이벤트")]
        private EventDispatcher _onTurnEnd;

        //

        [SerializeField, TitleGroup("캐릭터 관련 이벤트")]
        public GameEventGameObject HealthChange;

        //

        [SerializeField, TitleGroup("아이템 관련 이벤트")]
        private EventDispatcher _onItemSlotRefresh;

        //

        //

        public Dictionary<ItemType, bool> IsEquiped
        {
            get => _isEquiped;

            private set => _isEquiped = value;
        }

        // ==================================================================================================== Method

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Item Manager";

                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded -= OnSceneWasLoaded;
                SceneManager.sceneLoaded += OnSceneWasLoaded;

                _onGameStart.Listener += OnGameStart;
                _onGameEnd.Listener += OnGameEnd;
            }

            return isEmpty;
        }

        protected override bool Finalize()
        {
            var isEmpty = base.Finalize();

            if (!isEmpty)
            {
                SceneManager.sceneLoaded -= OnSceneWasLoaded;

                _onGameStart.Listener -= OnGameStart;
                _onGameEnd.Listener -= OnGameEnd;
            }

            return isEmpty;
        }

        // =========================================================================== Scene

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            _onItemSlotRefresh.Launch();
        }

        // =========================================================================== EventDispatcher

        //

        private void OnGameStart()
        {
            IsEquiped[ItemType.CLOTH] = false;
            IsEquiped[ItemType.RING] = false;
        }

        private void OnGameEnd()
        {
            IsEquiped[ItemType.CLOTH] = false;
            IsEquiped[ItemType.RING] = false;

            ChangeClothListener();
            ChangeRingListener();
        }

        //

        private void OnBattleEnd_Ring()
        {
            if (EntityManager.IsEmpty || EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            EntityManager.Instance.SetMoney(160);
        }

        //

        private void OnTurnEnd_Cloth()
        {
            if (EntityManager.IsEmpty || EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            var controller = GameObject.Find("BattleController").GetComponent<BattleController>();

            var player = GameObject.Find("Character 4(Clone)").GetComponent<CharacterManager>();

            if (controller.activeCharacter == player)
            {
                player.HealEntity(20);
            }
        }

        // =========================================================================== Item

        public void Equip(ItemType type, bool isEquip)
        {
            IsEquiped[type].Log();
            isEquip.Log();

            if (IsEquiped[type] == isEquip)
            {
                return;
            }

            IsEquiped[type] = isEquip;

            if (EntityManager.IsEmpty || EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            switch (type)
            {
                case ItemType.CLOTH:
                    Equip_Cloth();
                    break;
                case ItemType.RING:
                    Equip_Ring();
                    break;
                default:
                    break;
            }
        }

        private void Equip_Cloth()
        {
            if (EntityManager.IsEmpty || EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            var container = EntityManager.Instance.StatsContainer;

            var health = container.Health;
            var currentHealth = container.CurrentHealth;

            var endurance = container.Endurance;

            if (IsEquiped[ItemType.CLOTH])
            {
                health.ChangeStatValue(health.statValue + 80);

                endurance.ChangeStatValue(endurance.statValue + 70);
            }
            else
            {
                health.ChangeStatValue(health.statValue - 80);
                currentHealth.ChangeStatValue(Math.Min(health.statValue, currentHealth.statValue));

                endurance.ChangeStatValue(endurance.statValue - 70);
            }

            HealthChange.Raise(gameObject);

            ChangeClothListener();
        }

        private void ChangeClothListener()
        {
            if (IsEquiped[ItemType.CLOTH])
            {
                _onTurnEnd.Listener += OnTurnEnd_Cloth;
            }
            else
            {
                _onTurnEnd.Listener -= OnTurnEnd_Cloth;
            }
        }

        private void Equip_Ring()
        {
            if (EntityManager.IsEmpty || EntityManager.Instance.StatsContainer == null)
            {
                return;
            }

            ChangeRingListener();
        }

        private void ChangeRingListener()
        {
            if (IsEquiped[ItemType.RING])
            {
                _onBattleEnd.Listener += OnBattleEnd_Ring;
            }
            else
            {
                _onBattleEnd.Listener -= OnBattleEnd_Ring;
            }
        }
    } 
}
