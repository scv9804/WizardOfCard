using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Text;

public class DebugManager : MonoBehaviour // DamageAnouncer 대체
{
    [Header("기능 활성화")]
    [Tooltip("전투 방에 진입하면 콘솔 창을 초기화합니다.")] public bool isClearDebugConsole;
    [Tooltip("콘솔 창에 데미지를 준 카드와 데미지를 표시합니다.")] public bool isPrintDamage;
    [Tooltip("데미지 계산의 세부 흐름 내역을 표시합니다.")] public bool isPrintDamageCalculating;

    [Header("카드 생성")]
    [Tooltip("카드 원본 정보 데이터베이스")] public ItemSO itemSO;
    [Tooltip("생성할 카드 선택")] public CardEnum selectCard;

    public static DebugManager instance;

    StringBuilder sb = new StringBuilder();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Utility.onDamaged += PrintDamage;

        Utility.onBattleStart += ClearDebugConsole;
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        Utility.onDamaged -= PrintDamage;

        Utility.onBattleStart -= ClearDebugConsole;
#endif
    }

    void PrintDamage(Card _card, int _damage) // 카드 데미지 디버그
    {
        if (isPrintDamage)
        {
            if (_card != null)
            {
                sb.Clear();

                sb.Append(_card.name);
                sb.Append(", ");
                sb.Append(_damage);

                Debug.Log(sb.ToString());
            }
        }
    }


    void ClearDebugConsole() // 디버그 창 초기화
    {
        if (isClearDebugConsole)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");

            method.Invoke(new object(), null);
        }
    }

    public void CardMaker() // 카드 생성
    {
        Card selectedCard = itemSO.items[(int)selectCard].card;

        CardManager.Inst.InstantinateCard(selectedCard);
    }
}
