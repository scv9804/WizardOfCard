using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStrom : Card, IBurning, IAttack
{
	[Header("카드 추가 가변 데이터")]
	[Tooltip("카드 화상 부여 수치"), SerializeField] int[] burning = new int[3];
	[Tooltip("카드 데미지"), SerializeField] int[] damage = new int[3];

	public int Burning
	{
		get { return ApplyEnhanceValue(burning[i_upgraded]); }
	}

	public int Damage
	{
		get { return ApplyMagicAffinity(damage[i_upgraded]); }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
		sb.Replace("{0}", Burning.ToString());

		sb.Replace("{1}", "<color=#ff0000>{1}</color>");
		sb.Replace("{1}", Damage.ToString());

		return sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        TargetAll(() => AddBurning(_target_enemy), ref _target_enemy);
        TargetAll(() => Attack(_target_enemy), ref _target_enemy);

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
		Debug.LogError("잘못된 접근입니다.");
	}

	public void Attack(Entity _target)
	{
		if (!_target.is_die)
		{
			_target?.Damaged(Damage, enemyDamageSprite, this);

			StartCoroutine(PlayAttackSprite);

			MusicManager.inst.PlayerDefultSoundEffect();
		}
	}

	public void Attack(PlayerEntity _target)
	{
		Debug.LogError("잘못된 접근입니다.");
	}
}
