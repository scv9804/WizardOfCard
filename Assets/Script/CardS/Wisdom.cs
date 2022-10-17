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
            sb.Replace("���� ģȭ���� {0} ����ϴ�.", "");
            sb.Replace("�߰��� ", "");
        }

        if (i_damage > 0)
        {
            sb.Replace("{1}", i_damage.ToString());
        }
        else
        {
            sb.Replace("1�ϰ� �߰��� ���� ģȭ���� {1} ����ϴ�.", "");
        }

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

            PlayerEntity.Inst.dummy_i_everlasting[1] += applyEverlasting;
            PlayerEntity.Inst.dummy_i_everlasting[2] += i_damage;

            PlayerEntity.Inst.RefreshPlayer();
        }

        Debug.Log("���� ��� �Լ�");
    }
}
