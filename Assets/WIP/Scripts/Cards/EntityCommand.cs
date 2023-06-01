using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== EntityActionCommand

    public abstract class EntityActionCommand
    {
        // ==================================================================================================== Method

        // =========================================================================== Command

        // ================================================== Base

        public abstract void Execute(Entity target);
    }

    // ==================================================================================================== EntityAttackCommand

    public class EntityAttackCommand : EntityActionCommand, IAggresive
    {
        // ==================================================================================================== Field

        // =========================================================================== Command

        // ================================================== Attack

        private int _damage;

        // ================================================== Power

        private List<int> _attackPowers = new List<int>();

        private List<int> _multipliers = new List<int>();

        // =========================================================================== Asset

        // ================================================== Effect

        private Sprite _playerAttackSprite;
        private Sprite _enemyDamagedSprite;

        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Attack

        public int Damage
        {
            get
            {
                return _damage;
            }

            set
            {
                _damage = value;
            }
        }

        // ================================================== Power

        public List<int> AttackPowers
        {
            get
            {
                return _attackPowers;
            }

            set
            {
                _attackPowers = value;
            }
        }

        // _multipliers
        // Multipliers

        public List<int> Multipliers
        {
            get
            {
                return _multipliers;
            }

            set
            {
                _multipliers = value;
            }
        }

        // =========================================================================== Asset

        // ================================================== Effect

        public Sprite PlayerAttackSprite
        {
            get
            {
                return _playerAttackSprite;
            }

            set
            {
                _playerAttackSprite = value;
            }
        }

        public Sprite EnemyDamagedSprite
        {
            get
            {
                return _enemyDamagedSprite;
            }

            set
            {
                _enemyDamagedSprite = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Command

        // ================================================== Base

        public override void Execute(Entity target)
        {
            //////////////////////////////////////////////////
            Debug.Log($"{GetDamage()}의 데미지로 공격함");
            //////////////////////////////////////////////////

            if (target == null || target.is_die)
            {
                return;
            }

            PlayerEntity player = EntityManager.Inst.playerEntity;

            player.AttackSprite(player.playerChar.MagicBoltSprite, PlayerAttackSprite);
            target.Damaged(GetDamage(), EnemyDamagedSprite);
        }

        // ================================================== Attack

        public int GetDamage()
        {
            int damage = Damage;

            for (int i = 0; i < AttackPowers.Count; i++)
            {
                damage += AttackPowers[i];
            }

            for (int i = 0; i < Multipliers.Count; i++)
            {
                damage *= Multipliers[i];
            }

            return damage;
        }
    }

    // ==================================================================================================== EntityShieldCommand

    public class EntityShieldCommand : EntityActionCommand, IDefensive
    {
        // ==================================================================================================== Field

        // =========================================================================== Command

        // ================================================== Shield

        private int _shield;

        // ================================================== Power

        private List<int> _defensePowers = new List<int>();

        private List<int> _modifiers = new List<int>();

        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Shield

        public int Shield
        {
            get
            {
                return _shield;
            }

            set
            {
                _shield = value;
            }
        }

        // ================================================== Power

        public List<int> DefensePowers
        {
            get
            {
                return _defensePowers;
            }

            set
            {
                _defensePowers = value;
            }
        }

        public List<int> Multipliers
        {
            get
            {
                return _modifiers;
            }

            set
            {
                _modifiers = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Command

        // ================================================== Base

        public override void Execute(Entity target)
        {
            //////////////////////////////////////////////////
            Debug.Log($"{GetShield()}의 쉴드를 생성함");
            //////////////////////////////////////////////////
            
            if (true)
            {
                return;
            }

            PlayerEntity player = EntityManager.Inst.playerEntity;

            player.Status_Shiled += GetShield();
        }
            
        // ================================================== Attack

        public int GetShield()
        {
            int damage = Shield;

            for (int i = 0; i < DefensePowers.Count; i++)
            {
                damage += DefensePowers[i];
            }

            for (int i = 0; i < Multipliers.Count; i++)
            {
                damage *= Multipliers[i];
            }

            return damage;
        }
    }

    // ==================================================================================================== EntityBuffCommand

    public class EntityBuffCommand /*: EntityActionCommand, IModifier*/
    {

    }

    // ==================================================================================================== IAggresive

    public interface IAggresive : IMultipliers
    {
        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Power

        public List<int> AttackPowers
        {
            get; set;
        }
    }

    // ==================================================================================================== IDefensive

    public interface IDefensive : IMultipliers
    {
        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Power

        public List<int> DefensePowers
        {
            get; set;
        }
    }

    // ==================================================================================================== IMultipliers

    public interface IMultipliers
    {
        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Power

        public List<int> Multipliers
        {
            get; set;
        }
    }

    // ==================================================================================================== BuffType

    public enum BuffType
    {
        AttackPowerForTurn
    }
}
