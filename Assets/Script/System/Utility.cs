using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Pos_Rot_Scale
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


public static class Utility_enum
{
    public enum e_CardType { Spell, Spell_Enhance, Shlied, Heal, Buff, Debuff };
    public enum ItemType {Use, Equi, Quest}
}

public enum AttackRange
{ 
    Target_AllEnemy, 
    Target_Self, 
    Target_Single 
}

public enum CardType 
{
    Spell, 
    Spell_Enhance, 
    Shlied,
    Heal, 
    Buff, 
    Debuff
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
    ������
}


public class Utility : MonoBehaviour
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

    public static Action onBattleStart; // ���� �� ���� �� ȣ��

    public static Action<Card> onCardUsed; // ī�� ��� �� ȣ��
    public static Action<Card, int> onDamaged; // ī��� �������� ���� �� ȣ��
}

#region �ڷ�ƾ Ŀ���� Ŭ����

// <<22-10-30 ������ :: ��� ī�� ���� ������ ó���� ���������� �������ִ� Ŭ����>>
public class WaitAllCardUsingDone : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return CardManager.i_usingCardCount > 0 || EntityManager.i_checkingEntitiesCount > 0;
        }
    }
}

// <<22-10-30 ������ :: �ش� Entity�� �ǰ� ��� ó���� ���������� �������ִ� Ŭ����>>
// <<22-11-09 ������ :: ����>>
//public class WaitForAllMotionDone : CustomYieldInstruction

#endregion