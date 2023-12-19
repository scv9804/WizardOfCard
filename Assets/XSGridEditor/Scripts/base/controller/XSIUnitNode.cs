/// <summary>
/// @Author: xiaoshi
/// @Date: 2022-08-19 13:34:19
/// @Description: interface to XSUnitNode
/// </summary>
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;

namespace XSSLG
{
    public interface XSIUnitNode : XSINode
    {

        List<Vector3> GetMoveRegion();
        
        List<Vector3> GetAttackRange(int range);

        List<Vector3> GetAttackRegionTest_00();

        List<Vector3> playerRegionRoute();

        void UpdatePos();
    }
}