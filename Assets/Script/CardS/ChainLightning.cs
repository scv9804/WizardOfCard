using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Card
{
    [Header("���� Ƚ��"), SerializeField] int i_attackCount;
	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Append("\n{3}�� �� �ݺ��մϴ�.\n");
		sb.Replace("{3}", (i_attackCount - 1).ToString());

		explainTMP.text = sb.ToString();
	}

	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		yield return StartCoroutine(Repeat(() => T_Attack_RandomEnemy(_target_enemy, i_damage), i_attackCount));

		Debug.Log("�ƴ� ī�尡 ���� ����Ѵٴϱ�?");

		yield return StartCoroutine(T_EndUsingCard());
	}
}
