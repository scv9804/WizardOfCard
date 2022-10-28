using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [Header("���� ģȭ��"), SerializeField] int i_applyMagicAffinity_Battle;

    public override void ExplainRefresh() // �ڵ� ���� �ȵ��...
    {
        sb.Clear();
        if (b_isExile)
        {
            sb.Append("<color=#ff00ff>����</color>\n");
        }
        sb.Append(st_explain);

        if (i_damage != 0)
        {
            sb.Replace("{5}", "<color=#ff00ff>{5}</color>");
            sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());
        }
        else
        {
            sb.Replace("1�ϰ� �߰���", "");
            sb.Replace("���� ģȭ���� <color=#ff00ff>{5}</color> ����ϴ�.", "");
        }

        if (i_applyMagicAffinity_Battle > 0)
        {
            sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
            sb.Replace("{4}", ApplyEnhanceValue(i_applyMagicAffinity_Battle).ToString());
        }
        else
        {
            sb.Replace("���� ģȭ���� <color=#ff00ff>{4}</color> ����ϴ�.", "");
            sb.Replace(" �߰���", "");
        }

        explainTMP.text = sb.ToString();
    }

    public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//PlayerEntity.Inst.Status_MagicAffinity_Battle += ApplyEnhanceValue(i_applyMagicAffinity_Battle);
		//PlayerEntity.Inst.Status_MagicAffinity_Turn += ApplyEnhanceValue(i_damage);

  //      BattleCalculater.Inst.SpellEnchaneReset();
    }

    public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
    {
        yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

        BattleCalculater.Inst.SpellEnchaneReset();

        T_Apply_MagicAffinity_Turn(i_damage);
        T_Apply_MagicAffinity_Battle(i_applyMagicAffinity_Battle);

        yield return StartCoroutine(T_EndUsingCard());
    }
}
