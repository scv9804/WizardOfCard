using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseEffectManager : MonoBehaviour
{
	public static EnemyBaseEffectManager Inst{ get; private set; }

	private void Awake()
	{
		Inst = this;	
	}

	[Header("EntityAttackPatternImages")]
    [SerializeField] Sprite attackSprite;
	[SerializeField] Sprite shieldSprite;


	public Sprite AttackSprite
	{
		get
		{
			return attackSprite;
		}
	}

	public Sprite ShieldSprite
	{
		get
		{
			return shieldSprite;
		}
	}



}
