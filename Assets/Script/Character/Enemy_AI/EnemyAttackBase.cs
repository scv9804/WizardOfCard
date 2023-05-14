using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;

public class EnemyAttackBase : ScriptableObject
{
    public int damage = 3;
    public virtual IEnumerator DefultAttack()
    {
        PlayerEntity.Inst.Status_Health -= damage;
        yield return null;
    }

    protected virtual IEnumerator AttackAnime()
	{
        yield return null;
	}
}
