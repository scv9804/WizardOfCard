using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �߰�����
[System.Serializable]
public class PlayerChar
{
	public int i_CharCode;
	public int i_health;
	public string st_charName;
	public int ShiledEnchant;

	public Sprite sp_sprite;
	public Sprite damagedSprite;
	public Sprite MagicBoltSprite;
	public Sprite WandAttackEffect;
}

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptalbe Object/CharacterSO")]
public class CharacterSO : ScriptableObject
{
	public PlayerChar[] playrChar;
}

