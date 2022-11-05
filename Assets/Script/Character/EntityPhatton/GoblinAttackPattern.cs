using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/Goblin")]
public class GoblinAttackPattern : EntityPattern 
{
	public override bool Pattern(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0:
				_entity.ShowNextActionPattern(EnemyBaseEffectManager.Inst.AttackSprite);
				_entity.attackTime++;
				break;
			case 1:
				_entity.ShowNextActionPattern(EnemyBaseEffectManager.Inst.ShieldSprite);
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 2:
				_entity.ShowNextActionPattern(EnemyBaseEffectManager.Inst.AttackSprite);
				EntityManager.Inst.StartCoroutine(Shield(_entity));
				break;
			case 3:
				if (EntityManager.Inst.enemyEntities.Count < 3)
				{
					EntityManager.Inst.StartCoroutine(GoblinCall(_entity));
					_entity.attackTime = 1;
					break;
				}
				else
				{
					_entity.attackTime = 1;
					Pattern(_entity);
					break;
				}
		}

		return true;
	}

	public IEnumerator GoblinCall(Entity entity)
	{
		EntityManager.Inst.SelectSpawnEnemyEntity(1);
		entity.attackTime++;
		Debug.Log("1Â÷ ½Ãµµ");
		yield return new WaitForSeconds(0.15f);
	}
}

