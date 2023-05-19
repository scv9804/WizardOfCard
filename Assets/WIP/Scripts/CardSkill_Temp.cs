using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;

namespace WIP
{
    // ==================================================================================================== CardSkill_Temp

    public abstract class CardSkill_Temp : ScriptableObject, ICardSkill_Temp
    {
        // ==================================================================================================== Method

        // =========================================================================== Skill

        // ================================================== Base

        public abstract ICardSkillModel_Temp Create(int upgraded);

        public abstract IEnumerator Execute(CardTarget_Temp target, CardModel model, ICardSkillModel_Temp skillModel);

        // =========================================================================== Status

        public abstract string RefreshDescription(CardModel model, ICardSkillModel_Temp skill);
    }

    // ==================================================================================================== CardTarget_Temp

    public class CardTarget_Temp
    {
        // ==================================================================================================== Field

        // =========================================================================== Target

        private PlayerEntity _playerTarget;

        private List<Entity> _enemyTargets = new List<Entity>();

        // ==================================================================================================== Property

        // =========================================================================== Target

        public PlayerEntity PlayerTarget
        {
            get
            {
                return _playerTarget;
            }

            set
            {
                _playerTarget = value;
            }
        }

        public List<Entity> EnemyTargets
        {
            get
            {
                return _enemyTargets;
            }

            set
            {
                _enemyTargets = value;
            }
        }
    }

    // ==================================================================================================== ICardSkill_Temp

    public interface ICardSkill_Temp
    {
        // ==================================================================================================== Method

        // =========================================================================== Skill

        // ================================================== Base

        public ICardSkillModel_Temp Create(int upgraded);

        public IEnumerator Execute(CardTarget_Temp target, CardModel model, ICardSkillModel_Temp skillModel);

        // =========================================================================== Status

        public string RefreshDescription(CardModel model, ICardSkillModel_Temp skill);
    }

    // ==================================================================================================== ICardSkillModel_Temp

    public interface ICardSkillModel_Temp
    {

    }

    // ==================================================================================================== ICardAttackProperty_Temp

    public interface ICardAttackProperty_Temp : ICardSkillModel_Temp
    {
        // ==================================================================================================== Property

        // =========================================================================== Status

        public int Damage
        {
            get; set;
        }
    }
}
