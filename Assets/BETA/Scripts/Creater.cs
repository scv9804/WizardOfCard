using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TacticsToolkit;

namespace BETA.Editor
{
    public class Creater : MonoBehaviour
    {
        public Vector2Int SpawnLocation;

        public CharacterManager SpawnPlayerCharacter;

        public MovementController MovementController;
        public TurnBasedController TurnBasedController;
        public EnemyContainer EnemyContainer;

        public GameEvent StartLevel;

        private void Start()
        {
            var character = Instantiate(SpawnPlayerCharacter).GetComponent<CharacterManager>();

            var entity = character.GetComponent<TacticsToolkit.Entity>();
            var gameObject = character.gameObject;

            MovementController.PositionCharacterOnTile(entity, MapManager.Instance.map[SpawnLocation]);
            TurnBasedController.SpawnNewCharacter(gameObject);
            EnemyContainer.SpawnPlayerCharacter(gameObject);

            StartLevel.Raise();
        }
    } 
}
