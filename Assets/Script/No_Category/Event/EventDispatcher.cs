using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ EventDispatcher

[CreateAssetMenu(menuName = "Event/General")]
public class EventDispatcher : SerializedScriptableObject, IEventDispatcher
{
    // ================================================================================ Field

    // ============================================================ EventDispatcher

    public event Action Listener;

    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Launch()
    {
        Listener?.Invoke();
    }

    [Button, TitleGroup("√ ±‚»≠")]
    public void Clear()
    {
        Listener = null;
    }
}

// ================================================================================ IEventDispatcher

public interface IEventDispatcher
{
    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Clear();
}
