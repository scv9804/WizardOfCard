using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Text;

public class DebugManager : MonoBehaviour // DamageAnouncer ��ü
{
    [Header("��� Ȱ��ȭ")]
    [Tooltip("���� �濡 �����ϸ� �ܼ� â�� �ʱ�ȭ�մϴ�.")] public bool isClearDebugConsole;
    [Tooltip("�ܼ� â�� �������� �� ī��� �������� ǥ���մϴ�.")] public bool isPrintDamage;
    [Tooltip("������ ����� ���� �帧 ������ ǥ���մϴ�.")] public bool isPrintDamageCalculating;

    [Header("ī�� ����")]
    [Tooltip("ī�� ���� ���� �����ͺ��̽�")] public ItemSO itemSO;
    [Tooltip("������ ī�� ����")] public CardEnum selectCard;

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

    void PrintDamage(Card _card, int _damage) // ī�� ������ �����
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


    void ClearDebugConsole() // ����� â �ʱ�ȭ
    {
        if (isClearDebugConsole)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");

            method.Invoke(new object(), null);
        }
    }

    public void CardMaker() // ī�� ����
    {
        Card selectedCard = itemSO.items[(int)selectCard].card;

        CardManager.Inst.InstantinateCard(selectedCard);
    }
}
