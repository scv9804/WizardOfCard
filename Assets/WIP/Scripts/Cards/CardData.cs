using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== CardData

    [CreateAssetMenu(menuName = "WIP/Card/Data", fileName = "_CardData")]
    public class CardData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Status

        // ================================================== Base

        [Header("�̸�")]
        [SerializeField] private string _name;

        [Header("��͵�")]
        [SerializeField] private CardRarity _rarity;

        [Header("���")]
        [SerializeField] private int[] _cost = new int[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("Ű����")]
        [SerializeField] private CardKeyword[] _keyword = new CardKeyword[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("����")]
        [SerializeField, TextArea(3, 5)] private string[] _description = new string[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("���� Ÿ��")]
        [SerializeField] private bool _targetSelf;

        // ================================================== Asset

        [Header("������ ��������Ʈ")]
        [SerializeField] private Sprite[] _frameSprite = new Sprite[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("������ ��������Ʈ")]
        [SerializeField] private Sprite[] _artworkSprite = new Sprite[Card.MAX_UPGRADE_LEVEL + 1];

        // ================================================== Ability

        [Header("ȿ�� ������")]
        [SerializeField] private CardHandlerData _handlerData;

        [Header("��� ���� ������")]
        [SerializeField] private CardTargetData _targetData;

        // ==================================================================================================== Property

        // =========================================================================== Status

        // ================================================== Base

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public CardRarity Rarity
        {
            get
            {
                return _rarity;
            }
        }

        public int[] Cost
        {
            get
            {
                return _cost;
            }
        }

        public CardKeyword[] Keyword
        {
            get
            {
                return _keyword;
            }
        }

        public bool TargetSelf
		{
			get
			{
                return _targetSelf; 
			}
			set
			{
                _targetSelf = value;
			}
		}

        public string[] Description
        {
            get
            {
                return _description;
            }
        }

        // ================================================== Asset

        public Sprite[] FrameSprite
        {
            get
            {
                return _frameSprite;
            }
        }

        public Sprite[] ArtworkSprite
        {
            get
            {
                return _artworkSprite;
            }
        }

        // ================================================== Ability

        public CardHandlerData HandlerData
        {
            get
            {
                return _handlerData;
            }
        }

        public CardTargetData TargetData
        {
            get
            {
                return _targetData;
            }
        }
    }

    // ==================================================================================================== CardRarity

    [Serializable] public class CardRarity
    {
        // ==================================================================================================== Field

        // =========================================================================== Rarity

        [Header("���")]
        [SerializeField] private CardRarityType _type;

        [Header("�����")]
        [SerializeField] private float _value;

        // ==================================================================================================== Property

        // =========================================================================== Rarity

        public CardRarityType Type
        {
            get
            {
                return _type;
            }
        }

        public float Value
        {
            get
            {
                return _value;
            }
        }
    }

    // ==================================================================================================== CardRarityType

    public enum CardRarityType
    {
        Basic,

        Common,

        Rare,

        Unique
    }
}
