using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousAttack : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("������ ������"), SerializeField] int[] damagePerCard = new int[3];

	#region Properties
	int I_DamagePerCard
	{
		get
		{
			return damagePerCard[i_upgraded];
		}

		//set
		//{
		//	damagePerCard[i_upgraded] = value;
		//}
	}

	int I_Damage
	{
		get
		{
			return ApplyMagicAffinity(i_damage + I_DamagePerCard);
		}

		//set
		//{
		//    I_Damage = value;
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
			Attack(_target_enemy, I_Damage);
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, I_Damage);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, I_Damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
