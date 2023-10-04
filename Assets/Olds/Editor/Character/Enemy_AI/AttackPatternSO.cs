using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AttackPatternSO", menuName = "AttackPatternSO")]
public class AttackPatternSO : ScriptableObject
{
	[SerializeField] List<EnemyAttackBase> attackSO;

	public EnemyAttackBase AttackPattern(Entity entity)
	{
		entity.patternCount++;
		if (attackSO.Count <= entity.patternCount) //패턴 최대개수 넘어가면 초기화
		{
			entity.patternCount = 0;
		}

		return attackSO[entity.patternCount];
	}
}