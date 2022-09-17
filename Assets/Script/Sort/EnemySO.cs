using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	[System.Serializable]
	public class Enemy
	{
		public int i_CharCode;
		public int i_health;
		public int i_attackCount;
		public int i_damage;

		public float f_percentage;

		public string st_charName;
		public Sprite sp_sprite;
		public Sprite PlayerDamagedEffect;
		public Sprite EnemyDamagedSprite;
		public Sprite EnemyAttackSprite;


		public EntityPattern entityPattern;
}

	[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptalbe Object/EnemySO")]
	public class EnemySO : ScriptableObject
	{
		public Enemy[] enemy;
	}

