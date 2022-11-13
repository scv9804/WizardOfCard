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
				EnemySkillCollection.inst.StartCoroutine(EnemySkillCollection.inst.RustAccid(_entity));
				_entity.attackTime++;
				break;
			case 1:
				EnemySkillCollection.inst.StartCoroutine(EnemySkillCollection.inst.Attack(_entity));
				break;
			case 2:
				EnemySkillCollection.inst.StartCoroutine(EnemySkillCollection.inst.Shield(_entity));
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
