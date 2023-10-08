using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class SpawnPattern
//{
//	[Tooltip("3개까지 가능")] public Enemy[] enemy;
//	public int[] Reward_item;
//	public int Reward_Money;
//	public int[] Reward_Card;
//	public bool MoneyRandom;
//}

[CreateAssetMenu(fileName = "EnemySpawnPatternSO", menuName = "Scriptalbe Object/EnemySpawnPatternSO")]
public class EnemySpawnPatternSO : ScriptableObject
{
	public SpawnPattern[] spawnPattern;
}
