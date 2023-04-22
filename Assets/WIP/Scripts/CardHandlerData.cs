using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;
using System.Text;

namespace WIP
{
    // ==================================================================================================== CardHandlerData

    // TODO: Create CardHandlerNodePoolManager
    // HandlerData contains CardRootData
    [CreateAssetMenu(menuName = "WIP/Card/Handler", fileName = "_Handler")]
    public class CardHandlerData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Model

        [Header("발동 효과")]
        [SerializeField] private List<CardBehaviourNodeData> _behaviours = new List<CardBehaviourNodeData>();

        // ================================================== Root

        [Header("노드 타입")]
        [SerializeField] private CardBehaviourNodeType _nodeType;

        [Header("자식 노드 ID")]
        [SerializeField] private List<int> _linkedNodes = new List<int>();

        [Header("반복 여부")]
        [SerializeField] private List<bool> _isLoop = new List<bool>(Card.MAX_UPGRADE_LEVEL + 1);

        // =========================================================================== BlackBoard

        // ================================================== Model

        [Header("블랙보드 데이터")]
        [SerializeField] private List<CardBlackBoardNodeData> _blackBoards = new List<CardBlackBoardNodeData>();

        // =========================================================================== Event

        // ================================================== Model

        // TODO: List<CardEventNodeData> Events

        // =========================================================================== StringBuilder

        private StringBuilder _stringBuilder = new StringBuilder();

        // ==================================================================================================== Property

        // =========================================================================== Behaviour

        // ================================================== Model

        public ReadOnlyCollection<CardBehaviourNodeData> Behaviours
        {
            get
            {
                return _behaviours.AsReadOnly();
            }
        }

        // ================================================== Root

        public CardBehaviourNodeType NodeType
        {
            get
            {
                return _nodeType;
            }
        }

        public ReadOnlyCollection<int> LinkedNodes
        {
            get
            {
                return _linkedNodes.AsReadOnly();
            }
        }

        public ReadOnlyCollection<bool> IsLoop
        {
            get
            {
                return _isLoop.AsReadOnly();
            }
        }

        // =========================================================================== BlackBoard

        // ================================================== Model

        public ReadOnlyCollection<CardBlackBoardNodeData> BlackBoards
        {
            get
            {
                return _blackBoards.AsReadOnly();
            }
        }

        // =========================================================================== Event

        // ================================================== Model

        // TODO: List<CardEventNodeData> Events

        // ==================================================================================================== Method

        // =========================================================================== Handler

        public CardHandler Create(Card card)
        {
            var handler = new CardHandler()
            {
                LinkedNodes = _linkedNodes,

                IsLoop = _isLoop[card.Upgraded]
            };

            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].Create();
            }

            for (int i = 0; i < _blackBoards.Count; i++)
            {
                _blackBoards[i].Create();
            }

            // TODO: Apply Buff System

            return handler;
        }
    }

    // ==================================================================================================== CardHandler

    public class CardHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Controller

        public List<CardBehaviourNode> Behaviours = new List<CardBehaviourNode>();

        // ================================================== Root

        public CardBehaviourRootNode Root = new CardBehaviourRootNode();

        public List<int> LinkedNodes = new List<int>();

        public bool IsLoop;

        // =========================================================================== BlackBoard

        // ================================================== Controller

        public List<CardBlackBoardNode> BlackBoards = new List<CardBlackBoardNode>();

        // ================================================== Root

        public CardBehaviourProcess Result = CardBehaviourProcess.None;

        public int InvokeCount = 0;

        // ==================================================================================================== Method

        // =========================================================================== Handler

        // ================================================== Base

        public IEnumerator Invoke()
        {
            yield return CardManager.Instance.StartCoroutine(Root.Invoke(this, -1));
        }

        public void RecordResult(int index)
        {
            BlackBoards[index].Result = IsSelector(index) ? CardBehaviourProcess.Success : CardBehaviourProcess.Failed;
        }

        // ================================================== Evaluation

        public bool IsSelector(int index)
        {
            return Behaviours[index].NodeType == CardBehaviourNodeType.Sequence;
        }

        public bool IsParallel(int index)
        {
            return Behaviours[index].NodeType == CardBehaviourNodeType.Parallel;
        }

        public bool IsFailed(int index)
        {
            return BlackBoards[index].Result == CardBehaviourProcess.Failed;
        }

        public bool Evaluation(int index)
        {
            return IsParallel(index) || (IsSelector(index) == IsFailed(index));
        }

        // =========================================================================== Data

        // ================================================== Base

        public string AssemblyDescription()
        {
            return null;
        }
    }

    // ==================================================================================================== CardRootNode

    public class CardBehaviourRootNode
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Base

        public CardBehaviourNodeType NodeType;

        public List<int> LinkedNodes = new List<int>();

        public bool IsLoop;

        // ==================================================================================================== Method

        // =========================================================================== Behaviour

        // ================================================== Base

        public virtual IEnumerator Invoke(CardHandler handler, int current)
        {
            do
            {
                handler.InvokeCount += 1;

                for (int i = 0; i < LinkedNodes.Count; i++)
                {
                    if (!handler.Behaviours[i].IsActive)
                    {
                        continue;
                    }

                    CardManager.Instance.StartCoroutine(handler.Behaviours[i].Invoke(handler, LinkedNodes[i]));

                    if (!handler.Evaluation(i))
                    {
                        handler.RecordResult(current);

                        yield break;
                    }
                }
            }
            while (IsLoop);

            handler.RecordResult(current);

            yield return !handler.IsFailed(current);
        }

        // =========================================================================== Data

        // ================================================== Base

        public virtual string AssemblyDescription()
        {
            for (int i = 0; i < LinkedNodes.Count; i++)
            {

            }

            return null;
        }
    }
}
