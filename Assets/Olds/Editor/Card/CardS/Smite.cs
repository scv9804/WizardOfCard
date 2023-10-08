using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smite : Card, IAttack, IShield
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ������"), SerializeField] int[] damage = new int[3];

	int shield = 0;

	public int Damage
	{
		get { return ApplyMagicAffinity(damage[i_upgraded]); }
	}

	public int Shield
	{
		get { return shield; }

		set { shield = value; }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff0000>{0}</color>");
		sb.Replace("{0}", Damage.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		shield = 0;

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy);
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy), ref _target_enemy);
		}

		GainShield();

		yield return StartCoroutine(EndUsingCard());
	}

	public void Attack(Entity _target)
	{
		if (!_target.is_die)
		{
			Shield += (int)_target?.Damaged(Damage, enemyDamageSprite, this);

			StartCoroutine(PlayAttackSprite);

			MusicManager.inst.PlayerDefultSoundEffect();
		}
	}

	public void Attack(PlayerEntity _target)
	{
		Shield += (int)_target?.Damaged(Damage, this);

		StartCoroutine(PlayAttackSprite);
		_target?.SetDamagedSprite(enemyDamageSprite);

		MusicManager.inst.PlayerDefultSoundEffect();
	}

	public void GainShield()
    {
		Player.Status_Shiled += Shield;

		MusicManager.inst.PlayBarrierSound();
	}
}