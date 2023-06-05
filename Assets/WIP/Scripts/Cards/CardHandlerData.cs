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

        // =========================================================================== Target

        [Header("��� ������")]
        [SerializeField] private CardTargetData _targetData = new CardTargetData();

        // =========================================================================== Action

        [Header("��� ������")]
        [SerializeField] private List<CardActionNode> _actions = new List<CardActionNode>();

        // ==================================================================================================== Property

        // =========================================================================== Target

        public CardTargetData TargetData
        {
            get
            {
                return _targetData;
            }
        }

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

    // ==================================================================================================== CardTargetData

    // Ÿ���� ���� ���� �ʿ�
    [Serializable] public class CardTargetData
    {
        // ==================================================================================================== Field

        // =========================================================================== Area

        [Header("��Ÿ�")]
        [SerializeField] private int _range;

        [Header("����")]
        [SerializeField] private Vector3 _radius;
        // Vector3

        // =========================================================================== Option

        [Header("���� ��� ����")]
        [SerializeField] private bool _isTargetable;

        [Header("������ ���� ����")]
        [SerializeField] private bool _isRandom;

        // ==================================================================================================== Property

        // =========================================================================== Area

        public int Range
        {
            get
            {
                return _range;
            }

            set
            {
                _range = value;
            }
        }

        public Vector3 Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        // =========================================================================== Option

        public bool IsTargetable
        {
            get
            {
                return _isTargetable;
            }

            set
            {
                _isTargetable = value;
            }
        }

        public bool IsRandom
        {
            get
            {
                return _isRandom;
            }

            set
            {
                _isRandom = value;
            }
        }
    }
}