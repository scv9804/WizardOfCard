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
    마법진
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

    public static Action onBattleStart; // 전투 방 진입 시 호출

    public static Action<Card> onCardUsed; // 카드 사용 시 호출
    public static Action<Card, int> onDamaged; // 카드로 데미지를 입힐 시 호출
}

#region 코루틴 커스텀 클래스

// <<22-10-30 장형용 :: 모든 카드 사용과 데미지 처리가 끝날때까지 대기시켜주는 클래스>>
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

// <<22-10-30 장형용 :: 해당 Entity의 피격 모션 처리가 끝날때까지 대기시켜주는 클래스>>
// <<22-11-09 장형용 :: 삭제>>
//public class WaitForAllMotionDone : CustomYieldInstruction

#endregion