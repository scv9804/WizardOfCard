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

    [ShowInInspector, TitleGroup("���� ������")]
    private T _base;

    [ShowInInspector, TitleGroup("���� ������")]
    private T _current;

    [ShowInInspector, TitleGroup("���� ������")]
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

// ���� ����

// 1. �Ϲ� ������
// 2. �ݿ����� ������
// 3. ���� ������
// 4. Ƚ���� ������
// 5. ������(������) ������