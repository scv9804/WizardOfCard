using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : Card, IBurning
{
	[Header("카드 추가 가변 데이터")]
	[Tooltip("카드 화상 부여 수치"), SerializeField] int[] burning = new int[3];

	public int Burning
	{
		get { return ApplyEnhanceValue(burning[i_upgraded]); }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
		sb.Replace("{0}", Burning.ToString());

		return sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			AddBurning(_target_enemy);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			AddBurning(_target_player);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => AddBurning(_target_enemy), ref _target_enemy);
		}

		if(i_upgraded == 2)
			CardManager.Inst.AddCard();

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
	}

	public void AddBurning(Entity _target)
	{
		_target.i_burning += Burning;
	}

	public void AddBurning(PlayerEntity _target)
	{
		_target.Debuff_Burning += Burning;
	}
}
