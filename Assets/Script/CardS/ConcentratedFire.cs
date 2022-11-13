using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentratedFire : Card
{
	#region Properties

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

	int I_AttackCount
	{
		get
		{
			return CardManager.Inst.myCards.Count + 1;
		}

		//set
		//{
		//    I_Count = value;
		//}
	}

	#endregion

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, I_Damage), I_AttackCount));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, I_Damage), I_AttackCount));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_enemy, I_Damage), I_AttackCount)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
    }
}
