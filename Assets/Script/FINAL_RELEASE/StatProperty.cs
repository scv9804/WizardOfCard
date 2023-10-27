using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Stat

[Serializable]
public class Stat<T>
{
    // ================================================================================ Field

    // ============================================================ Stat

    [ShowInInspector, TitleGroup("스탯 데이터")]
    private T _base;

    [ShowInInspector, TitleGroup("스탯 데이터")]
    private T _current;

    [ShowInInspector, TitleGroup("스탯 데이터")]
    private List<StatModifier<T>> _modifier = new List<StatModifier<T>>();

    // ================================================================================ Property

    // ============================================================ Stat

    public T Base
    {
        get => _base;

        set => _base = value;
    }

    public T Current
    {
        get => _current;

        set => _current = value;
    }

    public List<StatModifier<T>> Modifier
    {
        get => _modifier;

        set => _modifier = value;
    }

    // ================================================================================ Method

    // ============================================================ Constructor

    public Stat()
    {

    }

    public Stat(T value)
    {
        Base = value;
        Current = value;
    }

    // ============================================================ Stat

    public void Refresh()
    {
        Current = Base;

        foreach (var modifier in Modifier)
        {
            //Current = modifier.Modify(this);

            modifier.Modify(this);
        }
    }

    public void AddModifier(StatModifier<T> modifier)
    {
        Modifier.Add(modifier);

        Refresh();
    }

    public void RemoveModifier(StatModifier<T> modifier)
    {
        Modifier.Remove(modifier);

        Refresh();
    }
}

// ================================================================================ StatModifier

[Serializable]
public abstract class StatModifier<T>
{
    // ================================================================================ Method

    // ============================================================ Stat

    public abstract T Modify(Stat<T> stat);
}

// 버프 종류

// 1. 일반 버프형
// 2. 반영구적 버프형
// 3. 턴제 버프형
// 4. 횟수제 버프형
// 5. 오오라(조건적) 버프형