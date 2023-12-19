using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;

namespace Reworked
{
    // ==================================================================================================== GameDatabase

    [CreateAssetMenu(menuName = "Game/Database", fileName = "Database")]
    public class GameDatabase : ScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Card

        [Header("카드 데이터")]
        [SerializeField] private Card.OriginData[] _cards;

        //

        //

        // ==================================================================================================== Field

        // =========================================================================== Data

        // ================================================== Card

        public ReadOnlyCollection<Card.OriginData> Cards
        {
            get
            {
                return Array.AsReadOnly(_cards);
            }
        }

        //

        //
    }
}
