using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableTome : Card
{
    public override void ExplainUpdate()
    {
        sb.Clear();

        sb.Append(st_explain);
        sb.Replace("{0}", i_damage.ToString());

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

            for(int i = 0; i < i_damage; i++)
            {
                if(CardManager.Inst.myDeck.Count > 0)
                {
                    CardManager.Inst.AddCard();
                    CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].card_info.i_Cost = 0; // start �Լ������� i_manaCost�� �ϸ� �� �ٲ� ������������������������������
                    CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].b_isExile = true;

                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1]);
                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].i_manaCost);
                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].b_isExile);
                }
            }
        }

        Debug.Log("�Ҿ����� �� ��� �Լ�");

        targetEntity = null;
    }
}
