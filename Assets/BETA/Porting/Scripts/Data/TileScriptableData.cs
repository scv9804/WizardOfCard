using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;

using Sirenix.OdinInspector;

using UnityEngine.Tilemaps;

namespace BETA.Porting
{
    // ==================================================================================================== TileScriptableData

    [CreateAssetMenu(menuName = "BETA/Porting/Tile/Data")]
    public sealed class TileScriptableData : ScriptableData
    {
        // ==================================================================================================== Field

        // =========================================================================== Tile

        [FoldoutGroup("타일")]
        public List<TileBase> TileBase;

        // =========================================================================== General

        [FoldoutGroup("일반 데이터")]
        public bool HasTooltip;

        [MultiLineProperty(10)] [FoldoutGroup("일반 데이터")]
        public string Description;

        // =========================================================================== Effect

        [FoldoutGroup("효과 데이터")]
        public TileType type = TileType.TRAVERSABLE;

        [FoldoutGroup("효과 데이터")]
        public int Cost = 1;

        //[FoldoutGroup("효과 데이터")]
        //public TacticsToolkit.ScriptableEffect effect;
    }
}
