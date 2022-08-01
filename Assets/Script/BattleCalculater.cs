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

    [SerializeField] Cards_Magician cards_Magician;
    int Defult;
    int culc_damage;

    public bool BattleCalc(Card _card, PlayerEntity _target)
	{
        _target.Status_Aether -= _card.i_manaCost;
        if (_target.Status_Aether <= 0)
        {
            TurnManager.Inst.EndTurn();
            return false;
        }
		else
		{
          
            return true;
        }
    }

    public bool BattleCalc(Card _card, Entity _target)
    {
        EntityManager.Inst.playerEntity.Status_Aether -= _card.i_manaCost;
        if (EntityManager.Inst.playerEntity.Status_Aether <= 0)
        {
            TurnManager.Inst.EndTurn();
            return false;
		}
            cards_Magician.MagicBolt(_card, _target);
            return true;
        
    }


    void EnchaneDamage()
	{
       
	}


    // ��ȭ���ΰ�?
    public void SpellEnchaneReset()
    {
        PlayerEntity.Inst.Status_EnchaneValue = 1;
    }
    public void SpellEnhaneCalc(Card _card)
    {
        PlayerEntity.Inst.Status_EnchaneValue *= _card.i_damage;
    }



	#region EnemyTarget

	//�⺻ �ֹ� ������


    //�ֹ� ��ȭ


    // ����
    public void EnemyShieldCalc(Card _card, Entity _entity)
    {
        _entity.i_shield += _card.i_damage;
        _entity.RefreshEntity();
        SpellEnchaneReset();
    }

    
    //��
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
        SpellEnchaneReset();
    }

    //�ֹ� ��ȭ


    // ����
    public void PlayerShieldCalc(Card _card, PlayerEntity _playerEntity)
    {
        _playerEntity.Status_Shiled += _card.i_damage;
        _playerEntity.RefreshPlayer();
        SpellEnchaneReset();
    }


    //��
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
