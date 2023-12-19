using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Reworked
{
    // ==================================================================================================== CardManager.Data

    public partial class CardManager
    {
        [Serializable] public sealed class Data
        {
            // ==================================================================================================== Field

            // =========================================================================== Card

            [Header("보유 카드")]
            public List<string> Owned = new List<string>();

            [Header("덱 카드")]
            public List<string> Deck = new List<string>();

            [Header("손패 카드")]
            public List<string> Hand = new List<string>();

            [Header("사용 후 카드")]
            public List<string> Discard = new List<string>();

            [Header("제외 카드")]
            public List<string> Exiled = new List<string>();
        }
    } 
}
