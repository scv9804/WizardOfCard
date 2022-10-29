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

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        BattleCalculater.Inst.SpellEnchaneReset();

		Add_Burning_AllEnemy(_target_enemy, i_applyBurning);
		Attack_AllEnemy(_target_enemy, i_damage);

		yield return StartCoroutine(EndUsingCard());
	}
}
