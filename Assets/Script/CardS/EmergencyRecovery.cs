using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyRecovery : Card
{
	#region ������Ƽ

	int I_Heal
	{
		get
		{
			return ApplyEnhanceValue(i_damage);
		}

        //set
        //{
        //    I_Heal = value;
        //}
    }

    #endregion

    // <<22-10-28 ������ :: ����>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        RestoreHealth(I_Heal);

        if(i_upgraded == 2)
        {
            CardManager.Inst.AddCard();
        }

        yield return StartCoroutine(EndUsingCard());
    }
}
