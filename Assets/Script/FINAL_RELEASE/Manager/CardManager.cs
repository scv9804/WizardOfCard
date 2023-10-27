using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ================================================================================ CardManager

public class CardManager : Singleton<CardManager>
{
    // ================================================================================ Constant

    // ============================================================ Card

    public const string OWN = "OWN";
    public const string DECK = "DECK";
    public const string HAND = "HAND";
    public const string DISCARD = "DISCARD";
    public const string EXCLUDE = "EXCLUDE";

    public const string SHOP = "SHOP";
    public const string EVENT = "EVENT";
    public const string REWARD = "REWARD";

}
