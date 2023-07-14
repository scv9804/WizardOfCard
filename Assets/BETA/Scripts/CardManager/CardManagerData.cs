using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    // ==================================================================================================== CardManager.Data

    public sealed partial class CardManager : Card.ManagerBehaviour<CardManager>
    {
        [Serializable] public sealed class Data<T>
        {
            // ==================================================================================================== Field

            // =========================================================================== Data

            [Header("보유 카드")]
            public List<T> Owned = new List<T>();

            [Header("덱 카드")]
            public List<T> Deck = new List<T>();

            [Header("손패 카드")]
            public List<T> Hand = new List<T>();

            [Header("사용 카드")]
            public List<T> Discarded = new List<T>();

            [Header("제외 카드")]
            public List<T> Exiled = new List<T>();
        }
    }
}