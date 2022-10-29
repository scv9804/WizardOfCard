using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyRecovery : Card
{
    [Header("ī�� ��ο� ����"), SerializeField] bool b_canDraw;

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        if(b_canDraw)
        {
            sb.Append("\n ī�带 1�� ��ο��մϴ�.");
        }

        explainTMP.text = sb.ToString();
    }

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
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
