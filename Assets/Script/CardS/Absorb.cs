using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : Card
{
	#region Properties

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

		if (CardManager.Inst.myCards.Count > 1)
		{
			do
			{
				if (CardManager.Inst.myCards[I_Index] != this)
					break;
			}
			while (true);

			PlayerEntity.Inst.Status_MaxAether_Battle += CardManager.Inst.myCards[I_Index].i_manaCost;

			if (i_upgraded == 2)
				CardManager.Inst.myCemetery.Add(CardManager.Inst.myCards[I_Index]);

			else
				CardManager.Inst.myExiledCards.Add(CardManager.Inst.myCards[I_Index]);

			CardManager.Inst.myCards[I_Index].gameObject.SetActive(false);
			CardManager.Inst.myCards.RemoveAt(I_Index);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
