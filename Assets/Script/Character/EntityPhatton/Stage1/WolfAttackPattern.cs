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
				EntityManager.Inst.StartCoroutine(WarCry(_entity));
				break;
			case 1:
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 2:
				EntityManager.Inst.StartCoroutine(Shield(_entity));
				break;
			case 3:
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 4:
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 5:
				//적을 부릅니다 보여주기(차징)

					break;
			case 6:
				if (EntityManager.Inst.enemyEntities.Count < 3)
				{
					EntityManager.Inst.StartCoroutine(CallEnemy(_entity, 0));
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
		

		return true;
	}

	IEnumerator WarCry(Entity _entity)
	{
		EntityManager.Inst.StartCoroutine(_entity.SkillNamePopup("전투의 함성"));
		_entity.IncreaseDamage = _entity.buffValue + _entity.i_damage ;
		_entity.attackTime++;
		yield return null;
	}

	IEnumerator SpecialAttack()
	{

		yield return new WaitForSeconds(0.15f);
	}

}
