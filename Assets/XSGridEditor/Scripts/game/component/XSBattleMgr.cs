/// <summary>
/// @Author: xiaoshi
/// @Date: 2022/2/2
/// @Description: Demo_1 manager
/// </summary>
using System;
using System.Linq;
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
        public bool IsMoving { get; private set; } = false;

        public bool IsEnemyMoving { get; set; }

        public List<Vector3> MoveRegion { get; set; }

        public XSUnitNode SelectedUnit { get; set; }


		#region 참조하지 않을 코드들
		bool isEnemyAttacking = true;

        GameObject[] units;

        public List<Vector3> mouseVector;

        bool SelectTile= false;

        #endregion
        /*
                /// <summary>
                /// 일단 테스트용
                /// 아래거를 리턴해서 범위 값을 카드에서 먼저 보내주고
                /// 범위 클릭하면 유닛비교해서 유닛 리스트 리턴해주기
                /// </summary>
                [Serializable]
                public class _2dArray
                {
                    public List<int> arr;
                }
                public List<_2dArray> array;


                public Dictionary<int ,List<Vector3> > dictest = new Dictionary<int, List<Vector3>>();*/// 생각해보니까 그냥 벡터로받으먄됨 ㅋ


        void Start()
        {
            if (XSUnityUtils.IsEditor())
            {
                this.enabled = false;
            }
            else
            {
                units = GameObject.FindGameObjectsWithTag("Enemy");
                this.GridMgr = XSInstance.Instance.GridMgr;
                var gridHelper = XSInstance.Instance.GridHelper;
                if (gridHelper)
                {
                    this.xsCamera.SetConfinerBound(gridHelper.GetBounds());
                }

                this.UnitMgr = new XSUnitMgr(gridHelper);

                var moveRegionCpt = XSGridShowRegionCpt.Create(XSGridDefine.SCENE_GRID_MOVE, gridHelper.MoveTilePrefab, 10);
                this.GridShowMgr = new XSGridShowMgr(moveRegionCpt);
                SetEntityDic();

              
            }
            TurnManager.enemyActions += SetEnemyAttack;
            TurnManager.onStartTurn += SelectUnitClear;
        }

		void SelectUnitClear(bool _myTurn)
        {
            SelectedUnit = null;
        }

        // Update is called once per frame
        void Update()
        {
            /************************* 움직임 제어문  ***********************/
            if (!this.IsMoving)
            {
                IsEnemyMoving = false;
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
                                if (tile.IsEntity == false)
                                {
                                    if (this.MoveRegion.Contains(tile.WorldPos))
                                    {
                                        GridMgr.GetXSTile(new Vector3(SelectedUnit.WorldPos.x, 0, SelectedUnit.WorldPos.z), out var nowXStile);
                                        nowXStile.IsEntity = false;
                                        this.GridShowMgr.ClearMoveRegion();
                                        this.MoveRegion = null;
                                        // cache
                                        if (this.SelectedUnit.CachedPaths != null && this.SelectedUnit.CachedPaths.ContainsKey(tile.WorldPos))
                                        {

                                            this.WalkTo(this.SelectedUnit.CachedPaths[tile.WorldPos]);//움직임
                                            tile.IsEntity = true;
                                            this.SelectedUnit.Is_attackable = true;
                                        }
                                        else
                                        {
                                            this.SelectedUnit = null;
                                        }
                                    }
                                }
                            }
                            /*else
                            {
                                var tile = XSUG.GetMouseTargetTile();

                                if (this.MoveRegion.Contains(tile.WorldPos))
                                {                                    
                                    this.GridShowMgr.ClearMoveRegion();
                                    this.MoveRegion = null;
                                    // 공격
                                    if (this.SelectedUnit.CachedPaths != null && this.SelectedUnit.CachedPaths.ContainsKey(tile.WorldPos))
                                    {
                                        PlayerAttack(tile.WorldPos);
                                    }
                                    else
                                    {
                                        this.SelectedUnit = null;
                                    }
                                }
                            }*/
                        }
                        else
                        {
                            var unit = (XSUnitNode)XSUG.GetMouseTargetUnit();
                            if (unit != null && !unit.IsNull() && unit.Id == "0") 
                            {
                                this.SelectedUnit = unit;
                                if (!this.SelectedUnit.Is_attackable)
                                {
                                     Debug.Log("SelectedUnit: " + unit.name);
                                    this.MoveRegion = this.GridShowMgr.ShowMoveRegion(unit); // 위치 보여주기
                                }
                                /*else
                                {
                                    Debug.Log("SelectedUnit_Attack: " + unit.name);
                                    this.MoveRegion = this.GridShowMgr.ShowAttackRegion(unit); // 이 부분 수정해서 공격으로 수정가능 
                                }*/
                            }

                        }
                        /************************* 여기서 움직임 끝임  ***********************/
                    }
                    else if (Mouse.current.rightButton.wasPressedThisFrame) // 취소
                    {
                        if (this.SelectedUnit)
                        {
                            this.GridShowMgr.ClearMoveRegion();
                            this.SelectedUnit = null;
                        }
                    }
                }//내턴 종료시
                else
                {
                    Debug.Log(isEnemyAttacking);
					if (!isEnemyAttacking)
					{
                        StartCoroutine(EnemyBehavior());
                    }
                }
            }

        }


        /// <summary>
        /// move to the destination
        /// </summary>
        /// <param name="path">move path</param>
     
        public void SetEnemyAttack()
		{
            isEnemyAttacking = false;
		}
        

        public IEnumerator EnemyBehavior()
		{
            isEnemyAttacking = true;
            var units = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var temp in units)
            {
                var unit = (XSUnitNode)temp.GetComponent<XSIUnitNode>();

                //
                if (unit != null && !unit.IsNull())
                {
                    SelectedUnit = unit;
#if UNITY_EDITOR
                   // Debug.Log("Enemy_Moving_editer_ : " + SelectedUnit.Is_attackable);
#endif
                    if (SelectedUnit.Is_attackable && !IsEnemyMoving)
                    {
                        var cheackAttackRegion = GridShowMgr.ShowMoveRegion(unit);                        
#if UNITY_EDITOR
                        Debug.Log("if진입");
#endif
                        MoveRegion = unit.playerRegionRoute();

                        var tile = PlayerEntity.Inst;

                        IsEnemyMoving = true;
                        SelectedUnit.Is_attackable = false;

                        Vector3 temtVect = new Vector3(tile.WorldPos.x, 0, tile.WorldPos.z);

                        if (cheackAttackRegion.Contains(temtVect))
                        {
#if UNITY_EDITOR
                          Debug.Log("공격");
#endif
                            yield return new WaitForSeconds(0.3f);
                            GridShowMgr.ClearMoveRegion();

                            var entity = SelectedUnit.GetComponent<Entity>();

                            yield return StartCoroutine(entity.attack.AttackPattern(entity).DefultAttack(entity));
                            MoveRegion = null;
                            SelectedUnit = null;
                        }
                        else if (MoveRegion.Contains(temtVect))
                        {
                            GridShowMgr.ClearMoveRegion();
                            MoveRegion = null;

                            if (SelectedUnit.CachedPaths != null)
                                yield return StartCoroutine(WalkTo_Enemy(SelectedUnit.CachedPaths[temtVect], unit.Move));
                            else
                                SelectedUnit = null;
#if UNITY_EDITOR
                            Debug.Log("컨테인");
#endif
                        }
                        Debug.Log("끝");
                    }
                    IsEnemyMoving = false;
                   // SelectedUnit.Is_attackable = true;

                }

            }
            yield return new WaitForSeconds(0.5f);
            LevelGeneration.Inst.EndTurn();
        }

        //적 선택하는거 테스트
        public IEnumerator SelectTarget(WIP.CardTarget cardTarget , List<Vector3> radius , int range)
        {
            var unit = (XSUnitNode)GameObject.FindGameObjectWithTag("Player").GetComponent<XSIUnitNode>();
            this.MoveRegion = this.GridShowMgr.ShowAttackRegion(unit, range);

            //cardTarget.IsActive = false;

            while (true)
            {
                if (!this.SelectedUnit && TurnManager.Inst.myTurn)
                {
                    var tile = XSUG.GetMouseTargetTile();

					if (tile != null)
					{
                        //첫 타일 값 삽입.
                        mouseVector.Add(tile.WorldPos);

                        //타일 주변값 비교
                        if (!SelectTile)
                        {
                            SelectTile = true;
                            Debug.Log("타일 값 넣기");
                            foreach (var t in radius)
                            {
                                if (GridMgr.GetTileVect().Contains(tile.WorldPos + t))
                                {
                                    mouseVector.Add(tile.WorldPos + t);
                                }
                            }
                        }
                        //타일위치 바뀌었는지 확인하기
                        if (tile.WorldPos != mouseVector[0])
                        {
                            SelectTile = false;
                            GridShowMgr.ClearMoveRegion();
                            mouseVector.Clear();
                        }

                        try
                        {
                            GridShowMgr.MoveShowRegion.ShowRegion(mouseVector);
                            this.GridShowMgr.ShowAttackRegion(unit, range);
                        }
                        catch
                        {
#if UNITY_EDITOR
                            Debug.Log("마우스 맵 밖에 존재함");
#endif
                        }


                        if (Mouse.current.leftButton.wasPressedThisFrame) //클릭하면 리턴임!!!!!!!!!!!!!!!!!
                        {
                            if (this.MoveRegion.Contains(mouseVector[0]))
                            {
                                foreach (var vect in mouseVector.Distinct())
                                {
                                    GridMgr.GetEntityInPos(vect, out var entity);
                                    if (entity != null)
                                    {
                                        cardTarget.Targets.Add(entity);
                                    }
                                }
                                cardTarget.IsActive = true;
                                GridShowMgr.ClearMoveRegion();
                                mouseVector.Clear();
                                break;
                            }
                            // <2023-06-09 장형용 :: 추가>
                            else
                            {
                                yield break;
                            }
                        }
                    }                
                }
                yield return null;
            }
            yield return null;
        }


        //리스트 값 리턴 예제
        /*Entity ns;
        StartCoroutine(test(n => { ns = n; }));
        
        */


        public void WalkTo(List<Vector3> path)
        {
            if (this.movementAnimationSpeed > 0)
            {
                StartCoroutine(MovementAnimation(path));
            }
        }

        public IEnumerator WalkTo_Enemy(List<Vector3> path, int move)
        {
            if (this.movementAnimationSpeed > 0)
            {
                yield return StartCoroutine(MovementAnimation_Enemy(path, move));
            }
        }

        // 플레이어 공격
        public void PlayerAttack(Vector3 AttackPos)
        {
            foreach (var unit in units)
			{      
                if (unit.transform.position.x == AttackPos.x && unit.transform.position.z == AttackPos.z)
				{                 
                    unit.GetComponent<Entity>().Damaged(10, null) ;
				}
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


		public void MoveSetTileExit(XSTile tile)
		{
            tile.Access.Down = true;
            tile.Access.Up = true;
            tile.Access.Left = true;
            tile.Access.Right = true;
		}
        private void MoveSetTileEnter(XSTile tile)
        {
            tile.Access.Down = false;
            tile.Access.Up = false;
            tile.Access.Left = false;
            tile.Access.Right = false;
        }



        public virtual IEnumerator MovementAnimation_Enemy(List<Vector3> path, int move)
        {
            this.IsMoving = true;
            path.Reverse(); // reverse the path

            GridMgr.GetXSTile(new Vector3(SelectedUnit.WorldPos.x, 0, SelectedUnit.WorldPos.z), out var nowXStile_1);
           
            nowXStile_1.IsEntity = false;

            MoveSetTileExit(nowXStile_1);

            var temtPos = new Vector3(SelectedUnit.WorldPos.x, 0, SelectedUnit.WorldPos.z);

            for (int i = 0; i < move; i++)
            {
                while (this.SelectedUnit.transform.position != path[i])
                {
                    if (GridMgr.IsEntityXSTile(path[i]))
                    {
                        this.SelectedUnit.transform.position = Vector3.MoveTowards(this.SelectedUnit.transform.position, path[i], Time.deltaTime * movementAnimationSpeed);
					}
					else
					{
                        break;
					}
                    yield return 0;
                }
            }

            GridMgr.EntityDicRefresh(temtPos, new Vector3(SelectedUnit.WorldPos.x, 0, SelectedUnit.WorldPos.z) , SelectedUnit.gameObject.GetComponent<Entity>());

            GridMgr.GetXSTile(new Vector3(SelectedUnit.WorldPos.x, 0, SelectedUnit.WorldPos.z), out var nowXStile_2);

            MoveSetTileEnter(nowXStile_2);
            nowXStile_2.IsEntity = true;

            this.SelectedUnit = null;
            this.IsMoving = false;
        }

        public void SetEntityDic()
		{
            foreach (var entity in EntityManager.Inst.enemyEntities.Distinct())
            {
#if UNITY_EDITOR
                Debug.Log("엔티티 딕셔너리에 넣었습니다.");
#endif
                GridMgr.GetXSTile(new Vector3(entity.transform.position.x, 0, entity.transform.position.z), out var nowXStile_1);
                MoveSetTileEnter(nowXStile_1);
                nowXStile_1.IsEntity = true;
                GridMgr.EntityDicAdd(new Vector3(entity.transform.position.x, 0, entity.transform.position.z), entity);
                
            }
        }

        /*        public virtual IEnumerator Setbool()
                {
                    this.SelectedUnit = null;
                    this.IsMoving = false;

                    IsEnemyMoving = false;
                }

                public virtual IEnumerator EnemyPattern()
                {

                }*/


    }
}
