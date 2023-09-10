using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA.Enums
{
    // ==================================================================================================== CardType

    [Flags]
    public enum CardKeyword
    {
        NONE,

        OBLIVION,

        //TRAP,
        //RUNE,

        ALL = int.MaxValue
    }

    // ==================================================================================================== CardType

    public enum CardType
    {
        NONE,

        ATTACK,
        FIRE
    }
}
