using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneCheaker : MonoBehaviour
{
	private void Start()
	{
		TurnManager.Inst.IsCombatScene = true;
		StartCoroutine(LevelGeneration.Inst.Co_StartGame());
		EntityManager.Inst.SetEnemyObjectArray();
	}

}
