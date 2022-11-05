using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EntityPattern : ScriptableObject
{
	public abstract bool Pattern(Entity _entity); //��������
	public abstract bool ShowNextPattern(Entity _entity); // �ൿ���� �����ֱ�

	protected int patternNum = 0;

	//�⺻ ����
	//��ƼƼ ������ �����ϱ�
	//��� �� �� StartCoroutine�� EntityManager���� ���. (��ũ���ͺ� ������Ʈ�� ¿��)
	//Scriptable ��� ����. �⺻ ���� © �� Sort�Ἥ �ٲٷ��� �� �ٲ���� ������


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
	public virtual IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		PlayerEntity.Inst.Damaged(_entity.enemy.i_damage);
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
		_entity.attackable = false;
	}

	//�⺻ �� �߰���ȯ�ϱ�
	public IEnumerator CallEnemy(Entity entity)
	{
		EntityManager.Inst.SelectSpawnEnemyEntity(0);
		entity.attackTime++;
		yield return new WaitForSeconds(0.15f);
	}

	//�⺻ ���� 
	public virtual IEnumerator Shield(Entity _entity)
	{
		_entity.attackTime++;
		_entity.i_shield += _entity.increaseShield;
		_entity.attackable = false;
		yield return new WaitForSeconds(0.15f);
		_entity.RefreshEntity();
	}

	#endregion

	#region ���� �⺻ �����
	//�ν� (��Ʋ ������ ����) 
	public virtual IEnumerator RustAccid(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Status_MagicAffinity_Battle -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	//���߷� ���� (�� ������ ����)
	public virtual IEnumerable DecreasedConcentration(Entity _entity)
	{
		EntityManager.Inst.playerEntity.Status_MagicAffinity_Turn -= _entity.debuffValue;
		_entity.attackTime++;
		yield return (EntityManager.Inst.StartCoroutine(AttackMotion(_entity)));
	}

	#endregion 

}
