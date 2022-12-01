using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormRune : Card, IAttack
{
	[Header("카드 추가 가변 데이터")]
	[Tooltip("카드 데미지"), SerializeField] int[] damage = new int[3];
	[Tooltip("공격 횟수"), SerializeField] int[] attackCount = new int[3];

	public int Damage
	{
		get { return ApplyMagicAffinity(damage[i_upgraded]); }
	}

	public int AttackCount
	{
		get { return attackCount[i_upgraded]; }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff0000>{0}</color>");
		sb.Replace("{0}", Damage.ToString());

		sb.Replace("{1}", (AttackCount - 1).ToString());

		return sb.ToString();
    }

    // <<22-10-28 장형용 :: 수정>>
    // <<22-11-24 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		for (int i = 0; i < AttackCount; i++)
		{
			_target_enemy = RandomTarget;

			if (_target_enemy != null)
				Attack(_target_enemy);

			yield return new WaitForSeconds(0.1f);
		}

		yield return StartCoroutine(EndUsingCard());
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
