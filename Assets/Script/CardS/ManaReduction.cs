using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaReduction : Card
{
    #region 프로퍼티

	int I_Index
    {
        get
        {
			return Random.Range(0, CardManager.Inst.myCards.Count);

		}

		//set
		//{
		//    I_Index = value;
		//}
	}

	#endregion


	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		do
		{
			if (CardManager.Inst.myCards[I_Index] != this)
				break;
		} 
		while (true);

		CardManager.Inst.myCards[I_Index].i_manaCost++;

		RestoreAether(i_damage);

		if (i_upgraded == 2)
			CardManager.Inst.AddCard();

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}
}
