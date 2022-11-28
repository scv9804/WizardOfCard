using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concentration : Card, IAttack
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ������"), SerializeField] int[] damage = new int[3];
	[Tooltip("�� �� ������ ������"), SerializeField] int[] damagePerTurn = new int[3];

	int bonusDamage;

	public int Damage
	{
		get { return ApplyMagicAffinity(damage[i_upgraded] + bonusDamage); }
	}

	public int DamagePerTurn
	{
		get { return damagePerTurn[i_upgraded]; }
	}

    protected override void Start()
    {
		Utility.onBattleStart += ResetDamage;
		TurnManager.onStartTurn += IncreaseDamage;
	}

	protected override void OnDisable()
	{
		Utility.onBattleStart -= ResetDamage;
		TurnManager.onStartTurn -= IncreaseDamage;
	}

	void ResetDamage()
	{
		bonusDamage = 0;
	}

	void IncreaseDamage(bool isMyTurn)
    {
		if (isMyTurn)
			bonusDamage += DamagePerTurn;
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff0000>{0}</color>");
		sb.Replace("{0}", Damage.ToString());

		sb.Replace("{1}", DamagePerTurn.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

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

		ResetDamage();

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
	}
}
