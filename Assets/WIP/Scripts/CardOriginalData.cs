using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;

namespace WIP
{
    [CreateAssetMenu(menuName = "WIP/Card/OriginalData", fileName = "_OriginalData")]
    public class CardOriginalData : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Base

        [Header("ÀÌ¸§")]
        [SerializeField] private string _name;

        [Header("Èñ±Íµµ")]
        [SerializeField] private CardRarity _rarity;

        [Header("ºñ¿ë")]
        [SerializeField] private List<int> _cost = new List<int>(Card.MAX_UPGRADE_LEVEL + 1);

        [Header("¸Á°¢ ¿©ºÎ")]
        [SerializeField] private List<bool> _isExile = new List<bool>(Card.MAX_UPGRADE_LEVEL + 1);

        [Header("¼³¸í")]
        [SerializeField, TextArea(3, 5)] private List<string> _description = new List<string>(Card.MAX_UPGRADE_LEVEL + 1);

        // ==================================================================================================== Property

        // =========================================================================== Data

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

        public ReadOnlyCollection<int> Cost
        {
            get
            {
                return _cost.AsReadOnly();
            }
        }

        public ReadOnlyCollection<bool> IsExile
        {
            get
            {
                return _isExile.AsReadOnly();
            }
        }

        public ReadOnlyCollection<string> Description
        {
            get
            {
                return _description.AsReadOnly();
            }
        }
    }

    // ==================================================================================================== CardRarity

    [Serializable] public class CardRarity
    {
        // ==================================================================================================== Field

        [Header("Èñ±Íµµ")]
        [SerializeField] private RarityType _rarityType;

        [Header("°ª")]
        [SerializeField] private float _rarityRate;

        // ==================================================================================================== Property

        public RarityType RarityType
        {
            get
            {
                return _rarityType;
            }
        }

        public float RarityRate
        {
            get
            {
                return _rarityRate;
            }
        }
    }

    public enum RarityType
    {
        Normal,

        Rare,

        Unique
    }
}
