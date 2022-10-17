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
            Debug.Log("잘못된 사용");
        }
        else
        {
            Debug.Log("나에게 사용");

            for(int i = 0; i < i_damage; i++)
            {
                if(CardManager.Inst.myDeck.Count > 0)
                {
                    CardManager.Inst.AddCard();
                    CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].card_info.i_Cost = 0; // start 함수때문에 i_manaCost로 하면 안 바뀜 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ
                    CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].b_isExile = true;

                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1]);
                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].i_manaCost);
                    Debug.Log(CardManager.Inst.myCards[CardManager.Inst.myCards.Count - 1].b_isExile);
                }
            }
        }

        Debug.Log("불안정한 고서 사용 함수");

        targetEntity = null;
    }
}
