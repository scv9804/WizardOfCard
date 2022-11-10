using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/Goblin")]
public class GoblinAttackPattern : EntityPattern 
{
	//고정패턴 기본
	public override bool Pattern(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0:
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 1:
				EntityManager.Inst.StartCoroutine(Shield(_entity));
				break;
			case 2:
				if (EntityManager.Inst.enemyEntities.Count < 3)
				{
					EntityManager.Inst.StartCoroutine(CallEnemy(_entity, 0));
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
		switch (_entity.attackTime)
		{
			case 0:
				break;
			case 2:
				break;
		}


		return true;
	}
}

