using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "EnemyAttackPattern/Stage1/Goblin")]
public class GoblinAttackPattern : EntityPattern 
{
	public Sprite sp;

	public override bool ExcuteRole(Entity _entity)
	{
		switch (_entity.attackTime)
		{
			case 0:
				_entity.attackTime++;
				break;
			case 1:
				EntityManager.Inst.StartCoroutine(Attack(_entity));
				break;
			case 2:
				EntityManager.Inst.StartCoroutine(Shield(_entity));
				_entity.attackTime = 0;
				break;

		}
		return true;
	}

	// DefultShield
	public IEnumerator Shield(Entity _entity)
	{
		_entity.attackTime++;
		_entity.i_shield++;
		yield return new WaitForSeconds(0.15f);
		_entity.RefreshEntity();
	}

	// DefultAttack
	public IEnumerator Attack(Entity _entity)
	{
		_entity.attackTime++;
		PlayerEntity.Inst.Damaged(_entity.enemy.i_damage);
		_entity.charater.sprite = _entity.enemy.EnemyAttackSprite;
		_entity.transform.DOMove(_entity.originPos + new Vector3(-0.15f, 0, 0), 0.1f);
		PlayerEntity.Inst.SetDamagedSprite(_entity.enemy.PlayerDamagedEffect);
		yield return new WaitForSeconds(0.15f);
		_entity.transform.DOMove(_entity.originPos, 0.2f);
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForSeconds(0.1f);
		_entity.charater.sprite = _entity.enemy.sp_sprite;
	}
}
