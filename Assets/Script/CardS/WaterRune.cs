using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRune : Card
{
	#region Properties

	int I_Heal
	{
		get
		{
			return ApplyEnhanceValue(i_damage);
		}

		//set
		//{
		//    I_Heal = value;
		//}
	}

	#endregion

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
        {
			RestoreHealth(I_Heal);

			CardManager.Inst.AddCard();

			yield return new WaitForSeconds(0.15f);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
