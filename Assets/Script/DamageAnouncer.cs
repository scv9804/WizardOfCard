using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnouncer : MonoBehaviour
{
    void Start()
    {
        Utility.onDamaged += DamageAnounce;
    }

    void OnDisable()
    {
        Utility.onDamaged -= DamageAnounce;
    }

    void DamageAnounce(Card _card, int _damage)
    {
        if(_card != null)
        {
            Debug.Log(_card + ", " + _damage);
        }
    }
}
