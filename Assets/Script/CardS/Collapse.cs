using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : Card
{
	[Header("���� ���� �� ������ ����"), SerializeField] int[] damageEnhanceValue = new int[3];

	#region ������Ƽ

	public int i_damageEnhanceValue
	{
		get
		{
			return damageEnhanceValue[i_upgraded];
		}

		//set
		//{
		//	damageEnhanceValue[i_upgraded] = value;
		//}
	}

	#endregion

	public override void ExplainRefresh()
    {
        base.ExplainRefresh();

		sb.Replace("{3}", i_damageEnhanceValue.ToString());

		explainTMP.text = sb.ToString();
    }

	int ShieldBreak(Entity _target, int _value)
	{
		if (_target.i_shield > 0)
		{
			return _value * i_damageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}

	int ShieldBreak(PlayerEntity _target, int _value)
	{
		if (_target.Status_Shiled > 0)
		{
			return _value * i_damageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage)));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage)));
		}
		else // ���� �Ǵ� ������ ��� (?) + �� ī��� Ư���� ��� ����;;;
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage))), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
