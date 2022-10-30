using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHasteBarrier : Card
{
    [Header("카드 드로우"), SerializeField] int i_drawCount;

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{3}", i_drawCount.ToString());

        explainTMP.text = sb.ToString();
    }

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        BattleCalculater.Inst.SpellEnchaneReset();

        Shield(ApplyMagicResistance_Instance(i_damage));

        for(int i = 0; i < i_drawCount; i++)
        {
            CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.15f);
        }

        yield return StartCoroutine(EndUsingCard());
    }
}
