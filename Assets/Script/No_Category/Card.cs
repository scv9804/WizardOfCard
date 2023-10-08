using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Card

[Serializable]
public class Card
{
    // ================================================================================ Constance

    // ============================================================ General

    public const int MAX_LEVEL = 2;

    // ================================================================================ Field

    // ============================================================ Data

    [ShowInInspector]
    private CardData _data;

    // ================================================================================ Method

    // ============================================================ Constructor

    private Card()
    {

    }

    public Card(CardData data)
    {
        _data = data;
    }
}
