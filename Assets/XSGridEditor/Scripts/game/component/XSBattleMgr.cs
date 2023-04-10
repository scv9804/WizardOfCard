/// <summary>
/// @Author: xiaoshi
/// @Date: 2022/2/2
/// @Description: Demo_1 manager
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XSSLG
{
    /// <summary> Demo_1 manager </summary>
    public class XSBattleMgr : MonoBehaviour
    {
        protected int movementAnimationSpeed = 3;

        [SerializeField]
        protected XSCamera xsCamera;

        public bool isEnemyWork { get; set; }

        public XSIGridMgr GridMgr { get; set; }

        public XSUnitMgr UnitMgr { get; protected set; }

        public XSGridShowMgr GridShowMgr { get; set; }

        /// <summary> unit is moving </summary>
        public bool IsMoving { get; private set; }
        

        public List<Vector3> MoveRegion { get; private set; }

        protected XSUnitNode SelectedUnit { get; set; }

        void Start()
        {
            if (XSUnityUtils.IsEditor())
            {
                this.enabled = false;
            }
            else
            {
                this.GridMgr = XSInstance.Instance.GridMgr;
                var gridHelper = XSInstance.Instance.GridHelper;
                if (gridHelper)
                {
                    this.xsCamera.SetConfinerBound(gridHelper.GetBounds());
                }

                this.UnitMgr = new XSUnitMgr(gridHelper);

                var moveRegionCpt = XSGridShowRegionCpt.Create(XSGridDefine.SCENE_GRID_MOVE, gridHelper.MoveTilePrefab, 10);
                this.GridShowMgr = new XSGridShowMgr(moveRegionCpt);
            }
        }

        // Update is called once per frame
        void Update()
        {
            /************************* ������ ���  ***********************/
            if (!this.IsMoving)
            {
				if (TurnManager.Inst.myTurn)
				{
                    // click mouse left buton to move
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        if (this.SelectedUnit)
                        {
                            if (!this.SelectedUnit.Is_attackable) 
                            {
                                var tile = XSUG.GetMouseTargetTile();
                                Debug.Log("tilePos: " + tile.TilePos);
                                // tile must be in the move range
                                if (this.MoveRegion.Contains(tile.WorldPos))
                                {
                                    this.GridShowMgr.ClearMoveRegion();
                                    this.MoveRegion = null;
                                    // cache
                                    if (this.SelectedUnit.CachedPaths != null && this.SelectedUnit.CachedPaths.ContainsKey(tile.WorldPos))
                                    {
                                        this.WalkTo(this.SelectedUnit.CachedPaths[tile.WorldPos]);//������
                                        this.SelectedUnit.Is_attackable = true;
                                    }
                                    else
                                    {
                                        this.SelectedUnit = null;
                                    }
                                }
							}
							else      //���� �ӽ� Test
							{
                                var tile = XSUG.GetMouseTargetTile();
                                if (this.MoveRegion.Contains(tile.WorldPos))
                                {
                                    this.GridShowMgr.ClearMoveRegion();
                                    this.MoveRegion = null;
                                    // cache
                                    if (this.SelectedUnit.CachedPaths != null && this.SelectedUnit.CachedPaths.ContainsKey(tile.WorldPos))
                                    {
                                        this.WalkTo(this.SelectedUnit.CachedPaths[tile.WorldPos]);
                                    }
                                    else
                                    {
                                        this.SelectedUnit = null;
                                    }
                                }
                            }                           
                        }
                        else
                        {
                            var unit = (XSUnitNode)XSUG.GetMouseTargetUnit();
                            if (unit != null && !unit.IsNull() && unit.Id == "0") // ���� �ٲ��ּ��� ��������...!!
                            {
                                this.SelectedUnit = unit;
                                if (!this.SelectedUnit.Is_attackable)
                                {
                                    Debug.Log("SelectedUnit: " + unit.name);
                                    this.MoveRegion = this.GridShowMgr.ShowMoveRegion(unit); // ��ġ �����ֱ�
								}
								else
								{
                                    Debug.Log("SelectedUnit_Attack: " + unit.name);
                                    this.MoveRegion = this.GridShowMgr.ShowAttackRegion(unit); // �� �κ� �����ؼ� �������� �������� 
                                }                         
                            }

                        }
                        /************************* deal unit select, and move  end  ***********************/
                    }
                    else if (Mouse.current.rightButton.wasPressedThisFrame) // ���
                    {
                        if (this.SelectedUnit)
                        {
                            this.GridShowMgr.ClearMoveRegion();
                            this.SelectedUnit = null;
                        }
                    }
				}
				else
				{
                    var unit = (XSUnitNode)GameObject.FindGameObjectWithTag("Enemy").GetComponent<XSIUnitNode>();
                    if (unit != null && !unit.IsNull()) // ���� �ٲ��ּ��� ��������...!!
                    {
                        this.SelectedUnit = unit;
                        Debug.Log(unit);
                        if (!this.SelectedUnit.Is_attackable)
                        {
                            Debug.Log("SelectedUnit: " + unit.name);
                            this.MoveRegion = this.GridShowMgr.ShowMoveRegion(unit); // ��ġ �����ֱ�

                            SelectedUnit.Is_attackable = true;

                            var tile = XSUG.GetMouseTargetTile();
                            if (this.MoveRegion.Contains(tile.WorldPos))
                            {
                                this.GridShowMgr.ClearMoveRegion();
                                this.MoveRegion = null;
                            }
                            if (this.SelectedUnit.CachedPaths != null && this.SelectedUnit.CachedPaths.ContainsKey(tile.WorldPos))
                            {
                                this.WalkTo_Enemy(this.SelectedUnit.CachedPaths[tile.WorldPos]);
                            };

                        }
                    }
                }
            }
              
        }




        /// <summary>
        /// ���� �޼���
        /// </summary>
        /// <param name = "Attack">attack range</param>>
        public void AttackTo(List<Vector3> range)
        {
 
        }


        /// <summary>
        /// move to the destination
        /// </summary>
        /// <param name="path">move path</param>
        public void WalkTo(List<Vector3> path)
        {
            if (this.movementAnimationSpeed > 0)
            {
                StartCoroutine(MovementAnimation(path));
            }
        }

        public void WalkTo_Enemy(List<Vector3> path)
        {
            if (this.movementAnimationSpeed > 0)
            {
                StartCoroutine(MovementAnimation_Enemy(path));
            }
        }


        /// <summary> Coroutine to deal move </summary>
        public virtual IEnumerator MovementAnimation(List<Vector3> path)
        {
            this.IsMoving = true;
            path.Reverse(); // reverse the path
            foreach (var pos in path)
            {
                while (this.SelectedUnit.transform.position != pos)
                {
                    this.SelectedUnit.transform.position = Vector3.MoveTowards(this.SelectedUnit.transform.position, pos, Time.deltaTime * movementAnimationSpeed);
                    yield return 0;
                }
            }
            this.SelectedUnit = null;
            this.IsMoving = false;
        }

        public virtual IEnumerator MovementAnimation_Enemy(List<Vector3> path)
        {
            this.IsMoving = true;
            path.Reverse(); // reverse the path
            foreach (var pos in path)
            {
                while (this.SelectedUnit.transform.position != pos)
                {
                    this.SelectedUnit.transform.position = Vector3.MoveTowards(this.SelectedUnit.transform.position, pos, Time.deltaTime * movementAnimationSpeed);
                    yield return 0;
                }
            }
            this.SelectedUnit = null;
            this.IsMoving = false;
        }
    }
}
