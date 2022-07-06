using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	public int i_attackCount = 1;
	

	public void Attack()
	{
		if (TurnManager.Inst.myTurn == true)
		{
			i_attackCount -= 1;
			EntityManager.Inst.playerEntity.i_health -= 1;
		}
	}


}
