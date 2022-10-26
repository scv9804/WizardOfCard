using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Card
{
	[Header("화상"), SerializeField] int i_applyBurning;

	public override void ExplainRefresh()
    {
		base.ExplainRefresh();

		sb.Replace("{4}", ApplyEnhanceValue(i_applyBurning).ToString());

		explainTMP.text = sb.ToString();
	}

	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			_target_enemy.i_burning += ApplyEnhanceValue(i_applyBurning);

			Attack_SingleEnemy(_target_enemy, ApplyManaAffinity(i_damage));

		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			_target_player.i_burning += ApplyEnhanceValue(i_applyBurning);

			Attack_PlayerSelf(_target_player, ApplyManaAffinity(i_damage));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
			{
				_target_enemy.i_burning += ApplyEnhanceValue(i_applyBurning);

				Attack_SingleEnemy(EntityManager.Inst.enemyEntities[i], ApplyManaAffinity(i_damage));
			}
		}

		BattleCalculater.Inst.SpellEnchaneReset();
	}
}
