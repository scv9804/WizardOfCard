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

    public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
    {
        base.UseCard(_target_enemy, _target_player);
    }

    public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
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
