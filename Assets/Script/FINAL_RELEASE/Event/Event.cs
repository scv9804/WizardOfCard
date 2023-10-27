using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Event

[CreateAssetMenu(menuName = "TEMP/Event/General")]
public class Event : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ EventListener

    private event EventListener _onEvent;

    // ================================================================================ Method

    // ============================================================ EventListener

    public void AddListener(EventListener listener)
    {
        _onEvent += listener;
    }

    public void RemoveListener(EventListener listener)
    {
        _onEvent -= listener;
    }

    public void Launch()
    {
        _onEvent?.Invoke();
    }
}
