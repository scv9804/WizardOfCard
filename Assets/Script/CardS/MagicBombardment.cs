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

	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//StartCoroutine(Repeat(() => Attack_AllEnemy(ApplyManaAffinity(i_damage)), i_attackCount));
		////StartCoroutine(RepeatC_AAE(i_attackCount));

		//if (b_canExtraAttack)
		//{
		//	Attack_SingleEnemy(EntityManager.Inst.enemyEntities[Random.Range(0, EntityManager.Inst.enemyEntities.Count)], ApplyManaAffinity(i_damage));
		//}

		//BattleCalculater.Inst.SpellEnchaneReset();
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

        yield return StartCoroutine(Repeat(() => T_Attack_AllEnemy(_target_enemy, i_damage), i_attackCount));

        if (b_canExtraAttack)
		{
			T_Attack_RandomEnemy(_target_enemy, i_damage);
		}

		Debug.Log("�ƴ� ī�尡 ���� ����Ѵٴϱ�?");

		yield return StartCoroutine(T_EndUsingCard());
	}
}
