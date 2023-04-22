using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.ObjectModel;
using System.Text;

namespace WIP
{
    // ==================================================================================================== CardBehaviourNodeData

    public abstract class CardBehaviourNodeData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Base

        [Header("�ߵ� ����")]
        [SerializeField] private List<bool> _isActive = new List<bool>(Card.MAX_UPGRADE_LEVEL + 1);

        [Header("��� Ÿ��")]
        [SerializeField] private CardBehaviourNodeType _nodeType;

        [Header("�ڽ� ��� ID")]
        [SerializeField] private List<int> _linkedNodes = new List<int>();

        [Header("�ݺ� ����")]
        [SerializeField] private List<bool> _isLoop = new List<bool>(Card.MAX_UPGRADE_LEVEL + 1);

        // =========================================================================== Data

        // ================================================== Base

        [Header("���� ���˰�")]
        [SerializeField] private string _descriptionFormat;

        // =========================================================================== StringBuilder

        private StringBuilder _stringBuilder = new StringBuilder();

        // ==================================================================================================== Property

        // =========================================================================== Behaviour

        // ================================================== Base

        public ReadOnlyCollection<bool> IsActive
        {
            get
            {
                return _isActive.AsReadOnly();
            }
        }

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

        // =========================================================================== Data

        // ================================================== Base

        public string DescriptionFormat
        {
            get
            {
                return _descriptionFormat;
            }
        }

        // =========================================================================== StringBuilder

        protected StringBuilder StringBuilder
        {
            get
            {
                return _stringBuilder;
            }

            set
            {
                _stringBuilder = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Behaviour

        public abstract void Create();
    }

    // ==================================================================================================== CardBehaviourNode

    public class CardBehaviourNode : CardBehaviourRootNode
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Base

        public bool IsActive;
    }

    // ==================================================================================================== CardBehaviourNodeType

    public enum CardBehaviourNodeType
    {
        Sequence,

        Selector,

        Parallel
    }
}
