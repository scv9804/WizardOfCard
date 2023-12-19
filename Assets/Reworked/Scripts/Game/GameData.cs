using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Reworked
{
    // ==================================================================================================== Game.Data

    public static partial class Game
    {
        [Serializable] public class Data
        {
            // ==================================================================================================== Field

            // =========================================================================== Identifier

            [Header("할당 개채 순번")]
            public int Allocated = 0;

            // ==================================================================================================== Method

            // =========================================================================== Instance

            public void Clear()
            {
                Allocated = 0;
            }
        }
    }
}