using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // ==================================================================================================== Range

    [CreateAssetMenu(menuName = "BETA/Range")]
    public sealed class Range : SerializedScriptableObject
    {
        [TableMatrix(SquareCells = true)]
        public int[,] Matrix;
    } 
}
