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

	#region �׳� ���Ѱɷ��ҷ�... ������....
	MethodInfo methodInfo;

	public static void PrototypeFunction(string _name, params object[] _parameters)
	{
		inst.methodInfo = inst.GetType().GetMethod(_name);
		Debug.Log("�� ��ų");

		inst.StartCoroutine((IEnumerator) inst.methodInfo.Invoke(inst, _parameters));
	}

	// <<22-11-22 ������ :: ���� �� ���������� ��� �ξ� ����>>
	#region Advanced Callback Method

	// �ش� �Ű� �������� ���ο� �ݹ� �޼ҵ� ���� �ʿ� && ȣ�� �� �ڵ� ���� ����
	// �ٵ� ���� �׳� StartCoroutine ������ ȣ���ϴ� �� �ȵ�?
	public static void AdvancedPrototypeFunction(Func</* Parameter Types Input Here, */IEnumerator> _routine/*, Parameters Input Here */)
	{
		inst.StartCoroutine(_routine(/* Parameters Input Here */));
	}

    #endregion

    #endregion

    #region ���� �⺻
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

	//�⺻ ����
	public IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		MusicManager.inst?.SlashSound();
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
	//�ν� (��Ʋ ������ ����)  0
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
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // ����Ʈ ���� ���� 
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("�ν�"));
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
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // ����Ʈ ���� ���� 
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("�ν�"));
			EntityManager.Inst.playerEntity.AddBuffImage(BuffDebuffManager.Inst.RustAccid, "RustAccid", 0, 1 ,1, false);
		}
	}

	//���߷� ���� (�� ������ ����)  1
	public IEnumerator DecreasedConcentration(Entity _entity)
	{
		if (EntityManager.Inst.playerEntity.CompareBuffImage(1, _entity.debuffValue))
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // ����Ʈ ���� ���� 
			MusicManager.inst?.SlashSound();
			_entity.attackTime++;
			yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("���߷� ����"));
		}
		else
		{
			EntityManager.Inst.playerEntity.Buff_MagicAffinity_Turn -= _entity.debuffValue;
			_entity.attackTime++;
			MusicManager.inst?.SlashSound();
			EntityManager.Inst.playerEntity.debuffEffect.Play(); // ����Ʈ ���� ���� 
			yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
			StartCoroutine(EntityManager.Inst.playerEntity.SkillNamePopup("���߷� ����"));
			EntityManager.Inst.playerEntity.AddBuffImage(BuffDebuffManager.Inst.DecreasedConcentration, "DecreasedConcentration", 1, 1 , 0 , false);
		}
	}

	//��ų ���� ��� : �̸����� �����ϱ�� ��, 
	//������ �Լ� 100
	public IEnumerator WarCry(Entity _entity)
	{
		if (_entity.CompareBuffImage(0, _entity.buffValue))
		{
			MusicManager.inst?.WarCrySound();
			//yield return StartCoroutine(BuffDebuffManager.Inst.SpawnSkillEffect(_entity, "WarCry"));
			StartCoroutine(_entity.SkillNamePopup("������ �Լ�"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.attackTime++;
			yield return null;
		}
		else
		{
			MusicManager.inst?.WarCrySound();
			_entity.AddBuffImage(BuffDebuffManager.Inst.WarCrySprite, "WarCry", 100, 1 , 1, true);
			//yield return StartCoroutine(BuffDebuffManager.Inst.SpawnSkillEffect(_entity, "WarCry"));
			StartCoroutine(_entity.SkillNamePopup("������ �Լ�"));
			_entity.IncreaseDamage = _entity.buffValue;
			_entity.attackTime++;
			yield return null;
		}
	}
	#endregion

	#region Ư������
	//�� ���� �߰� �ؾ���
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
