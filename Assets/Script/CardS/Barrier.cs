using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Card
{
    #region Properties

    int I_Shield
    {
        get
        {
            return ApplyMagicResistance(i_damage);
        }

        //set
        //{
        //    I_Shield = value;
        //}
    }

    #endregion

    // <<22-10-28 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.ResetEnhanceValue();

        Shield(I_Shield);

		yield return StartCoroutine(EndUsingCard());
	}
}
