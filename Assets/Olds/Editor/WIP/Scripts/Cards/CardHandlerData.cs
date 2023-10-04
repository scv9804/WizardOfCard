using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== CardHandlerData

    [CreateAssetMenu(menuName = "WIP/Card/HandlerData", fileName = "_CardHandlerData")]
    public class CardHandlerData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Action

        [Header("대상 데이터")]
        [SerializeField] private List<CardActionNode> _actions = new List<CardActionNode>();

        // ==================================================================================================== Property

        // =========================================================================== Action

        public List<CardActionNode> Actions
        {
            get
            {
                return _actions;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Status

        public string GetDescription(string description, int upgraded)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                description = Actions[i].GetDescription(description, upgraded);
            }

            return description;
        }

        // =========================================================================== Handler

        public CardHandler Execute(Card card, Entity target)
        {
            CardHandler handler = new CardHandler();

            handler.Target = target;

            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Execute(card, handler);
            }

            handler.Execute();

            return handler;
        }
    }

    public class CardHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Target

        private Entity _target;

        // =========================================================================== Skill

        private List<EntityActionCommand> _commands = new List<EntityActionCommand>();

        // ==================================================================================================== Property

        // =========================================================================== Target

        public Entity Target
        {
            get
            {
                return _target;
            }

            set
            {
                _target = value;
            }
        }

        // =========================================================================== Skill

        public List<EntityActionCommand> Commands
        {
            get
            {
                return _commands;
            }

            set
            {
                _commands = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Skill

        public void Execute()
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                Commands[i].Execute(Target);
            }
        }
    }
}