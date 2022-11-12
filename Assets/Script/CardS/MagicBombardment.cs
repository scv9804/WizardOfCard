using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBombardment : Card
{
	[Header("���� Ƚ��"), SerializeField] int[] attackCount = new int[3];

	#region Properties

	int I_AttackCount
	{
		get
		{
			return attackCount[i_upgraded];
		}

		//set
		//{
		//    I_Count = value;
		//}
	}

	int I_Damage
	{
		get
		{
			return ApplyMagicAffinity(i_damage);
		}

		//set
		//{
		//    I_Damage = value;
		//}
	}

	#endregion

	// <<22-11-09 ������ :: ����>>
	//public override void ExplainRefresh()

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		yield return StartCoroutine(Repeat(() => TargetAll(() => Attack(_target_enemy, I_Damage), ref _target_enemy), I_AttackCount)); // ������;;;

        if (i_upgraded == 2)
		{
			Attack_RandomEnemy(_target_enemy, I_Damage);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
