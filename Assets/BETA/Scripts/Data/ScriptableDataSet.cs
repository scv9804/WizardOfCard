using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // =========================================================================== ScriptableDataSet

    public abstract class ScriptableDataSet<TScriptableData> : SerializedScriptableObject where TScriptableData : ScriptableData
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [TableList]
        public TScriptableData[] Data;
    }
}