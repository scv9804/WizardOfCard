using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStrom : Card
{
    [Header("화상"), SerializeField] int i_applyBurning;
    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
        sb.Replace("{4}", ApplyEnhanceValue(i_applyBurning).ToString());

        explainTMP.text = sb.ToString();
    }

	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		T_Apply_Burning_AllEnemy(_target_enemy, i_applyBurning);
		T_Attack_AllEnemy(_target_enemy, i_damage);

		yield return StartCoroutine(T_EndUsingCard());
	}
}
