using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Card
{
    public override void ExplainUpdate()
    {
        sb.Clear();

        sb.Append(st_explain);
        sb.Replace("{0}", ApplyEnchantValue(i_damage).ToString());

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

            PlayerEntity.Inst.Status_Shiled += ApplyEnchantValue(i_damage);

            //PlayerEntity.Inst.Status_EnchaneValue *= 2; // 현재 강화 수치 테스트하느라 쉴드 대신 강화가 됩니당

            PlayerEntity.Inst.RefreshPlayer();
        }

        Debug.Log("보호막 사용 함수");

        targetEntity = null;
    }
}
