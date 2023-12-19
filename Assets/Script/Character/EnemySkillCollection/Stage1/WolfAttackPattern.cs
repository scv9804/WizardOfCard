using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/Boss_wolf")]
public class WolfAttackPattern : EntityPattern
{
	public override bool Pattern(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0: // 전투의 포효
                EnemySkillCollection.PrototypeFunction(SkillName.WarCry.ToString(), _entity);
				break;
			case 1:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				break;
			case 2:
				EnemySkillCollection.PrototypeFunction(SkillName.Shield.ToString(), _entity);
				break;
			case 3:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				break;
			case 4:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				break;
			case 5:
				//적을 부릅니다 보여주기(차징)

					break;
			case 6:
				if (EntityManager.Inst.enemyEntities.Count < 3)
				{
					EnemySkillCollection.PrototypeFunction(SkillName.CallEnemy.ToString(), _entity, 0);
					EnemySkillCollection.PrototypeFunction(SkillName.CallEnemy.ToString(), _entity, 0);
					_entity.attackTime = 0;
					break;
				}
				else
				{
					_entity.attackTime = 0;
					Pattern(_entity);
					break;
				}
		}
		return true;
	}

	public override bool ShowNextPattern(Entity _entity)
	{
		return true;
	}

	IEnumerator SpecialAttack()
	{
		yield return new WaitForSeconds(0.15f);
	}

}
