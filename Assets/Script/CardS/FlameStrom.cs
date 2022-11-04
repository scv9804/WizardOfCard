using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStrom : Card
{
    [Header("화상"), SerializeField] int[] applyBurning = new int[3];

    #region 프로퍼티

    public int i_applyBurning
    {
        get
        {
            return applyBurning[i_upgraded];
        }

        set
        {
            applyBurning[i_upgraded] = value;
        }
    }

    #endregion

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

        TargetAll(() => Add_Burning(_target_enemy, i_applyBurning), ref _target_enemy);
        TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), ref _target_enemy);

        yield return StartCoroutine(EndUsingCard());
	}
}
