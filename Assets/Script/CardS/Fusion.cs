using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : Card
{
	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyEnhanceValue(i_damage).ToString());

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Add_Burning(_target_enemy, i_damage);

		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Add_Burning(_target_player, i_damage);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Add_Burning(_target_enemy, i_damage), ref _target_enemy);
		}

		if(i_upgraded == 2)
        {
			CardManager.Inst.AddCard();
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
