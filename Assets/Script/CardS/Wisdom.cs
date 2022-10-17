using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisdom : Card
{
    [SerializeField] int applyEverlasting = 0;

    public override void ExplainUpdate()
    {
        sb.Clear();

        sb.Append(st_explain);

        if (applyEverlasting > 0)
        {
            sb.Replace("{0}", applyEverlasting.ToString());
        }
        else
        {
            sb.Replace("마나 친화성을 {0} 얻습니다.", "");
            sb.Replace("추가로 ", "");
        }

        if (i_damage > 0)
        {
            sb.Replace("{1}", i_damage.ToString());
        }
        else
        {
            sb.Replace("1턴간 추가로 마나 친화성을 {1} 얻습니다.", "");
        }

        explainTMP.text = sb.ToString();
    }

    public override void UseCard(int index)
    {
        base.UseCard(index);

        if (index < EntityManager.Inst.enemyEntities.Count)
        {
            Debug.Log("잘못된 사용");
        }
        else
        {
            Debug.Log("나에게 사용");

            PlayerEntity.Inst.dummy_i_everlasting[1] += applyEverlasting;
            PlayerEntity.Inst.dummy_i_everlasting[2] += i_damage;

            PlayerEntity.Inst.RefreshPlayer();
        }

        Debug.Log("지혜 사용 함수");
    }
}
