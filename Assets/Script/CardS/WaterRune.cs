using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRune : Card ,IRestoreHealth
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ü�� ȸ�� ��ġ"), SerializeField] int[] health = new int[3];

	public int Health
	{
		get { return ApplyEnhanceValue(health[i_upgraded]); }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
		sb.Replace("{0}", Health.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		for (int i = 0; i < Enemies.Count; i++)
        {
			RestoreHealth();

			CardManager.Inst.AddCard();

			yield return new WaitForSeconds(0.15f);
		}

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
	}

	public void RestoreHealth()
	{
		Player.Status_Health += Health;
	}
}
