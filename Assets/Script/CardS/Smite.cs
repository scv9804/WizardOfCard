using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smite : Card
{
	#region ������Ƽ

	int I_Damage
	{
		get
		{
			return ApplyMagicAffinity(i_damage);
		}

		//set
		//{
		//    I_Damage = value;
		//}
	}

    #endregion

    protected override void Start()
    {
		Utility.onDamaged -= Smite_Shield;
		Utility.onDamaged += Smite_Shield;
	}

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return base.UseCard(_target_enemy, _target_player);

		PlayerEntity.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, I_Damage);
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, I_Damage);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, I_Damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}

	void Smite_Shield(Card card, int damage)
    {
		if (card == this)
			Shield(damage);
    }
}
