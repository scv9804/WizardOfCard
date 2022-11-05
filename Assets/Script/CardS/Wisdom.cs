using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [Header("���� ģȭ��"), SerializeField] int[] applyMagicAffinity_Battle = new int[3];

    #region ������Ƽ

    public int i_applyMagicAffinity_Battle
    {
        get
        {
            return applyMagicAffinity_Battle[i_upgraded];
        }

        //set
        //{
        //	applyMagicAffinity_Battle[i_upgraded] = value;
        //}
    }

    #endregion

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
        sb.Replace("{4}", ApplyEnhanceValue(i_applyMagicAffinity_Battle).ToString());

        explainTMP.text = sb.ToString();
    }

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        Add_MagicAffinity_Turn(i_damage);
        Add_MagicAffinity_Battle(i_applyMagicAffinity_Battle);

        yield return StartCoroutine(EndUsingCard());
    }
}
