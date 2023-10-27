using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Event

public abstract class Event<T0> : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ EventListener

    private event EventListener<T0> _onEvent;

    // ================================================================================ Method

    // ============================================================ EventListener

    public void AddListener(EventListener<T0> listener)
    {
        _onEvent += listener;
    }

    public void RemoveListener(EventListener<T0> listener)
    {
        _onEvent -= listener;
    }

    public void Launch(T0 parameter0)
    {
        _onEvent?.Invoke(parameter0);
    }
}

public abstract class Event<T0, T1> : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ EventListener

    private event EventListener<T0, T1> _onEvent;

    // ================================================================================ Method

    // ============================================================ EventListener

    public void AddListener(EventListener<T0, T1> listener)
    {
        _onEvent += listener;
    }

    public void RemoveListener(EventListener<T0, T1> listener)
    {
        _onEvent -= listener;
    }

    public void Launch(T0 parameter0, T1 parameter1)
    {
        _onEvent?.Invoke(parameter0, parameter1);
    }
}

public abstract class Event<T0, T1, T2> : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ EventListener

    private event EventListener<T0, T1, T2> _onEvent;

    // ================================================================================ Method

    // ============================================================ EventListener

    public void AddListener(EventListener<T0, T1, T2> listener)
    {
        _onEvent += listener;
    }

    public void RemoveListener(EventListener<T0, T1, T2> listener)
    {
        _onEvent -= listener;
    }

    public void Launch(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        _onEvent?.Invoke(parameter0, parameter1, parameter2);
    }
}

public abstract class Event<T0, T1, T2, T3> : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ EventListener

    private event EventListener<T0, T1, T2, T3> _onEvent;

    // ================================================================================ Method

    // ============================================================ EventListener

    public void AddListener(EventListener<T0, T1, T2, T3> listener)
    {
        _onEvent += listener;
    }

    public void RemoveListener(EventListener<T0, T1, T2, T3> listener)
    {
        _onEvent -= listener;
    }

    public void Launch(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        _onEvent?.Invoke(parameter0, parameter1, parameter2, parameter3);
    }
}