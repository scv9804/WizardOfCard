using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySkillTest", menuName = "TestModules/EnemyAttack")]
public class SkillTest : EnemyAttackBase
{
	public override IEnumerator DefultAttack(Entity entity)
	{
		return base.DefultAttack(entity);
	}

}
