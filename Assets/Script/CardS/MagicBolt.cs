using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack_SingleEnemy(_target_enemy, ApplyManaAffinity(i_damage));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack_PlayerSelf(_target_player, ApplyManaAffinity(i_damage));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			Attack_AllEnemy(ApplyManaAffinity(i_damage));
        }

		BattleCalculater.Inst.SpellEnchaneReset();
	}
}
