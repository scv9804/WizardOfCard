using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [Header("ī�� �߰� ������")]
    [Tooltip("���� ģȭ��"), SerializeField] int[] applyMagicAffinity_Battle = new int[3];

    #region ������Ƽ

    int I_MagicAffinity_Turn
    {
        get
        {
            return ApplyEnhanceValue(i_damage);
        }

        //set
        //{
        //    I_MagicAffinity_Turn = value;
        //}
    }

    int I_MagicAffinity_Battle
    {
        get
        {
            return ApplyEnhanceValue(applyMagicAffinity_Battle[i_upgraded]);
        }

        //set
        //{
        //    i_MagicAffinity_Battle = value;
        //}
    }

    #endregion

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
        sb.Replace("{4}", I_MagicAffinity_Battle.ToString());

        explainTMP.text = sb.ToString();
    }

    // <<22-10-28 ������ :: ����>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        Add_MagicAffinity_Turn(I_MagicAffinity_Turn);
        Add_MagicAffinity_Battle(I_MagicAffinity_Battle);

        yield return StartCoroutine(EndUsingCard());
    }
}
