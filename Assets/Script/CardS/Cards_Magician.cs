using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards_Magician : MonoBehaviour
{
	public void CompareCard(Card _card, Entity _target)
	{
		switch (_card.i_CardNum)
		{
			case 0:
				MagicBolt(_card , _target);
				StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite ,_card.playerAttackEffectSpriteRenderer.sprite));
				StartCoroutine(_target.Damaged(_card.enemyDamagedEffectSpriteRenderer.sprite));
				break;
			case 1:
				Debug.Log("잘못된 선택입니다.");
				break;
			case 2:


				break;
			case 3:


				break;
			case 4:


				break;
			case 5:


				break;
			case 6:


				break;
			case 7:


				break;
			case 8:


				break;
			case 9:


				break;

		}

		GameManager.Inst.GameTick();
	}

	public void CompareCard(Card _card, PlayerEntity _target)
	{
		switch (_card.i_CardNum)
		{
			case 0:
				MagicBolt(_card, _target);
				break;
			case 1:
				N_TimeSpellDamage(_card);
				break;
			case 2:


				break;
			case 3:


				break;
			case 4:


				break;
			case 5:


				break;
			case 6:


				break;
			case 7:


				break;
			case 8:


				break;
			case 9:


				break;

		}


		
		GameManager.Inst.GameTick();
	}

	int CulcEnchaneDamage(Card _card)
	{
		return _card.i_damage * PlayerEntity.Inst.Status_EnchaneValue;
	}



	#region MagicBolt_0
	public void MagicBolt(Card _card, Entity _target)
	{
		_target?.Damaged(CulcEnchaneDamage(_card));
		BattleCalculater.Inst.SpellEnchaneReset();
		_target.RefreshEntity();
	}
	public void MagicBolt(Card _card, PlayerEntity _target)
	{
		_target?.Damaged(_card.i_damage);
		BattleCalculater.Inst.SpellEnchaneReset();
		_target.RefreshPlayer();
	}
	#endregion 

	public void N_TimeSpellDamage(Card _card)
	{
		PlayerEntity.Inst.Status_EnchaneValue *= _card.i_damage;
	}



}



