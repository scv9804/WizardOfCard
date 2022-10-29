using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
public class DamageAnouncer : MonoBehaviour
{
    public bool �����_�ʱ�ȭ_���;



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
        if(_card != null)
        {
            Debug.Log(_card + ", " + _damage);
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
}
