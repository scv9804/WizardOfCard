using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //공격패턴
	public abstract bool ShowNextPattern(Entity _entity); // 행동패턴 보여주기
}
