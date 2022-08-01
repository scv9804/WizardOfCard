using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards_Magician : MonoBehaviour
{
	public void MagicBolt(Card _card, Entity _target)
	{
		_target?.Damaged(_card.i_damage);
		BattleCalculater.Inst.SpellEnchaneReset();
	}

}



