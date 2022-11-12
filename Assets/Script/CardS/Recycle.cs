using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycle : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("카드 드로우 횟수"), SerializeField] int[] drawCount = new int[3];

    #region Properties

    int I_DrawCount
    {
        get
        {
            return drawCount[i_upgraded];
        }

        //set
        //{
        //	drawCount[i_upgraded] = value;
        //}
    }

    int I_RecycleCount
    {
        get
        {
            return i_damage;
        }

        //set
        //{
        //	drawCount[i_upgraded] = value;
        //}
    }

    #endregion

    public override void ExplainRefresh()
    {
        base.ExplainRefresh();

        sb.Replace("{3}", I_DrawCount.ToString());

        explainTMP.text = sb.ToString();
    }

    // <<22-10-28 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        int random;

        for (int i = 0; i < I_RecycleCount; i++)
        {
            random = UnityEngine.Random.Range(0, CardManager.Inst.myCemetery.Count);

            CardManager.Inst.ReplaceCardFromCemeteryToDeck(random);

            if (i < I_DrawCount)
                CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.1f);
        }

        CardManager.Inst.DeckShuffle();

        yield return StartCoroutine(EndUsingCard());
	}
}
