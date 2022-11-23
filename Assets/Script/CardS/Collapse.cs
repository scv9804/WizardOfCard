using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("���� ���� �� ������ ����"), SerializeField] int[] damageEnhanceValue = new int[3];

	#region Properties

	int I_DamageEnhanceValue
	{
		get
		{
			return damageEnhanceValue[i_upgraded];
		}

		//set
		//{
		//    damageEnhanceValue[i_upgraded] = value;
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

	public override string ExplainRefresh()
    {
        base.ExplainRefresh();

		sb.Replace("{3}", I_DamageEnhanceValue.ToString());

		explainTMP.text = sb.ToString();

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, ShieldBreak(_target_enemy, I_Damage));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, ShieldBreak(_target_player, I_Damage));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, ShieldBreak(_target_enemy, I_Damage)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}

	int ShieldBreak(Entity _target, int _value)
	{
		if (_target.i_shield > 0)
		{
			return _value * I_DamageEnhanceValue;
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
			return _value * I_DamageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}
}
