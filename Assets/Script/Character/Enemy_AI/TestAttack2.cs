using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;


[CreateAssetMenu(fileName = "EnemyTest", menuName = "TestModules/EnemyAttack")]
public class TestAttack2 : TestAttack
{
    public int damage = 3;
    public override IEnumerator attackNewOne()
    {
        PlayerEntity.Inst.Status_Health -= damage;
        yield return null;


    }
}
