using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardSkill_C000_Temp

    [CreateAssetMenu(menuName = "WIP/Card/Skill/C000", fileName = "C000_CardSkill_Temp")]
    public class CardSkill_C000_Temp : CardSkill_Temp
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        [Header("비용")]
        [SerializeField] private List<int> _damage = new List<int>(Card.MAX_STATUS_SIZE);

        // =========================================================================== Effect

        [Header("공격 이펙트 스프라이트")]
        [SerializeField] private Sprite _playerEffectSprite;

        [Header("피격 이펙트 스프라이트")]
        [SerializeField] private Sprite _targetEffectSprite;

        // ==================================================================================================== Property

        // =========================================================================== Status

        public List<int> Damage
        {
            get
            {
                return _damage;
            }
        }

        // =========================================================================== Effect

        public Sprite PlayerEffectSprite
        {
            get
            {
                return _playerEffectSprite;
            }
        }

        public Sprite TargetEffectSprite
        {
            get
            {
                return _targetEffectSprite;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Skill

        // ================================================== Base

        public override ICardSkillModel_Temp Create(int upgraded)
        {
            var skillModel = new CardSkillModel_C000_Temp();
            skillModel.Damage = Damage[upgraded];

            return skillModel;
        }

        public override IEnumerator Execute(CardTarget_Temp target, Card card, ICardSkillModel_Temp skillModel)
        {
            ICardAttackProperty_Temp skillModel_C000 = skillModel as ICardAttackProperty_Temp;

            Debug.Log("마법구 사용");

            if (target.PlayerTarget != null)
            {
                Attack(target.PlayerTarget, skillModel_C000.Damage);
            }

            for (int i = 0; i < target.EnemyTargets.Count; i++)
            {
                Attack(target.EnemyTargets[i], skillModel_C000.Damage);
            }

            yield return null;
        }

        // ================================================== Attack

        public void Attack(PlayerEntity _target, int damage)
        {
            _target?.Damaged(damage);
            _target?.SetDamagedSprite(TargetEffectSprite);

            //MusicManager.inst?.PlayerDefultSoundEffect();
        }

        public void Attack(Entity _target, int damage)
        {
            if (!_target.is_die)
            {
                _target?.Damaged(damage, TargetEffectSprite);

                //MusicManager.inst?.PlayerDefultSoundEffect();
            }
        }

        // =========================================================================== Status

        public override string RefreshDescription(Card card, ICardSkillModel_Temp skill)
        {
            Utility.StringBuilder.Clear();
            Utility.StringBuilder.Append(card.Description);

            Utility.StringBuilder.Replace("{0}", "<color=#FF0000>{0}</color>");
            Utility.StringBuilder.Replace("{0}", Damage[card.Upgraded].ToString());

            return Utility.StringBuilder.ToString();
        }
    }

    // ==================================================================================================== CardSkillModel_C000_Temp

    public class CardSkillModel_C000_Temp : ICardAttackProperty_Temp
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        private int _damage;

        // ==================================================================================================== Property

        // =========================================================================== Status

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
    }
}