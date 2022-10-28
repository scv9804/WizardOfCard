using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//if (_target_enemy != null && _target_player == null) // ���� ���
		//{
		//	Attack_SingleEnemy(_target_enemy, ApplyManaAffinity(i_damage));
		//}
		//else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		//{
		//	Attack_PlayerSelf(_target_player, ApplyManaAffinity(i_damage));
		//}
		//else // ���� �Ǵ� ������ ��� (?)
		//{
		//	Attack_AllEnemy(ApplyManaAffinity(i_damage));
  //      }

		//BattleCalculater.Inst.SpellEnchaneReset();
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			T_Attack(_target_enemy, i_damage);
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			T_Attack(_target_player, i_damage);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			T_Attack_AllEnemy(_target_enemy, i_damage);
		}

		yield return StartCoroutine(T_EndUsingCard());
	}
}
