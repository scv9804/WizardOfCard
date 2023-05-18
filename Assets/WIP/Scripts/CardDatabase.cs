using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    [CreateAssetMenu(menuName = "WIP/Card/Database", fileName = "CardDatabase")]
    public class CardDatabase : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [Header("데이터베이스")]
        [SerializeField] private List<CardData> _cards = new List<CardData>();

        // ==================================================================================================== Property

        // =========================================================================== Data

        public List<CardData> Cards
        {
            get
            {
                return _cards;
            }
        }
    }
}