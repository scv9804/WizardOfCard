using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UnstableTome : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		for (int i = 0; i < i_damage; i++)
		{
			if(CardManager.Inst.myDeck.Count > 0)
            {
				CardManager.Inst.AddCard();

				CardManager.Inst.myCards.Last().i_manaCost = 0;
				CardManager.Inst.myCards.Last().b_isExile = true;

				CardManager.Inst.myCards.Last().ManaCostRefresh();
				CardManager.Inst.myCards.Last().ExplainRefresh();
			}
		}
	}
}
