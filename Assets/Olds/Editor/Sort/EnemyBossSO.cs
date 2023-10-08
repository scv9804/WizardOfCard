using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBoss
{
	public int i_CharCode;
	public int i_health;
	public int i_attackCount;
	public int i_damage;

	public float f_percentage;

	public string st_charName;
	public Sprite sp_sprite;
}

[CreateAssetMenu(fileName = "EnemyBossSO", menuName = "Scriptalbe Object/EnemyBossSO")]
public class EnemyBossSO : ScriptableObject
{
	public EnemyBoss[] enemyBoss;
}

