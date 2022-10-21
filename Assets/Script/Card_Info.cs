using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Info : MonoBehaviour
{
	public enum CardType { Spell, Spell_Enhance, Shlied, Heal, Buff, Debuff };

	public int i_itemNum;

	public string st_cardName;
	public int i_attack;
	public int i_Cost;
	public Utility_enum.AttackRange attackRange;

	public float f_percentage;
	[TextArea] public string st_explainCard_UI;
	[TextArea] public string st_explainCard;

	public bool b_isExile;

	public CardType type;
	public Sprite sp_CardSprite;
	public SpriteRenderer enemyDamageSprite;
	public SpriteRenderer playerAttackSprite;
}
