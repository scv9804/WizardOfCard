using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calm : Card, IManaAffinity_Battle
{
	[Header("카드 추가 데이터")]
	[Tooltip("카드 전투 지속 마나 친화성 부여 수치"), SerializeField] int[] manaAffinity_battle = new int[3];
	[Tooltip("추가 마나 친화성 증가량"), SerializeField] int[] extraManaAffinity_battle = new int[3];

    public int ManaAffinity_Battle
	{
		get { return ApplyEnhanceValue(manaAffinity_battle[i_upgraded]); }
	}

	public int ExtraManaAffinity_Battle
	{
		get { return ApplyEnhanceValue(extraManaAffinity_battle[i_upgraded]); }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
		sb.Replace("{0}", ManaAffinity_Battle.ToString());

		sb.Replace("{1}", "<color=#ff00ff>{1}</color>");
		sb.Replace("{1}", (ManaAffinity_Battle + ExtraManaAffinity_Battle).ToString());

		return sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		GainManaAffinity_Battle();

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
	}

	public void GainManaAffinity_Battle()
	{
		Player.Buff_MagicAffinity_Battle += ManaAffinity_Battle;

		for (int i = 0; i < MyHandCards.Count; i++)
		{
			if (MyHandCards[i].i_manaCost >= 3)
            {
				Player.Buff_MagicAffinity_Battle += ExtraManaAffinity_Battle;

				break;
			}
		}
	}
}
