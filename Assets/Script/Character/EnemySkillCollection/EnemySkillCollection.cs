using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using System;

public class EnemySkillCollection : MonoBehaviour
{
	public static EnemySkillCollection inst;

	private void Awake()
	{
		inst = this;

		type = inst.GetType();
	}

	#region 그냥 편한걸로할래... 귀찮아....
	MethodInfo methodInfo;

	Type type;

	public static void PrototypeFunction(string _name, params object[] _parameters)
	{
		inst.methodInfo = inst.type.GetMethod(_name);

		inst.StartCoroutine((IEnumerator) inst.methodInfo.Invoke(inst, _parameters));
	}

	#endregion

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
	public IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		PlayerEntity.Inst.Damaged(_entity.FinalAttackValue());
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
		_entity.attackable = false;
	}


	//기본 쉴드 
	public IEnumerator Shield(Entity _entity)
	{
		_entity.attackTime++;
		_entity.i_shield += _entity.increaseShield;
		_entity.attackable = false;
		yield return new WaitForSeconds(0.15f);
		_entity.RefreshEntity();
	}

	//기본 적 추가소환하기
	public IEnumerator CallEnemy(Entity entity, int ID)
	{
		EntityManager.Inst.SelectSpawnEnemyEntity(ID);
		entity.attackTime++;
		yield return new WaitForSeconds(0.15f);
	}


	#endregion

	#region 공통 기본 디버프
	//부식 (배틀 데미지 감소) 
	public IEnumerator RustAccid(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Buff_MagicAffinity_Battle -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//집중력 저하 (턴 데미지 감소)
	public IEnumerable DecreasedConcentration(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//스킬 제작 방식 : 이름으로 구분하기로 함, 
	//전투의 함성
	public IEnumerator WarCry(Entity _entity)
	{
		if (_entity.CompareBuffImage(0, 1))
		{
			yield return null;
		}
		else
		{
			yield return EntityManager.Inst.StartCoroutine(BuffDebuffSpriteManager.Inst.SpawnSkill(_entity));
			EntityManager.Inst.StartCoroutine(_entity.SkillNamePopup("전투의 함성"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.AddBuffImage(BuffDebuffSpriteManager.Inst.WarCrySprite, "WarCry", 0, 1);
			_entity.attackTime++;
			yield return null;
		}
	}
	#endregion




}
