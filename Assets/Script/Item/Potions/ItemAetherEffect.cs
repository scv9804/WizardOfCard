using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/aether")]
public class ItemAetherEffect : ItemEffect
{
    public int AetherPoint = 0;

    public override bool ExcuteRole()
    {
        int CurrentAetherPoint = PlayerEntity.Inst.Status_Aether + AetherPoint;

        PlayerEntity.Inst.Status_Aether = CurrentAetherPoint <= PlayerEntity.Inst.Status_MaxAether ? CurrentAetherPoint : PlayerEntity.Inst.Status_MaxAether;

        return true;
    }
}