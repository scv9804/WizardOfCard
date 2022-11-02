using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
public class DamageAnouncer : MonoBehaviour
{
    [Header("����� ��� Ȱ��ȭ ����")]
    public bool �����_�ʱ�ȭ_���;
    public bool ������_�����_���;

    [Header("ī�� ����")]
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

    void DamageAnounce(Card _card, int _damage) // ī�� ������ �����
    {
        if (������_�����_���)
        {
            if (_card != null)
            {
                Debug.Log(_card + ", " + _damage);
            }
        }
    }

    void DebugClear() // ����� â �ʱ�ȭ
    {
        if(�����_�ʱ�ȭ_���)
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }

    public void CardMaker() // ī�� ����
    {
        Card selectedCard = itemSO.items[(int)selectCard].card;

        CardManager.Inst.InstantinateCard(selectedCard);
    }
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
    //����_ȭ��, <<22-11-01 ������ :: MonoBehaviour �̽��� ���� ����>>
    ����,
    ������,
    ����_��ȭ,
    ������
}
