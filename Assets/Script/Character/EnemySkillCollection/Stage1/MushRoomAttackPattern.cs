using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/MushRoom")]
public class MushRoomAttackPattern : EntityPattern
{
	public override bool Pattern(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0:
				EnemySkillCollection.PrototypeFunction(SkillName.Shield.ToString(), _entity, _entity.specialSkillSprite[0]);
				break;
			case 1:
				EnemySkillCollection.PrototypeFunction(SkillName.Shield.ToString(), _entity);
				break;
			case 2:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				_entity.attackTime = 1;
				break;
		}

		return true;
	}

	public override bool ShowNextPattern(Entity _entity)
	{
		return true;

	}

}
