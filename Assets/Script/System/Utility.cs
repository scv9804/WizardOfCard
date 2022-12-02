using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Jobs;
using Unity.Collections;

[Serializable] public class Pos_Rot_Scale
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public Pos_Rot_Scale(Vector3 _pos, Quaternion _rot, Vector3 _scale)
    {
        pos = _pos;
        rot = _rot;
        scale = _scale;
    }
}

public class Utility
{
    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
            return result;
        }
    }

    //확률 뽑기
    public static float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = UnityEngine.Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    public static Action onBattleStart; // 전투 방 진입 시 호출

    public static Action<Card> onCardUsed; // 카드 사용 시 호출
    public static Action<Card, int> onDamaged; // 카드로 데미지를 입힐 시 호출
}

// << 22-11-12 장형용 :: 클래스 제거>>
//public static class Utility_enum

#region Enums

//public enum ItemType
//{
//    Use,
//    Equi,
//    Quest
//}

public enum AttackRange
{
    Target_AllEnemy,
    Target_Self,
    Target_Single
}

// <<22-11-24 장형용 :: 수정>>
[Flags] public enum CardType
{
    ATTACK  = 1 << 1,
    DEFENCE = 1 << 2,
    BUFF    = 1 << 3,
    DEBUFF  = 1 << 4,
    DRAW    = 1 << 5,
    UTILITY = 1 << 6,
}

public enum CardEnum
{
    마법구,
    보호막,
    화염구,
    불안정한_고서,
    지혜,
    마력_폭격,
    마력창,
    긴급_회복,
    화염_폭풍,
    연쇄_번개,
    마나_가속_방패,
    비전_광선,
    붕괴,
    벼락,
    폭발,
    융해,
    과부하,
    집중_포화,
    침착,
    물의_룬,
    불의_룬,
    폭풍의_룬,
    대지의_룬,
    역장,
    마나_환원,
    숨_고르기,
    강타,
    쉴드_폭발,
    마력포,
    마법_재활용,
    마력_집중,
    연속_공격,
    마법진,
    디버그용
}

public enum E_CardStats
{ 
    Cannot, 
    CanMouseOver,
    CanAll
}

#endregion

#region Custom Yield Instructions

// <<22-10-30 장형용 :: 모든 카드 사용과 데미지 처리가 끝날때까지 대기시켜주는 클래스>>
public class WaitAllCardUsingDone : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get { return CardManager.i_usingCardCount > 0 || EntityManager.i_checkingEntitiesCount > 0; }
    }
}

// <<22-10-30 장형용 :: 해당 Entity의 피격 모션 처리가 끝날때까지 대기시켜주는 클래스>>
// <<22-11-09 장형용 :: 삭제>>
//public class WaitForAllMotionDone : CustomYieldInstruction

#endregion

#region Job System

#region Entities

// <<22-11-12 장형용 :: 보호 계산 Job>>
public struct ProtectionJob : IJob
{
    public NativeArray<int> values; // 0: Damage, 1: Protection

    public bool isPrint;

    public void Execute()
    {
        #region 디버그

        if (isPrint)
        {
            Debug.Log("보호 연산 전 데미지 : " + values[0]);
            Debug.Log("보호 연산 전 보호 : " + values[1]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        if (values[1] >= values[0])
        {
            values[0] = 0;
        }

        if (values[1] > 0)
            values[1]--;

        #region 디버그

        if (isPrint)
        {
            Debug.Log("보호 연산 후 데미지 : " + values[0]);
            Debug.Log("보호 연산 후 보호 : " + values[1]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

// <<22-11-12 장형용 :: 쉴드 계산 Job>>
public struct ShieldJob : IJob
{
    public NativeArray<int> values; // 0: Damage, 2: Shield, 3: Health

    public bool isPrint;

    public void Execute()
    {
        #region 디버그

        if (isPrint)
        {
            Debug.Log("쉴드 연산 전 데미지 : " + values[0]);
            Debug.Log("쉴드 연산 전 쉴드 : " + values[2]);
            Debug.Log("쉴드 연산 전 체력 : " + values[3]);
            Debug.Log("-------------------------------------");
        }
        #endregion

        if (values[2] > values[0])
        {
            values[2] -= values[0];
            values[0] = 0;
        }
        else
        {
            values[0] -= values[2];
            values[2] = 0;

            values[3] -= values[0];
        }

        #region 디버그

        if (isPrint)
        {
            Debug.Log("쉴드 연산 후 데미지 : " + values[0]);
            Debug.Log("쉴드 연산 후 쉴드 : " + values[2]);
            Debug.Log("쉴드 연산 후 체력 : " + values[3]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

// <<22-11-12 장형용 :: 화상 계산 Job>>
public struct BurningJob : IJob
{
    public NativeArray<int> values; // 2: Shield, 3: Health, 4: Burning

    public bool isPrint;

    public void Execute()
    {
        #region 디버그

        if (isPrint)
        {
            Debug.Log("화상 연산 전 쉴드 : " + values[2]);
            Debug.Log("화상 연산 전 체력 : " + values[3]);
            Debug.Log("화상 연산 전 화상 : " + values[4]);
            Debug.Log("-------------------------------------");
        }
        #endregion

        if (values[2] > values[4])
        {
            values[2] -= values[4];
        }
        else
        {
            values[3] += (values[2] - values[4]);

            values[2] = 0;
        }

        if (values[4] > 0)
            values[4]--;

        #region 디버그

        if (isPrint)
        {
            Debug.Log("화상 연산 후 쉴드 : " + values[2]);
            Debug.Log("화상 연산 후 체력 : " + values[3]);
            Debug.Log("화상 연산 후 화상 : " + values[4]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

#endregion

#region Cards

// <<22-11-12 장형용 :: 마나 친화성 계산 Job>>
public struct MagicAffinityJob : IJob
{
    public NativeArray<int> value;

    public int magicAffinity;

    public void Execute()
    {
        value[0] += magicAffinity;

        if (value[0] < 0)
            value[0] = 0;
    }
}

// <<22-11-12 장형용 :: 마나 저항력 계산 Job>>
public struct MagicResistanceJob : IJob
{
    public NativeArray<int> value;

    public int magicResistance;

    public void Execute()
    {
        value[0] += magicResistance;

        if (value[0] < 0)
            value[0] = 0;
    }
}

// <<22-11-12 장형용 :: 강화 수치 계산 Job>>
public struct EnhanceValueJob : IJob
{
    public NativeArray<int> value;

    public int enhanceValue;

    public void Execute()
    {
        value[0] *= enhanceValue;

        if (value[0] < 0)
            value[0] = 0;
    }
}

#endregion

#endregion