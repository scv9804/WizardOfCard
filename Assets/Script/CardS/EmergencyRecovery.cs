using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyRecovery : Card
{
    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        explainTMP.text = sb.ToString();
    }

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        RestoreHealth(ApplyEnhanceValue_Instance(i_damage));

        if(i_upgraded == 2)
        {
            CardManager.Inst.AddCard();
        }

        yield return StartCoroutine(EndUsingCard());
    }
}
