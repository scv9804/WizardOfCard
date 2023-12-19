using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Reworked
{
    // ==================================================================================================== Card.Cache

    public partial class Card
    {
        public static class Cache
        {
            // ==================================================================================================== Field

            // =========================================================================== Data

            private static Dictionary<string, Data> s_data = new Dictionary<string, Data>();

            // ==================================================================================================== Property

            // =========================================================================== Data

            public static Dictionary<string, Data> Data
            {
                get
                {
                    return s_data;
                }
            }

            // ==================================================================================================== Method

            // =========================================================================== Data

            public static void Clear()
            {
                Data.Clear();
            }

            public static void Refresh()
            {

            }
        }
    }
}
