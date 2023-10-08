using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA;

using UnityEngine.SceneManagement;

namespace TacticsToolkit 
{
    //Script for a playable character.
    public class CharacterManager : Entity
    {
        // ******
        public GameEvent BattleEnd;

        // Start is called before the first frame update
        public void Start()
        {
            // ******
            //GameObject.Find("EntityHasDie_Listener").GetComponent<GameEventGameObjectListener>().Response.AddListener(OnEntityDie);

            //if (EntityManager.Instance.PlayerData == null)
            //{
            //    EntityManager.Instance.PlayerData = new PlayerCharacterData(statsContainer);
            //}
            //else
            //{
            //    EntityManager.Instance.PlayerData.SetStats(statsContainer);

            //    UpdateCharacterUI();
            //}

            statsContainer = EntityManager.Instance.StatsContainer;

            statsContainer.Health.character = this;
            statsContainer.Mana.character = this;
            statsContainer.Strenght.character = this;
            statsContainer.Endurance.character = this;
            statsContainer.Speed.character = this;
            statsContainer.Intelligence.character = this;
            statsContainer.MoveRange.character = this;
            statsContainer.AttackRange.character = this;
            statsContainer.CurrentHealth.character = this;
            statsContainer.CurrentMana.character = this;
            statsContainer.Shield.character = this;

            statsContainer.CurrentMana.statValue = statsContainer.Mana.statValue;

            UpdateCharacterUI();
        }

        public override void OnEntityDie()
        {
            //var entity = character.GetComponent<Entity>();

            //if (entity.GetType() == typeof(EnemyController))
            //{
            //    Debug.Log("엥 이 친구 적 캐릭터인데요?");

            //    return;
            //}

            BattleEnd.Raise();

            SceneManager.LoadScene("GameOverScene");
        }
    }
}