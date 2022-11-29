using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Card, IAttack
{
	[Header("카드 추가 가변 데이터")]
	[Tooltip("카드 데미지"), SerializeField] int[] minDamage = new int[3];
	[Tooltip("최댓값"), SerializeField] int[] maxDamage = new int[3];

	int damage = 0;

	public int Damage
	{
		get { return ApplyMagicAffinity(damage); }
	}

	int MinDamage
	{
		get { return ApplyMagicAffinity(minDamage[i_upgraded]); }
	}

	int MaxDamage
	{
		get { return ApplyMagicAffinity(maxDamage[i_upgraded]); }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff0000>{0}</color>");
		sb.Replace("{0}", MinDamage.ToString());

		sb.Replace("{1}", "<color=#ff0000>{1}</color>");
		sb.Replace("{1}", MaxDamage.ToString());

		return sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		damage = Random.Range(minDamage[i_upgraded], maxDamage[i_upgraded] + 1); // 여러 대상 간 데미지 통일

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy), ref _target_enemy);
		}

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
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
		_target?.Damaged(Damage, this);

		StartCoroutine(PlayAttackSprite);
		_target?.SetDamagedSprite(enemyDamageSprite);

		MusicManager.inst.PlayerDefultSoundEffect();
	}
}
