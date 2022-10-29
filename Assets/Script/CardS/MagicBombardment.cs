using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBombardment : Card
{
	[Header("���� Ƚ��"), SerializeField] int i_attackCount;
	[Header("������ �� �߰����� ����"), SerializeField] bool b_canExtraAttack;

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		if (i_attackCount > 1)
		{
			sb.Append("\n{3}�� �� �ݺ��մϴ�.\n");
			sb.Replace("{3}", (i_attackCount - 1).ToString());
		}

		if (b_canExtraAttack)
        {
			sb.Append("�� �� ������ ������\n �������� <color=#ff0000>{1}</color> �ݴϴ�.");
			sb.Replace("{1}", ApplyManaAffinity(i_damage).ToString());
		}

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

        yield return StartCoroutine(Repeat(() => Attack_AllEnemy(_target_enemy, i_damage), i_attackCount));

        if (b_canExtraAttack)
		{
			Attack_RandomEnemy(_target_enemy, i_damage);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
