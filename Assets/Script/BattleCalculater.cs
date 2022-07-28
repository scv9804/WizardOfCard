using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  BattleCalculater: MonoBehaviour
{
    public static BattleCalculater Inst { get; private set; }
    private void Awake()
    {
        Inst = this;

        DontDestroyOnLoad(this);
    }

    int Defult;

    public void BattleCalc(Card _card, PlayerEntity _target)
	{
        _target.Status_Aether -= _card.i_manaCost;
        if (_target.Status_Aether <= 0)
        {
            TurnManager.Inst.EndTurn();
        }
        switch (CardManager.Inst.selectCard.i_cardType)
        {
            case 0: //Attack
                {
                    PlayerSpellDamagedCalc(_card, _target);
                    break;
                }
            case 1: //Spell_Enhance
                {
                    SpellEnhaneCalc(_card);
                    break;
                }
            case 2: // Shiled
                {
                    PlayerShieldCalc(_card, _target);
                    break;
                }
            case 3: //Heal
                {
                    PlayerHealCalc(_card, _target);
                    break;
                }
            case 4:  //Buff
                {
                    PlayerBuffCalc(_card);
                    break;
                }
            case 5: //Debuff
                {
                    PlayerDebuffCalc(_card);
                    break;
                }
        }

    }

    public void BattleCalc(Card _card, Entity _target)
    {
        EntityManager.Inst.playerEntity.Status_Aether -= _card.i_manaCost;
        if (EntityManager.Inst.playerEntity.Status_Aether <= 0)
        {
            TurnManager.Inst.EndTurn();
        }
        switch (CardManager.Inst.selectCard.i_cardType)
        {
            case 0: //Spell
                {
                    EnemySpellDamagedCalc(_card, _target);
                    GameManager.Inst.GameTick();
                    break;
                }
            case 1: //Spell_Enhance
                {
                    SpellEnhaneCalc(_card);
                    break;
                }
            case 2: // Shiled
                {
                    EnemyShieldCalc(_card, _target);
                    break;
                }
            case 3: //Heal
                {
                    EnemyHealCalc(_card, _target);
                    break;
                }
            case 4:  //Buff
                {
                    EnemyBuffCalc(_card);
                    break;
                }
            case 5: //Debuff
                {
                    EnemyDebuffCalc(_card);
                    break;
                }
        }

    }


    // 강화중인가?
    private void SpellEnchaneReset()
    {
        PlayerEntity.Inst.Status_EnchaneValue *= 1;
    }
    public void SpellEnhaneCalc(Card _card)
    {
        PlayerEntity.Inst.Status_EnchaneValue *= _card.i_damage;
    }



	#region EnemyTarget

	//기본 주문 데미지
	public void EnemySpellDamagedCalc(Card _card, Entity _target)
    {
        _target?.Damaged(_card.i_damage);
        SpellEnchaneReset();
    }

    //주문 강화


    // 쉴드
    public void EnemyShieldCalc(Card _card, Entity _entity)
    {
        _entity.i_shield += _card.i_damage;
        _entity.RefreshEntity();
        SpellEnchaneReset();
    }

    
    //힐
    public void EnemyHealCalc(Card _card, Entity _entity)
    {
        if (_entity.i_health + _card.i_damage <= _entity.HEALTHMAX)
        {
            _entity.i_health += _card.i_damage;
            _entity.RefreshEntity();
            SpellEnchaneReset();
        }
        else
        {
            _entity.i_health = _entity.HEALTHMAX;
            _entity.RefreshEntity();
            SpellEnchaneReset();
        }
    }

    //버프
    public void EnemyBuffCalc(Card _card)
    {

    }

    //디버프
    public void EnemyDebuffCalc(Card _card)
    {

    }


    #endregion


    #region PlayerTarget

    public void PlayerSpellDamagedCalc(Card _card, PlayerEntity _target)
    {
        _target?.Damaged(_card.i_damage);
        SpellEnchaneReset();
    }

    //주문 강화


    // 쉴드
    public void PlayerShieldCalc(Card _card, PlayerEntity _playerEntity)
    {
        _playerEntity.Status_Shiled += _card.i_damage;
        _playerEntity.RefreshPlayer();
        SpellEnchaneReset();
    }


    //힐
    public void PlayerHealCalc(Card _card, PlayerEntity _playerEntity)
    {
        if (_playerEntity.Status_Health + _card.i_damage <= _playerEntity.Status_MaxHealth)
        {
            _playerEntity.Status_Health += _card.i_damage;
            _playerEntity.RefreshPlayer();
            SpellEnchaneReset();
        }
        else
        {
            _playerEntity.Status_Health = _playerEntity.Status_MaxHealth;
            _playerEntity.RefreshPlayer();
            SpellEnchaneReset();
        }
    }

    //버프
    public void PlayerBuffCalc(Card _card)
    {

    }

    //디버프
    public void PlayerDebuffCalc(Card _card)
    {

    }

	#endregion

}
