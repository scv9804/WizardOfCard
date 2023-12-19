using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== Card.Instance

    public sealed partial class Card
    {
        static class Instance
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

                private set
                {
                    s_data = value;
                }
            }

            // ==================================================================================================== Method

            // =========================================================================== BETA

            public static void ReadAllData()
            {
                foreach (var record in Data)
                {
                    EditorDebug.EditorLog($"{"Instance ID".Color("#7FFFD4").Bold()}: {record.Key}, {record.Value.ReadData()}");
                }
            }
        }
    }
}
