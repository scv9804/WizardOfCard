using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/aether")]
public class AetherValueChange : ItemEffect
{
    public int Min_AetherPoint = 0;
    public int Max_AetherPoint = 0;

    public override bool ExcuteRole()
    {
        int AetherPoint = Random.Range(Min_AetherPoint, Max_AetherPoint);

        PlayerEntity.Inst.Status_Aether += AetherPoint;

        if(PlayerEntity.Inst.Status_Aether > PlayerEntity.Inst.Status_MaxAether)
        {
            PlayerEntity.Inst.Status_Aether = PlayerEntity.Inst.Status_MaxAether;
        }

        return true;
    }
}