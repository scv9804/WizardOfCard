using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/health")]
public class HealthValueChange : ItemEffect
{
    public int Min_HealthPoint = 0;
    public int Max_HealthPoint = 0;

    public override bool ExcuteRole()
	{
        int HealthPoint = Random.Range(Min_HealthPoint, Max_HealthPoint);

        PlayerEntity.Inst.Status_Health += HealthPoint;

        if (PlayerEntity.Inst.Status_Health > PlayerEntity.Inst.Status_MaxHealth)
        {
            PlayerEntity.Inst.Status_Health = PlayerEntity.Inst.Status_MaxHealth;
        }
        //else if(PlayerEntity.Inst.Status_Health < 0)
        //{
        //    PlayerEntity.Inst.Status_Health = 1;
        //} 자살 방지 코드

        return true;
    }
}
