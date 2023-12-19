using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycle : Card
{
	[Header("카드 추가 데이터")]
    [Tooltip("카드 드로우 횟수"), SerializeField] int[] drawCount = new int[3];

    int index;

    int DrawCount
    {
        get { return drawCount[i_upgraded]; }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", DrawCount.ToString());

        return sb.ToString();
    }

    // <<22-10-28 장형용 :: 수정>>
    // <<22-11-24 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        for (int i = 0; i < 4; i++)
        {
            if (MyCemeteryCards.Count == 0)
                break;

            index = Random.Range(0, MyCemeteryCards.Count);

            CardManager.Inst.ReplaceCardFromCemeteryToDeck(index);

            if (i < DrawCount)
                CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.1f);
        }

        CardManager.Inst.DeckShuffle();

        yield return StartCoroutine(EndUsingCard());
    }
}
