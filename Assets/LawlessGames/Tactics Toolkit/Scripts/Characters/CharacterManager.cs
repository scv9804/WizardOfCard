using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameObject.Find("EntityHasDie_Listener").GetComponent<GameEventGameObjectListener>().Response.AddListener(OnEntityDie);
        }

        public void OnEntityDie(GameObject character)
        {
            var entity = character.GetComponent<Entity>();

            if (entity.GetType() == typeof(EnemyController))
            {
                Debug.Log("�� �� ģ�� �� ĳ�����ε���?");

                return;
            }

            BattleEnd.Raise();

            SceneManager.LoadScene("GameOverScene");
        }
    }
}