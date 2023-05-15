using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;

public class EnemyAttackBase : ScriptableObject
{
    public int damage ;

	public virtual IEnumerator DefultAttack(Entity entity)
    {
		damage = entity.FinalAttackValue();
       
        PlayerEntity.Inst.Status_Health -= damage;
        yield return null;
    }
}
