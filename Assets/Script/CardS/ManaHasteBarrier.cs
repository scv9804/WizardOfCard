using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHasteBarrier : Card
{
    [Header("카드 추가 데이터")]
    [Tooltip("카드 드로우 횟수"), SerializeField] int[] drawCount = new int[3];

    #region 프로퍼티

    int I_DrawCount
    {
        get
        {
            return drawCount[i_upgraded];
        }

        //set
        //{
        //	drawCount[i_upgraded] = value;
        //}
    }

    int I_Shield
    {
        get
        {
            return ApplyMagicResistance(i_damage);
        }

        //set
        //{
        //    I_Shield = value;
        //}
    }

    #endregion

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{3}", I_DrawCount.ToString());

        explainTMP.text = sb.ToString();
    }


    // <<22-10-28 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        Shield(I_Shield);

        for(int i = 0; i < I_DrawCount; i++)
        {
            CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.15f);
        }

        yield return StartCoroutine(EndUsingCard());
    }
}
