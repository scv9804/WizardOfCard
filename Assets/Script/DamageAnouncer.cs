using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
public class DamageAnouncer : MonoBehaviour
{
    public bool 디버그_초기화_기능;



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
        if(_card != null)
        {
            Debug.Log(_card + ", " + _damage);
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
}
