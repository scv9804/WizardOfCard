using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/Slime")]
public class SlimeAttackPattern : EntityPattern
{
	//�������� �⺻
	public override bool Pattern(Entity _entity)
	{
		switch (UnityEngine.Random.Range(0, 2))
		{
			case 0: // �ν�
				EnemySkillCollection.PrototypeFunction(SkillName.RustAccid.ToString(), _entity , null);
				_entity.attackTime++;
				break;
			case 1:
				EnemySkillCollection.PrototypeFunction(SkillName.Attack.ToString(), _entity);
				break;
			case 2:
				EnemySkillCollection.PrototypeFunction(SkillName.Shield.ToString(), _entity);
				_entity.attackTime = 1;
				break;
		}
		return true;
	}

	public override bool ShowNextPattern(Entity _entity)
	{
		//���⼭ ���� ���� �ؼ� ���� �����ֱ� �� ���� ���ϼ���
		return true;
	}
}
