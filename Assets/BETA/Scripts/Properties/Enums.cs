using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA.Enums
{
    // ==================================================================================================== Arrow

    [Flags]
    public enum Arrow
    {
        NONE    = 1 << 0,

        UP      = 1 << 1,
        DOWN    = 1 << 2,
        LEFT    = 1 << 3,
        RIGHT   = 1 << 4,

        END     = 1 << 5,

        All = int.MaxValue
    }

    // ==================================================================================================== CardKeyword

    [Flags]
    public enum CardKeyword
    {
        NONE        = 1 << 0,

        OBLIVION    = 1 << 1,

        //TRAP,
        //RUNE,

        ALL         = int.MaxValue
    }

    // ==================================================================================================== CardState

    public enum CardState
    {
        NONE            = 0,

        UNABLE          = 10,

        ON_POINTER_OVER = 20,
        ON_DRAG         = 21,
        ON_USING        = 22,
        ON_MOVING       = 23
    }

    // ==================================================================================================== CardType

    public enum CardType
    {
        NONE,

        ATTACK,
        FIRE
    }

    // ==================================================================================================== IFFType

    [Flags]
    public enum IFFType
    {
        NONE    = 1 << 0,

        PLAYER  = 1 << 1,
        MONSTER = 1 << 2,

        ALL     = int.MaxValue
    }

    // ==================================================================================================== TileColor

    public enum TileColor
    {
        MOVEMENT_COLOR,

        ATTACK_RANGE_COLOR,

        ATTACK_COLOR
    }

    // ==================================================================================================== TileType

    public enum TileType
    {
        TRAVERSABLE,

        NON_TRAVERSABLE,

        EFFECT
    }
}
