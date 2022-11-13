using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //��������
	public abstract bool ShowNextPattern(Entity _entity); // �ൿ���� �����ֱ�
}
