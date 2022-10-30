using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneRay : Card
{
    public static int i_usedCardCount;
	[Header("데미지 증가에 필요한 카드"), SerializeField] int i_damagePerCard;

	protected override void Start()
	{
		Utility.onCardUsed += IncreaseCardCount;

		TurnManager.onStartTurn += ResetCardCount;
	}

	protected void OnDisable()
    {
		Utility.onCardUsed -= IncreaseCardCount;

		TurnManager.onStartTurn -= ResetCardCount;
	}

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{3}", i_damagePerCard.ToString());

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyManaAffinity(i_damage + (int)(i_usedCardCount / i_damagePerCard)).ToString());

		explainTMP.text = sb.ToString();
	}

	void IncreaseCardCount(Card _card)
    {
		i_usedCardCount++;

		ExplainRefresh();
	}

	void ResetCardCount(bool isMyTurn)
    {
		i_usedCardCount = 0;

		ExplainRefresh();
	}

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage + (int) (i_usedCardCount / i_damagePerCard)));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, ApplyManaAffinity_Instance(i_damage + (int)(i_usedCardCount / i_damagePerCard)));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage + (int)(i_usedCardCount / i_damagePerCard))), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
