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

		RepeatAttack_AllEnemy(i_attackCount, ApplyManaAffinity(i_damage));

		if (b_canExtraAttack)
		{
			Attack_Defalut(EntityManager.Inst.enemyEntities[Random.Range(0, EntityManager.Inst.enemyEntities.Count)], ApplyManaAffinity(i_damage));
		}

		BattleCalculater.Inst.SpellEnchaneReset();
	}
}
