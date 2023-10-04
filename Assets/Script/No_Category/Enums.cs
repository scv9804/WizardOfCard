using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// ================================================================================ CardType

public enum CardType
{
    NONE    = 0,

    ATTACK  = 10,
    DEFENSE = 11,
    HEAL    = 12
}

// ================================================================================ CardKeyword

[Flags]
public enum CardKeyword
{
    OBLIVION    = 1 << 1,
    VANISH      = 1 << 2
}