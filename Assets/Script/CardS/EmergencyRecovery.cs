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

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        BattleCalculater.Inst.SpellEnchaneReset();

        RestoreHealth(i_damage);

        if(b_canDraw)
        {
            CardManager.Inst.AddCard();
        }

        yield return StartCoroutine(EndUsingCard());
    }
}
