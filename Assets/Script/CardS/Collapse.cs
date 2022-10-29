using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : Card
{
	[Header("���� ���� �� ������ ����"), SerializeField] int i_DamageEnhanceValue;

	public override void ExplainRefresh()
    {
        base.ExplainRefresh();

		sb.Replace("{3}", i_DamageEnhanceValue.ToString());

		explainTMP.text = sb.ToString();
    }

	int ShieldBreak(Entity _target, int _value)
	{
		if (_target.i_shield > 0)
		{
			return _value * i_DamageEnhanceValue;
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
			return _value * i_DamageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, ShieldBreak(_target_enemy, i_damage));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, ShieldBreak(_target_player, i_damage));
		}
		else // ���� �Ǵ� ������ ��� (?) + �� ī��� Ư���� ��� ����;;;
		{
			for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
			{
				_target_enemy = EntityManager.Inst.enemyEntities[i];

				if (!_target_enemy.is_die)
				{
					Attack(_target_enemy, ShieldBreak(_target_enemy, i_damage));
				}
			}
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
