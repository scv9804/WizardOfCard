using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
public class DamageAnouncer : MonoBehaviour
{
    [Header("디버그 기능 활성화 여부")]
    public bool 디버그_초기화_기능;
    public bool 데미지_디버그_기능;

    [Header("카드 생성")]
    public ItemSO itemSO;
    public CardEnum selectCard;

    void Start()
    {
        Utility.onDamaged += DamageAnounce;

        Utility.onBattleStart += DebugClear;
    }

    void OnDisable()
    {
        Utility.onDamaged -= DamageAnounce;

        Utility.onBattleStart -= DebugClear;
    }

    void DamageAnounce(Card _card, int _damage) // 카드 데미지 디버그
    {
        if (데미지_디버그_기능)
        {
            if (_card != null)
            {
                Debug.Log(_card + ", " + _damage);
            }
        }
    }

    void DebugClear() // 디버그 창 초기화
    {
        if(디버그_초기화_기능)
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }

    public void CardMaker() // 카드 생성
    {
        Card selectedCard = itemSO.items[(int)selectCard].card;

        CardManager.Inst.InstantinateCard(selectedCard);
    }
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
    //마나_화살, <<22-11-01 장형용 :: MonoBehaviour 이슈로 구현 연기>>
    융해,
    과부하,
    집중_포화,
    마법진
}
