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

    //Ȯ�� �̱�
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

    public static Action onBattleStart; // ���� �� ���� �� ȣ��

    public static Action<Card> onCardUsed; // ī�� ��� �� ȣ��
    public static Action<Card, int> onDamaged; // ī��� �������� ���� �� ȣ��
}

// << 22-11-12 ������ :: Ŭ���� ����>>
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

// <<22-11-24 ������ :: ����>>
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
    ������,
    ��ȣ��,
    ȭ����,
    �Ҿ�����_��,
    ����,
    ����_����,
    ����â,
    ���_ȸ��,
    ȭ��_��ǳ,
    ����_����,
    ����_����_����,
    ����_����,
    �ر�,
    ����,
    ����,
    ����,
    ������,
    ����_��ȭ,
    ħ��,
    ����_��,
    ����_��,
    ��ǳ��_��,
    ������_��,
    ����,
    ����_ȯ��,
    ��_����,
    ��Ÿ,
    ����_����,
    ������,
    ����_��Ȱ��,
    ����_����,
    ����_����,
    ������,
    ����׿�
}

public enum E_CardStats
{ 
    Cannot, 
    CanMouseOver,
    CanAll
}

#endregion

#region Custom Yield Instructions

// <<22-10-30 ������ :: ��� ī�� ���� ������ ó���� ���������� �������ִ� Ŭ����>>
public class WaitAllCardUsingDone : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get { return CardManager.i_usingCardCount > 0 || EntityManager.i_checkingEntitiesCount > 0; }
    }
}

// <<22-10-30 ������ :: �ش� Entity�� �ǰ� ��� ó���� ���������� �������ִ� Ŭ����>>
// <<22-11-09 ������ :: ����>>
//public class WaitForAllMotionDone : CustomYieldInstruction

#endregion

#region Job System

#region Entities

// <<22-11-12 ������ :: ��ȣ ��� Job>>
public struct ProtectionJob : IJob
{
    public NativeArray<int> values; // 0: Damage, 1: Protection

    public bool isPrint;

    public void Execute()
    {
        #region �����

        if (isPrint)
        {
            Debug.Log("��ȣ ���� �� ������ : " + values[0]);
            Debug.Log("��ȣ ���� �� ��ȣ : " + values[1]);
            Debug.Log("-------------------------------------");
        }

        #endregion

        if (values[1] >= values[0])
        {
            values[0] = 0;
        }

        if (values[1] > 0)
            values[1]--;

        #region �����

        if (isPrint)
        {
            Debug.Log("��ȣ ���� �� ������ : " + values[0]);
            Debug.Log("��ȣ ���� �� ��ȣ : " + values[1]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

// <<22-11-12 ������ :: ���� ��� Job>>
public struct ShieldJob : IJob
{
    public NativeArray<int> values; // 0: Damage, 2: Shield, 3: Health

    public bool isPrint;

    public void Execute()
    {
        #region �����

        if (isPrint)
        {
            Debug.Log("���� ���� �� ������ : " + values[0]);
            Debug.Log("���� ���� �� ���� : " + values[2]);
            Debug.Log("���� ���� �� ü�� : " + values[3]);
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

        #region �����

        if (isPrint)
        {
            Debug.Log("���� ���� �� ������ : " + values[0]);
            Debug.Log("���� ���� �� ���� : " + values[2]);
            Debug.Log("���� ���� �� ü�� : " + values[3]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

// <<22-11-12 ������ :: ȭ�� ��� Job>>
public struct BurningJob : IJob
{
    public NativeArray<int> values; // 2: Shield, 3: Health, 4: Burning

    public bool isPrint;

    public void Execute()
    {
        #region �����

        if (isPrint)
        {
            Debug.Log("ȭ�� ���� �� ���� : " + values[2]);
            Debug.Log("ȭ�� ���� �� ü�� : " + values[3]);
            Debug.Log("ȭ�� ���� �� ȭ�� : " + values[4]);
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

        #region �����

        if (isPrint)
        {
            Debug.Log("ȭ�� ���� �� ���� : " + values[2]);
            Debug.Log("ȭ�� ���� �� ü�� : " + values[3]);
            Debug.Log("ȭ�� ���� �� ȭ�� : " + values[4]);
            Debug.Log("-------------------------------------");
        }

        #endregion
    }
}

#endregion

#region Cards

// <<22-11-12 ������ :: ���� ģȭ�� ��� Job>>
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

// <<22-11-12 ������ :: ���� ���׷� ��� Job>>
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

// <<22-11-12 ������ :: ��ȭ ��ġ ��� Job>>
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