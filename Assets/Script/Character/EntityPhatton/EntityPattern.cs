using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //공격패턴
	public abstract bool ShowNextPattern(Entity _entity); // 행동패턴 보여주기

	protected int patternNum = 0;

	//기본 사용법
	//엔티티 값으로 수정하기
	//사용 할 때 StartCoroutine을 EntityManager에서 사용. (스크립터블 오브젝트라 쩔수)
	//Scriptable 사용 이유. 기본 골자 짤 때 Sort써서 바꾸려면 다 바꿔야함 귀찮음


	#region 공통 기본
	IEnumerator AttackMotion(Entity _entity)
	{
		_entity.charater.sprite = _entity.enemy.EnemyAttackSprite;
		_entity.transform.DOMove(_entity.originPos + new Vector3(-0.15f, 0, 0), 0.1f);
		PlayerEntity.Inst.SetDamagedSprite(_entity.enemy.PlayerDamagedEffect);
		yield return new WaitForSeconds(0.15f);
		_entity.transform.DOMove(_entity.originPos, 0.2f);
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForSeconds(0.1f);
		_entity.charater.sprite = _entity.enemy.sp_sprite;
	}
	//기본 공격
	public virtual IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		PlayerEntity.Inst.Damaged(_entity.enemy.i_damage);
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
		_entity.attackable = false;
	}

	//기본 적 추가소환하기
	public IEnumerator CallEnemy(Entity entity)
	{
		EntityManager.Inst.SelectSpawnEnemyEntity(0);
		entity.attackTime++;
		yield return new WaitForSeconds(0.15f);
	}

	//기본 쉴드 
	public virtual IEnumerator Shield(Entity _entity)
	{
		_entity.attackTime++;
		_entity.i_shield += _entity.increaseShield;
		_entity.attackable = false;
		yield return new WaitForSeconds(0.15f);
		_entity.RefreshEntity();
	}

	#endregion

	#region 공통 기본 디버프
	//부식 (배틀 데미지 감소) 
	public virtual IEnumerator RustAccid(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Status_MagicAffinity_Battle -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//집중력 저하 (턴 데미지 감소)
	public virtual IEnumerable DecreasedConcentration(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Status_MagicAffinity_Turn -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	#endregion 

}
