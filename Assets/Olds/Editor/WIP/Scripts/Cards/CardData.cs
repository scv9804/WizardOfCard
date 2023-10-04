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

        [Header("이름")]
        [SerializeField] private string _name;

        [Header("희귀도")]
        [SerializeField] private CardRarity _rarity;

        [Header("비용")]
        [SerializeField] private int[] _cost = new int[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("키워드")]
        [SerializeField] private CardKeyword[] _keyword = new CardKeyword[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("설명")]
        [SerializeField, TextArea(3, 5)] private string[] _description = new string[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("본인 타겟")]
        [SerializeField] private bool _targetSelf;

        // ================================================== Asset

        [Header("프레임 스프라이트")]
        [SerializeField] private Sprite[] _frameSprite = new Sprite[Card.MAX_UPGRADE_LEVEL + 1];

        [Header("아이콘 스프라이트")]
        [SerializeField] private Sprite[] _artworkSprite = new Sprite[Card.MAX_UPGRADE_LEVEL + 1];

        // ================================================== Ability

        [Header("효과 데이터")]
        [SerializeField] private CardHandlerData _handlerData;

        [Header("대상 설정 데이터")]
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

        [Header("등급")]
        [SerializeField] private CardRarityType _type;

        [Header("등장률")]
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
