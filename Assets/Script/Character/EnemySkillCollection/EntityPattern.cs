using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //��������
	public abstract bool ShowNextPattern(Entity _entity); // �ൿ���� �����ֱ�
	protected enum SkillName
	{
		[Tooltip("�ʿ��μ� : Entity")] Attack,
		[Tooltip("�ʿ��μ� : Entity")] Shield,
		[Tooltip("�ʿ��μ� : Entity , ���� ID")] CallEnemy,
		[Tooltip("�ʿ��μ� : Entity")] RustAccid,
		[Tooltip("�ʿ��μ� : Entity")] DecreasedConcentration,
		[Tooltip("�ʿ��μ� : Entity")] WarCry
	};
}
