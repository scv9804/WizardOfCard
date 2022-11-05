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
				EntityManager.Inst.StartCoroutine(RustAccid(_entity));
				_entity.attackTime++;
				break;
			case 1:
				_entity.ShowNextActionPattern(EnemyBaseEffectManager.Inst.ShieldSprite);
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 2:
				_entity.ShowNextActionPattern(EnemyBaseEffectManager.Inst.AttackSprite);
				EntityManager.Inst.StartCoroutine(Shield(_entity));
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
