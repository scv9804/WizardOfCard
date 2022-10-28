using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneRay : Card
{
    public static int i_usedCardCount;
	[Header("������ ������ �ʿ��� ī��"), SerializeField] int i_damagePerCard;

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

	void IncreaseCardCount()
    {
		i_usedCardCount++;

		ExplainRefresh();
	}

	void ResetCardCount(bool isMyTurn)
    {
		i_usedCardCount = 0;

		ExplainRefresh();
	}

	public override void ExplainRefresh()
    {
        base.ExplainRefresh();

		sb.Replace("{3}", i_damagePerCard.ToString());

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyManaAffinity(i_damage + (int) (i_usedCardCount / i_damagePerCard)).ToString());

		explainTMP.text = sb.ToString();
    }

    public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
    {
        base.UseCard(_target_enemy, _target_player);
    }

    public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
    {
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			T_Attack(_target_enemy, i_damage + (int) (i_usedCardCount / i_damagePerCard));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			T_Attack(_target_player, i_damage + (int) (i_usedCardCount / i_damagePerCard));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			T_Attack_AllEnemy(_target_enemy, i_damage + (int) (i_usedCardCount / i_damagePerCard));
		}

		yield return StartCoroutine(T_EndUsingCard());
	}
}
