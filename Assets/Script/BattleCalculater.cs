using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  BattleCalculater: MonoBehaviour
{
    public static BattleCalculater Inst { get; private set; }
    private void Awake()
    {
        Inst = this;

        //DontDestroyOnLoad(this);
    }

    int Defult;

   // public enum e_CardType { Spell, Spell_Enhance, Shlied, Heal, Buff, Debuff };


    [HideInInspector] public bool is_canUseSelf;
    [HideInInspector] public int i_enhacneVal=1;
    [HideInInspector] public int i_calcDamage;
    [HideInInspector] public int i_everlasting = 0;  //고정 마법 증폭 (수정여지있음)

    public void BattleCalc(Card _card, PlayerEntity _target)
	{
        _target.i_manaCost -= _card.i_manaCost;
        if (_target.i_manaCost <= 0)
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
                    SpellEnhanceDamagedCalc(_card);
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
        EntityManager.Inst.playerEntity.i_manaCost -= _card.i_manaCost;
        if (EntityManager.Inst.playerEntity.i_manaCost <= 0)
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
                    SpellEnhanceDamagedCalc(_card);
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
    private void IsUseEnchance()
    {
        if (CardManager.Inst.is_useEnhance == true)
        {
            i_enhacneVal = 1 + i_everlasting;
            CardManager.Inst.AfterUseEnhance();
        }
        CardManager.Inst.is_useEnhance = false;
    }
    public void SpellEnhanceDamagedCalc(Card _card)
    {
        i_enhacneVal *= _card.i_explainDamageOrigin;
        CardManager.Inst.UseEnhanceRefresh(i_enhacneVal);
        CardManager.Inst.is_useEnhance = true;
    }



	#region EnemyTarget

	//기본 주문 데미지
	public void EnemySpellDamagedCalc(Card _card, Entity _target)
    {
        _target?.Damaged(_card.i_damage);
        IsUseEnchance();
    }

    //주문 강화


    // 쉴드
    public void EnemyShieldCalc(Card _card, Entity _entity)
    {
        _entity.i_shield += _card.i_damage;
        _entity.RefreshEntity();
        IsUseEnchance();
    }


    //힐
    public void EnemyHealCalc(Card _card, Entity _entity)
    {
        if (_entity.i_health + _card.i_damage <= _entity.HEALTHMAX)
        {
            _entity.i_health += _card.i_damage;
            _entity.RefreshEntity();
            IsUseEnchance();
        }
        else
        {
            _entity.i_health = _entity.HEALTHMAX;
            _entity.RefreshEntity();
            IsUseEnchance();
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
        IsUseEnchance();
    }

    //주문 강화


    // 쉴드
    public void PlayerShieldCalc(Card _card, PlayerEntity _playerEntity)
    {
        _playerEntity.i_shield += _card.i_damage;
        _playerEntity.RefreshPlayer();
        IsUseEnchance();
    }


    //힐
    public void PlayerHealCalc(Card _card, PlayerEntity _playerEntity)
    {
        if (_playerEntity.i_health + _card.i_damage <= _playerEntity.HEALTHMAX)
        {
            _playerEntity.i_health += _card.i_damage;
            _playerEntity.RefreshPlayer();
            IsUseEnchance();
        }
        else
        {
            _playerEntity.i_health = _playerEntity.HEALTHMAX;
            _playerEntity.RefreshPlayer();
            IsUseEnchance();
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
