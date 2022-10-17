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
            Debug.Log("�߸��� ���");
        }
        else
        {
            Debug.Log("������ ���");

            PlayerEntity.Inst.Status_Shiled += ApplyEnchantValue(i_damage);

            //PlayerEntity.Inst.Status_EnchaneValue *= 2; // ���� ��ȭ ��ġ �׽�Ʈ�ϴ��� ���� ��� ��ȭ�� �˴ϴ�

            PlayerEntity.Inst.RefreshPlayer();
        }

        Debug.Log("��ȣ�� ��� �Լ�");

        targetEntity = null;
    }
}
