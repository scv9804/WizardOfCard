using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBreath : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("���� ȸ����"), SerializeField] int[] restoreAether = new int[3];

	#region Properties

	int I_Aether
	{
		get
		{
			return restoreAether[i_upgraded];
		}

		//set
		//{
		//    I_RestoreAether = value;
		//}
	}

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

		PlayerEntity.Inst.ResetEnhanceValue();

		RestoreHealth(I_Heal); 
		RestoreAether(I_Aether);

		yield return StartCoroutine(EndUsingCard());
	}
}
