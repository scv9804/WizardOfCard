using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ EventDispatcher

public abstract class EventDispatcher<T1> : SerializedScriptableObject, IEventDispatcher
{
    // ================================================================================ Field

    // ============================================================ EventDispatcher

    public event Action<T1> Listener;

    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Launch(T1 param1)
    {
        Listener?.Invoke(param1);
    }

    [Button, TitleGroup("초기화")]
    public void Clear()
    {
        Listener = null;
    }
}

public abstract class EventDispatcher<T1, T2> : SerializedScriptableObject, IEventDispatcher
{
    // ================================================================================ Field

    // ============================================================ EventDispatcher

    public event Action<T1, T2> Listener;

    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Launch(T1 param1, T2 param2)
    {
        Listener?.Invoke(param1, param2);
    }

    [Button, TitleGroup("초기화")]
    public void Clear()
    {
        Listener = null;
    }
}

public abstract class EventDispatcher<T1, T2, T3> : SerializedScriptableObject, IEventDispatcher
{
    // ================================================================================ Field

    // ============================================================ EventDispatcher

    public event Action<T1, T2, T3> Listener;

    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Launch(T1 param1, T2 param2, T3 param3)
    {
        Listener?.Invoke(param1, param2, param3);
    }

    [Button, TitleGroup("초기화")]
    public void Clear()
    {
        Listener = null;
    }
}

public abstract class EventDispatcher<T1, T2, T3, T4> : SerializedScriptableObject, IEventDispatcher
{
    // ================================================================================ Field

    // ============================================================ EventDispatcher

    public event Action<T1, T2, T3, T4> Listener;

    // ================================================================================ Method

    // ============================================================ EventDispatcher

    public void Launch(T1 param1, T2 param2, T3 param3, T4 param4)
    {
        Listener?.Invoke(param1, param2, param3, param4);
    }

    [Button, TitleGroup("초기화")]
    public void Clear()
    {
        Listener = null;
    }
}
