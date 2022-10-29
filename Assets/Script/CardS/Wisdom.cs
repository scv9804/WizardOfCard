using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [Header("마나 친화성"), SerializeField] int i_applyMagicAffinity_Battle;

    public override void ExplainRefresh() // 코드 맘에 안들어...
    {
        sb.Clear();
        if (b_isExile)
        {
            sb.Append("<color=#ff00ff>망각</color>\n");
        }
        sb.Append(st_explain);

        if (i_damage != 0)
        {
            sb.Replace("{5}", "<color=#ff00ff>{5}</color>");
            sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());
        }
        else
        {
            sb.Replace("1턴간 추가로", "");
            sb.Replace("마나 친화성을 <color=#ff00ff>{5}</color> 얻습니다.", "");
        }

        if (i_applyMagicAffinity_Battle > 0)
        {
            sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
            sb.Replace("{4}", ApplyEnhanceValue(i_applyMagicAffinity_Battle).ToString());
        }
        else
        {
            sb.Replace("마나 친화성을 <color=#ff00ff>{4}</color> 얻습니다.", "");
            sb.Replace(" 추가로", "");
        }

        explainTMP.text = sb.ToString();
    }

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        BattleCalculater.Inst.SpellEnchaneReset();

        Add_MagicAffinity_Turn(i_damage);
        Add_MagicAffinity_Battle(i_applyMagicAffinity_Battle);

        yield return StartCoroutine(EndUsingCard());
    }
}
