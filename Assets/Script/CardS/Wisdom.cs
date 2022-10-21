using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [Header("마나 친화성"), SerializeField] int i_applyMagicAffinity_Battle;

    public override void ExplainRefresh()
    {
        sb.Clear();
        if (b_isExile)
        {
            sb.Append("<color=#ff00ff>망각</color>\n");
        }
        sb.Append(st_explain);

        if (i_applyMagicAffinity_Battle > 0)
        {
            sb.Replace("{4}", ApplyEnhanceValue(i_applyMagicAffinity_Battle).ToString());
        }
        else
        {
            sb.Replace("마나 친화성을 <color=#ff00ff>{4}</color> 얻습니다.", "");
            sb.Replace("추가로", "");
        }

        if (i_damage != 0)
        {
            sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());
        }
        else
        {
            sb.Replace("1턴간", "");
            sb.Replace("추가로", ""); // 더 좋은 방식이 있을텐데 일단 이렇게 해둠...
            sb.Replace("마나 친화성을 <color=#ff00ff>{5}</color> 얻습니다.", ""); // 왜 유니티는 줄 바꿈 못 읽음? 다른 방법이 있나?
        }

        explainTMP.text = sb.ToString();
    }

    public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		PlayerEntity.Inst.Status_MagicAffinity_Battle += ApplyEnhanceValue(i_applyMagicAffinity_Battle);
		PlayerEntity.Inst.Status_MagicAffinity_Turn += ApplyEnhanceValue(i_damage);

        BattleCalculater.Inst.SpellEnchaneReset();
    }
}
