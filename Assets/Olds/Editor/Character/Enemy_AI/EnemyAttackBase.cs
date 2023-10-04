using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;

public class EnemyAttackBase : ScriptableObject
{
    public int damage ;

	public virtual IEnumerator DefultAttack(Entity entity)
    {
        yield return EntityManager.Inst.playerEntity.Damaged(damage);
    }
}
