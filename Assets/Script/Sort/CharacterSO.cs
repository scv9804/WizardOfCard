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
	public Sprite sp_sprite;
}

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptalbe Object/CharacterSO")]
public class CharacterSO : ScriptableObject
{
	public PlayerChar[] playrChar;
}

