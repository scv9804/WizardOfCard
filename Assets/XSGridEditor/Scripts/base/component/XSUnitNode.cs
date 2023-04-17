/// <summary>
/// @Author: xiaoshi
/// @Date: 2022/2/9
/// @Description: script added to unit gameobject
/// </summary>
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

namespace XSSLG
{
    /// <summary> unit data </summary>
    public class XSUnitNode : MonoBehaviour, XSIUnitNode
    {
        /// <summary> it is more common to represent id as a string </summary>
        [SerializeField]
        protected string id = "-1";
        public string Id { get => id; set => id = value; }

        protected bool is_attackable = false;

        public bool Is_attackable { get => is_attackable; set => is_attackable = value; }

        [SerializeField]
        protected int move = 6;
        public int Move { get => move; set => move = value; }

        public Dictionary<Vector3, List<Vector3>> CachedPaths { get; protected set; }

        public Vector3 WorldPos { get => this.transform.position; set => this.transform.position = value; }

        public virtual void AddBoxCollider()
        {
            var collider = this.gameObject.AddComponent<BoxCollider>();
            var bounds = this.GetMaxBounds();
            collider.bounds.SetMinMax(bounds.min, bounds.max);
            collider.center = collider.transform.InverseTransformPoint(bounds.center);
            collider.size = bounds.size;
        }
        
        protected virtual Bounds GetMaxBounds()
        {
            var renderers = this.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds();
            }

            var ret = renderers[0].bounds;
            foreach (Renderer r in renderers)
            {
                ret.Encapsulate(r.bounds);
            }
            
            return ret;
        }

        /// <summary>
        /// get the unit move region
        /// </summary>
        /// <returns></returns>
        public virtual List<Vector3> GetMoveRegion()
        {
            var gridMgr = XSInstance.Instance.GridMgr; // �̰� ������ �������ݾ�
            gridMgr.GetXSTile(this.transform.position, out var srcTile); //�̰ɷ� ĳ���� ���� Ÿ�� �� �ޱ� ����
            // first cache
            this.CachedPaths = gridMgr.FindAllPath(srcTile, Move); // �����ġ�� ���� ����
                                                                        // Accumulate this.CachedPaths
            var ret = this.CachedPaths.Aggregate(new List<Vector3>(), (ret, pair) =>
            {
                ret.AddRange(pair.Value.Distinct());
                return ret;
            }).Distinct().ToList(); // �̰� �ߺ� ����
            return ret;
        }


        //���� ������ �������� �̰ɷ� ��ġ ���� �� ���� ���� �����ϵ��� �غ�
        public virtual List<Vector3> GetAttackRegionTest_00()
		{
            var gridMgr = XSInstance.Instance.GridMgr;
            gridMgr.GetXSTile(this.transform.position, out var srcTile);
            // first cache
            this.CachedPaths = gridMgr.FindAllPath(srcTile, 1);// ���⿡ ���� ���� ��ġ ��Ե� �Է��ϸ� �� �� ����
            // ������ �� �ִ� ��.
            var ret = this.CachedPaths.Aggregate(new List<Vector3>(), (ret, pair) =>
            {
                // deduplication
                ret.AddRange(pair.Value.Distinct());
                return ret;
            }).Distinct().ToList(); // deduplication
            return ret;
        }

        public virtual List<Vector3> playerRegionRoute()
        {
            var gridMgr = XSInstance.Instance.GridMgr;
            gridMgr.GetXSTile(this.WorldPos, out var srcTile);
            //gridMgr.GetXSTile(EntityManager.Inst.playerEntity.transform.position, out var playerTile);

            gridMgr.GetXSTile(PlayerEntity.Inst.WorldPos , out var playerTile); // ��ġ��        

			var sum = Mathf.Abs(playerTile.TilePos.x - srcTile.TilePos.x) + Mathf.Abs(playerTile.TilePos.y - srcTile.TilePos.y);

            this.CachedPaths = gridMgr.FindAllPath(srcTile, sum);// ���⿡ ���� ���� ��ġ ��Ե� �Է��ϸ� �� �� ����
 
            var ret = this.CachedPaths.Aggregate(new List<Vector3>(), (ret, pair) =>
            {
                // deduplication
                ret.AddRange(pair.Value.Distinct());
                return ret;
            }).Distinct().ToList(); // deduplication

            return ret;
        }

        public virtual void RemoveNode() => XSUnityUtils.RemoveObj(this.gameObject);

        public virtual bool IsNull() => this == null;

        
        public virtual void UpdatePos()
        {
            XSInstance.Instance.GridHelper.SetTransToTopTerrain(this.transform, true);
        }
    }
}
