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

        [FoldoutGroup("Ÿ��")]
        public List<TileBase> TileBase;

        // =========================================================================== General

        [FoldoutGroup("�Ϲ� ������")]
        public bool HasTooltip;

        [MultiLineProperty(10)] [FoldoutGroup("�Ϲ� ������")]
        public string Description;

        // =========================================================================== Effect

        [FoldoutGroup("ȿ�� ������")]
        public TileType type = TileType.TRAVERSABLE;

        [FoldoutGroup("ȿ�� ������")]
        public int Cost = 1;

        //[FoldoutGroup("ȿ�� ������")]
        //public TacticsToolkit.ScriptableEffect effect;
    }
}
