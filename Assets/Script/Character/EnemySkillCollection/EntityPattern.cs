using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //공격패턴
	public abstract bool ShowNextPattern(Entity _entity); // 행동패턴 보여주기
	protected enum SkillName
	{
		[Tooltip("필요인수 : Entity")] Attack,
		[Tooltip("필요인수 : Entity")] Shield,
		[Tooltip("필요인수 : Entity , 몬스터 ID")] CallEnemy,
		[Tooltip("필요인수 : Entity")] RustAccid,
		[Tooltip("필요인수 : Entity")] DecreasedConcentration,
		[Tooltip("필요인수 : Entity")] WarCry
	};
}
