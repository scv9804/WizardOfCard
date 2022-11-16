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

	#region �׳� ���Ѱɷ��ҷ�... ������....
	MethodInfo methodInfo;

	Type type;

	public static void PrototypeFunction(string _name, params object[] _parameters)
	{
		inst.methodInfo = inst.type.GetMethod(_name);

		inst.StartCoroutine((IEnumerator) inst.methodInfo.Invoke(inst, _parameters));
	}

	#endregion

	#region ���� �⺻
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

	//�⺻ ����
	public IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		PlayerEntity.Inst.Damaged(_entity.FinalAttackValue());
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
		_entity.attackable = false;
	}


	//�⺻ ���� 
	public IEnumerator Shield(Entity _entity)
	{
		_entity.attackTime++;
		_entity.i_shield += _entity.increaseShield;
		_entity.attackable = false;
		yield return new WaitForSeconds(0.15f);
		_entity.RefreshEntity();
	}

	//�⺻ �� �߰���ȯ�ϱ�
	public IEnumerator CallEnemy(Entity entity, int ID)
	{
		EntityManager.Inst.SelectSpawnEnemyEntity(ID);
		entity.attackTime++;
		yield return new WaitForSeconds(0.15f);
	}


	#endregion

	#region ���� �⺻ �����
	//�ν� (��Ʋ ������ ����) 
	public IEnumerator RustAccid(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Buff_MagicAffinity_Battle -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//���߷� ���� (�� ������ ����)
	public IEnumerable DecreasedConcentration(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//��ų ���� ��� : �̸����� �����ϱ�� ��, 
	//������ �Լ�
	public IEnumerator WarCry(Entity _entity)
	{
		if (_entity.CompareBuffImage(0, 1))
		{
			yield return null;
		}
		else
		{
			yield return EntityManager.Inst.StartCoroutine(BuffDebuffSpriteManager.Inst.SpawnSkill(_entity));
			EntityManager.Inst.StartCoroutine(_entity.SkillNamePopup("������ �Լ�"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.AddBuffImage(BuffDebuffSpriteManager.Inst.WarCrySprite, "WarCry", 0, 1);
			_entity.attackTime++;
			yield return null;
		}
	}
	#endregion




}
