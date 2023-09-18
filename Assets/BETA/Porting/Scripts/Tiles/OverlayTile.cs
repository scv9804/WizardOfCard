using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using Sirenix.OdinInspector;

namespace BETA.Porting
{
    // ==================================================================================================== OverlayTile

    public sealed class OverlayTile : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        // =========================================================================== A*

        private int _g;
        private int _h;

        private int _accumulativeCost;

        private int _remainCost;

        private OverlayTile _previous;

        // =========================================================================== Instance

        private int _ID;

        // =========================================================================== Tile

        private bool _isBlocked = false;

        private bool _isFocused;

        private Vector3Int _location;

        private TacticsToolkit.Entity _entity;

        // =========================================================================== Component

        // ================================================== SpriteRenderer

        [FoldoutGroup("스프라이트 렌더러")]
        public SpriteRenderer TileRenderer;

        [FoldoutGroup("스프라이트 렌더러")]
        public SpriteRenderer ArrowRenderer;

        // ==================================================================================================== Property

        // =========================================================================== A*

        public int G
        {
            get
            {
                return _g;
            }

            set
            {
                _g = value;
            }
        }

        public int H
        {
            get
            {
                return _h;
            }

            set
            {
                _h = value;
            }
        }

        public int F
        {
            get
            {
                return G + H;
            }
        }

        //public int Cost
        //{
        //    get
        //    {
        //        return IsAvailable() ? Data.Cost : 1;
        //    }
        //}

        public int AccumulativeCost
        {
            get
            {
                return _accumulativeCost;
            }

            set
            {
                _accumulativeCost = value;
            }
        }

        public int RemainCost
        {
            get
            {
                return _remainCost;
            }

            set
            {
                _remainCost = value;
            }
        }

        public OverlayTile Previous
        {
            get
            {
                return _previous;
            }

            set
            {
                _previous = value;
            }
        }

        // =========================================================================== Instance

        public int ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        // =========================================================================== Tile

        public bool IsBlocked
        {
            get
            {
                return _isBlocked;
            }

            set
            {
                _isBlocked = value;
            }
        }

        public bool IsFocused
        {
            get
            {
                return _isFocused;
            }

            set
            {
                _isFocused = value;
            }
        }

        public Vector2Int Location2D
        {
            get
            {
                return new Vector2Int(_location.x, _location.y);
            }
        }

        public Vector3Int Location3D
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
            }
        }

        public TacticsToolkit.Entity Entity
        {
            get
            {
                return _entity;
            }

            set
            {
                _entity = value;
            }
        }

        // =========================================================================== Data

        //public TileScriptableData Data
        //{
        //    get
        //    {
        //        return IsAvailable() ? DataManager.Instance.Tiles.Data[ID] : null;
        //    }
        //}

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            //AccumulativeCost = Cost;
        }

        // =========================================================================== Overlay

        public void ShowTile(Color color)
        {
            TileRenderer.color = color;
        }

        public void HideTile()
        {
            TileRenderer.color = new Color(1, 1, 1, 0);

            SetArrowOverlay(Arrow.NONE);
        }

        // =========================================================================== Arrow

        public void SetArrowOverlay(Arrow arrow)
        {
            if (arrow == Arrow.NONE)
            {
                ArrowRenderer.color = new Color(1, 1, 1, 0);
            }
            else
            {
                ArrowRenderer.color = new Color(1, 1, 1, 1);

                var index = arrow.GetSpriteIndex();

                if (index > 0)
                {
                    //ArrowRenderer.sprite = DataManager.Instance.Arrow.Sprite[index];
                }
            }
        }

        // =========================================================================== Data

        //private bool IsAvailable()
        //{
        //    return ID > -1 && ID < DataManager.Instance.Tiles.Data.Length;
        //}
    }
}
