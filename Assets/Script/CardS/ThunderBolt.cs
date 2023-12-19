using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Card, IAttack
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ������"), SerializeField] int[] minDamage = new int[3];
	[Tooltip("�ִ�"), SerializeField] int[] maxDamage = new int[3];

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

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		damage = Random.Range(minDamage[i_upgraded], maxDamage[i_upgraded] + 1); // ���� ��� �� ������ ����

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
		_target?.Damaged(Damage, this);

		StartCoroutine(PlayAttackSprite);
		_target?.SetDamagedSprite(enemyDamageSprite);

		MusicManager.inst.PlayerDefultSoundEffect();
	}
}