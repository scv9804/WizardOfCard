using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyRecovery : Card
{
    [Header("카드 드로우 여부"), SerializeField] bool b_canDraw;

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        if(b_canDraw)
        {
            sb.Append("\n 카드를 1장 드로우합니다.");
        }

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

        T_ResotreHealth(i_damage);

        if(b_canDraw)
        {
            CardManager.Inst.AddCard();
        }

        yield return StartCoroutine(T_EndUsingCard());
    }
}
