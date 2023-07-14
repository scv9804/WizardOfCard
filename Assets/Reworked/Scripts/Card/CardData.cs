using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Reworked
{
    // ==================================================================================================== Card.Data

    public partial class Card
    {
        [Serializable] public sealed class Data
        {
            // ==================================================================================================== Field

            // =========================================================================== Identifier

            [Header("원본 ID")]
            public int SerialID;

            // =========================================================================== Status

            // ================================================== Base

            [Header("이름")]
            public string Name;

            [Header("강화 횟수")]
            [Range(0, 2)] public int Level;

            [Header("비용")]
            public int Cost;

            [Header("설명")]
            public string Description;

            // ==================================================================================================== Method

            // =========================================================================== Instance

            public static Data Create(string instanceID)
            {
                if (Cache.Data.ContainsKey(instanceID))
                {
                    #region #if UNITY_EDITOR => Debug.LogWarning();
#if UNITY_EDITOR
                    Debug.LogWarning($"{instanceID} Has Already Allocated");
#endif
                    #endregion

                    return Cache.Data[instanceID];
                }

                var data = new Data();

                Cache.Data.Add(instanceID, data);

                return data;
            }

            public static Data Create(string instanceID, int serialID)
            {
                if (Cache.Data.ContainsKey(instanceID))
                {
                    #region #if UNITY_EDITOR => Debug.LogWarning();
#if UNITY_EDITOR
                    Debug.LogWarning($"{instanceID} Has Already Allocated");
#endif
                    #endregion

                    return Cache.Data[instanceID];
                }

                var data = new Data();

                Cache.Data.Add(instanceID, data);

                return data;
            }
        }
    }
}
