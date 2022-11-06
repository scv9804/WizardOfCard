using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptalbe Object/Enemy")]
public class Enemy : ScriptableObject
{
	public int i_CharCode;
	public int i_health;
	public int i_attackCount;
	public int i_damage;
	public int increaseShield;
	public int buffValue;

	public int debuffValue;
	public float f_percentage;

	public string st_charName;
	public Sprite sp_sprite;
	public Sprite PlayerDamagedEffect;
	public Sprite EnemyDamagedSprite;
	public Sprite EnemyAttackSprite;


	public EntityPattern entityPattern;
}
