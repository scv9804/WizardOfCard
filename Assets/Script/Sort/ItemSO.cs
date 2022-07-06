using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {Spell, Spell_Enhance, Shlied, Heal, Buff, Debuff};

[System.Serializable]
public class Item
{
	public int i_itemNum;

	public string st_cardName;
	public int i_attack;
	public int i_Cost;

	public float f_percentage;
	public string st_explainCard;
	public Sprite sp_CharacterSprite;

	public CardType type;
	public Sprite sp_CardSprite;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptalbe Object/ItemSO")]
public class ItemSO : ScriptableObject
{
	public Item[] items;
} 