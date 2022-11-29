using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/ThiefAttackPattern")]
public class ThiefAttackPattern : EntityPattern
{
	public override bool Pattern(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0:
				EnemySkillCollection.PrototypeFunction(SkillName.DecreasedConcentration.ToString(), _entity);
				//EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				break;
			case 1:
				EnemySkillCollection.PrototypeFunction(SkillName.DecreasedConcentration.ToString(), _entity);
				break;
			case 2:
				EnemySkillCollection.PrototypeFunction(SkillName.StealMoney.ToString(), _entity);
				break;
			case 3:
				EnemySkillCollection.PrototypeFunction(SkillName.StealMoney.ToString(), _entity);
				break;
			case 4:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				_entity.attackTime = 0;
				break;
		}

		return true;
	}

	public override bool ShowNextPattern(Entity _entity)
	{
		return true;
	}


}
