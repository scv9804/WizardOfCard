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

    public class EntityAttackCommand : EntityActionCommand
    {
        // ==================================================================================================== Field

        // =========================================================================== Command

        // ================================================== Attack

        private int _damage;

        // ================================================== Power

        private List<int> _attackPowers = new List<int>();

        private List<int> _modifiers = new List<int>();

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

        public List<int> Modifiers
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

            for (int i = 0; i < Modifiers.Count; i++)
            {
                damage *= Modifiers[i];
            }

            return damage;
        }
    }

    // ==================================================================================================== IAggresive

    public interface IAggresive
    {
        // ==================================================================================================== Property

        // =========================================================================== Command

        // ================================================== Power

        public int AttackPower
        {
            get; set;
        }

        public int Modifier
        {
            get; set;
        }
    }

    // ==================================================================================================== IDefensive

    public interface IDefensive
    {

    }
}
