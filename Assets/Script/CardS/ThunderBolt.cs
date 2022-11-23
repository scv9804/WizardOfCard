using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("�ִ�"), SerializeField] int[] maxDamage = new int[3];

	#region Properties

	int I_MaxDamage
	{
		get
		{
			return ApplyMagicAffinity(maxDamage[i_upgraded]);
		}

		//set
		//{
		//	maxDamage[i_upgraded] = value;
		//}
	}

	#endregion

	public override string ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff0000>{4}</color>");
		sb.Replace("{4}", I_MaxDamage.ToString());

		explainTMP.text = sb.ToString();

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		int damage = ApplyMagicAffinity(Random.Range(i_damage, maxDamage[i_upgraded] + 1)); // ���� ��� �� ������ ����

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, damage);
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, damage);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
