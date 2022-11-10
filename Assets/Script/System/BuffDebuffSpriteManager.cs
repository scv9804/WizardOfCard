using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffSpriteManager : MonoBehaviour
{
	public static BuffDebuffSpriteManager Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}

	[Header("EntityAttackPatternImages")]
	[SerializeField] Sprite warCrySprite;
	[SerializeField] Sprite shieldSprite;


	public Sprite WarCrySprite
	{
		get
		{
			return warCrySprite;
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
