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

        [Header("이름")]
        [SerializeField] private string _name;

        [Header("희귀도")]
        [SerializeField] private CardRarity _rarity;

        [Header("비용")]
        [SerializeField] private List<int> _cost = new List<int>(Card.MAX_STATUS_SIZE);

        [Header("키워드")]
        [SerializeField] private List<CardKeyword> _keyword = new List<CardKeyword>(Card.MAX_STATUS_SIZE);

        [Header("설명")]
        [SerializeField, TextArea(3, 5)] private List<string> _description = new List<string>(Card.MAX_STATUS_SIZE);

        // =========================================================================== Asset

        [Header("프레임 스프라이트")]
        [SerializeField] private List<Sprite> _frameSprite = new List<Sprite>(Card.MAX_STATUS_SIZE);

        [Header("아이콘 스프라이트")]
        [SerializeField] private List<Sprite> _artworkSprite = new List<Sprite>(Card.MAX_STATUS_SIZE);

        // =========================================================================== Skill

        [Header("스킬 데이터")]
        [SerializeField] private CardSkill_Temp _skillData;

        // ==================================================================================================== Property

        // =========================================================================== Status

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

        public List<int> Cost
        {
            get
            {
                return _cost;
            }
        }

        public List<CardKeyword> Keyword
        {
            get
            {
                return _keyword;
            }
        }

        public List<string> Description
        {
            get
            {
                return _description;
            }
        }

        // =========================================================================== Asset

        public List<Sprite> FrameSprite
        {
            get
            {
                return _frameSprite;
            }
        }

        public List<Sprite> ArtworkSprite
        {
            get
            {
                return _artworkSprite;
            }
        }

        // =========================================================================== Skill

        public CardSkill_Temp SkillData
        {
            get
            {
                return _skillData;
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
