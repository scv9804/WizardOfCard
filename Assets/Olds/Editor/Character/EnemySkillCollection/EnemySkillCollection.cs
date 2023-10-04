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
	}

	#region 그냥 편한걸로할래... 귀찮아....
	MethodInfo methodInfo;

	public static void PrototypeFunction(string _name, params object[] _parameters)
	{
		inst.methodInfo = inst.GetType().GetMethod(_name);
		Debug.Log("적 스킬");

		inst.StartCoroutine((IEnumerator) inst.methodInfo.Invoke(inst, _parameters));
	}

	// <<22-11-22 장형용 :: 조금 더 귀찮아지는 대신 훨씬 빠름>>
	#region Advanced Callback Method

	// 해당 매개 변수마다 새로운 콜백 메소드 생성 필요 && 호출 시 코드 길이 증가
	// 근데 역시 그냥 StartCoroutine 생으로 호출하는 건 안됨?
	public static void AdvancedPrototypeFunction(Func</* Parameter Types Input Here, */IEnumerator> _routine/*, Parameters Input Here */)
	{
		inst.StartCoroutine(_routine(/* Parameters Input Here */));
	}

    #endregion

    #endregion

    #region 공통 기본
    IEnumerator AttackMotion(Entity _entity)
	{
		_entity.entitySkeletonGameObject.SetActive(false);
		_entity.charater.enabled = true;
		_entity.charater.sprite = _entity.enemy.EnemyAttackSprite;
		_entity.transform.DOMove(_entity.originPos + new Vector3(-0.15f, 0, 0), 0.1f);
		PlayerEntity.Inst.SetDamagedSprite(_entity.enemy.PlayerDamagedEffect);
		yield return new WaitForSeconds(0.15f);
		_entity.transform.DOMove(_entity.originPos, 0.2f);
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForSeconds(0.1f);
		_entity.entitySkeletonGameObject.SetActive(true);
		_entity.charater.enabled = false;
		_entity.charater.sprite = _entity.enemy.sp_sprite;
	}

	IEnumerator AttackMotion(Entity _entity, Sprite _sprite)
	{
		_entity.entitySkeletonGameObject.SetActive(false);
		_entity.charater.enabled = true;
		_entity.charater.sprite = _sprite;
		_entity.transform.DOMove(_entity.originPos + new Vector3(-0.15f, 0, 0), 0.1f);
		PlayerEntity.Inst.SetDamagedSprite(_entity.enemy.PlayerDamagedEffect);
		yield return new WaitForSeconds(0.15f);
		_entity.transform.DOMove(_entity.originPos, 0.2f);
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForSeconds(0.1f);
		_entity.entitySkeletonGameObject.SetActive(true);
		_entity.charater.enabled = false;
		_entity.charater.sprite = _entity.enemy.sp_sprite;
	}

	//기본 공격
	public IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		MusicManager.inst?.SlashSound();
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
	//부식 (배틀 데미지 감소)  0
	public IEnumerator RustAccid(Entity _entity, Sprite _sprite)
	{
		if (EntityManager.Inst.playerEntity.CompareBuffImage(0, _entity.debuffValue))
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Battle -= _entity.debuffValue;
			_entity.attackTime++;
			if (_sprite != null)
			{
				yield return StartCoroutine(AttackMotion(_entity, _sprite));
			}
			else
			{
				yield return StartCoroutine(AttackMotion(_entity));
			}
			MusicManager.inst?.SlashSound();
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // 이펙트 땜빵 ㅅㅂ 
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("부식"));
			yield return null;
		}
		else
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Battle -= _entity.debuffValue;
			
			_entity.attackTime++;
			if (_sprite != null)
			{
				yield return StartCoroutine(AttackMotion(_entity, _sprite));
			}
			else
			{
				yield return StartCoroutine(AttackMotion(_entity));
			}
			MusicManager.inst?.SlashSound();
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // 이펙트 땜빵 ㅅㅂ 
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("부식"));
			EntityManager.Inst.playerEntity.AddBuffImage(BuffDebuffManager.Inst.RustAccid, "RustAccid", 0, 1 ,1, false);
		}
	}

	//집중력 저하 (턴 데미지 감소)  1
	public IEnumerator DecreasedConcentration(Entity _entity)
	{
		if (EntityManager.Inst.playerEntity.CompareBuffImage(1, _entity.debuffValue))
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // 이펙트 땜빵 ㅅㅂ 
			MusicManager.inst?.SlashSound();
			_entity.attackTime++;
			yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("집중력 저하"));
		}
		else
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
			_entity.attackTime++;
			MusicManager.inst?.SlashSound();
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // 이펙트 땜빵 ㅅㅂ 
			yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("집중력 저하"));
			EntityManager.Inst.playerEntity.AddBuffImage(BuffDebuffManager.Inst.DecreasedConcentration, "DecreasedConcentration", 1, 1 , 0 , false);
		}
	}

	//스킬 제작 방식 : 이름으로 구분하기로 함, 
	//전투의 함성 100
	public IEnumerator WarCry(Entity _entity)
	{
		if (_entity.CompareBuffImage(0, _entity.buffValue))
		{
			MusicManager.inst?.WarCrySound();
			//yield return StartCoroutine(BuffDebuffManager.Inst.SpawnSkillEffect(_entity, "WarCry"));
			StartCoroutine(_entity.SkillNamePopup("전투의 함성"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.attackTime++;
			yield return null;
		}
		else
		{
			MusicManager.inst?.WarCrySound();
			_entity.AddBuffImage(BuffDebuffManager.Inst.WarCrySprite, "WarCry", 100, 1 , 1, true);
			//yield return StartCoroutine(BuffDebuffManager.Inst.SpawnSkillEffect(_entity, "WarCry"));
			StartCoroutine(_entity.SkillNamePopup("전투의 함성"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.attackTime++;
			yield return null;
		}
	}
	#endregion

	#region 특수공격
	//돈 사운드 추가 해야해
	public IEnumerator StealMoney(Entity _entity)
	{
		_entity.attackTime++;
		MusicManager.inst?.SlashSound();
		MusicManager.inst?.PlayBuyingSound();
		PlayerEntity.Inst.Damaged(_entity.FinalAttackValue()-1);
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
		CharacterStateStorage.Inst.money -= 5;
		_entity.attackable = false;
	}

	#endregion


}
