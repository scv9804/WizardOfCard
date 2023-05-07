using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;

[CreateAssetMenu(fileName = "EnemyTest", menuName = "TestModules/EnemyAttack")]
public class EnemyAttackTest2 : EnemyAttackTest
{
    public override IEnumerator attackNewOne(XSBattleMgr mgr)
    {
        mgr.isEnemyAttacking = true;
        //var unit = (XSUnitNode)GameObject.FindGameObjectWithTag("Enemy").GetComponent<XSIUnitNode>();
        var units = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var temp in units)
        {
            var unit = (XSUnitNode)temp.GetComponent<XSIUnitNode>();

            //이거 적 공격 트리
            if (unit != null && !unit.IsNull())
            {
                mgr.SelectedUnit = unit;
#if UNITY_EDITOR
                Debug.Log("Enemy_Moving_editer_ : " + mgr.SelectedUnit.Is_attackable);
#endif
                if (mgr.SelectedUnit.Is_attackable && !mgr.IsEnemyMoving)
                {
                    var cheackAttackRegion = mgr.GridShowMgr.ShowMoveRegion(unit); // 위치 보여주기

#if UNITY_EDITOR
                    Debug.Log("if문 진입");
#endif
                    mgr.MoveRegion = unit.playerRegionRoute();

                    var tile = PlayerEntity.Inst;

                    mgr.IsEnemyMoving = true;
                    mgr.SelectedUnit.Is_attackable = false;

                    Vector3 temtVect = new Vector3(tile.WorldPos.x, 0, tile.WorldPos.z);

                    if (cheackAttackRegion.Contains(temtVect))
                    {
                        EntityManager.Inst.playerEntity.Status_Health -= 1;
#if UNITY_EDITOR
                        Debug.Log("공격진입");
#endif
                        mgr.GridShowMgr.ClearMoveRegion();
                        yield return new WaitForSeconds(0.3f);
                        mgr.MoveRegion = null;
                        mgr.SelectedUnit = null;
                    }
                    else if (mgr.MoveRegion.Contains(temtVect))
                    {
                        mgr.GridShowMgr.ClearMoveRegion();
                        mgr.MoveRegion = null;

                        if (mgr.SelectedUnit.CachedPaths != null)
                            yield return TurnManager.Inst.StartCoroutine(mgr.WalkTo_Enemy(mgr.SelectedUnit.CachedPaths[temtVect], unit.Move));
                        else
                            mgr.SelectedUnit = null;
#if UNITY_EDITOR
                        Debug.Log("컨테인진입");
#endif
                    }
                }
            }

        }
    }
}
