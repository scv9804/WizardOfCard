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
    [HideInInspector] public int i_everlasting = 0;  //���� ���� ���� (������������)

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


    // ��ȭ���ΰ�?
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

	//�⺻ �ֹ� ������
	public void EnemySpellDamagedCalc(Card _card, Entity _target)
    {
        _target?.Damaged(_card.i_damage);
        IsUseEnchance();
    }

    //�ֹ� ��ȭ


    // ����
    public void EnemyShieldCalc(Card _card, Entity _entity)
    {
        _entity.i_shield += _card.i_damage;
        _entity.RefreshEntity();
        IsUseEnchance();
    }


    //��
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

    //����
    public void EnemyBuffCalc(Card _card)
    {

    }

    //�����
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

    //�ֹ� ��ȭ


    // ����
    public void PlayerShieldCalc(Card _card, PlayerEntity _playerEntity)
    {
        _playerEntity.i_shield += _card.i_damage;
        _playerEntity.RefreshPlayer();
        IsUseEnchance();
    }


    //��
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

    //����
    public void PlayerBuffCalc(Card _card)
    {

    }

    //�����
    public void PlayerDebuffCalc(Card _card)
    {

    }

    #endregion

}
