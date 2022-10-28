using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHasteBarrier : Card
{
    [Header("ī�� ��ο�"), SerializeField] int i_drawCount;

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{3}", "<color=#ff00ff>{3}</color>");
        sb.Replace("{3}", i_drawCount.ToString());

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

        T_Shield(i_damage);

        for(int i = 0; i < i_drawCount; i++)
        {
            CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.15f);
        }

        yield return StartCoroutine(T_EndUsingCard());
    }
}
