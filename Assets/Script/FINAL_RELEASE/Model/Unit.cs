using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Unit

[Serializable]
public abstract class Unit
{
    // ================================================================================ Field

    // ============================================================ Unit

    [ShowInInspector, TitleGroup("개체 데이터")]
    private string _instanceID;

    [ShowInInspector, TitleGroup("개체 데이터")]
    private int _serialID;

    // ================================================================================ Property

    // ============================================================ Unit

    public string InstanceID
    {
        get => _instanceID;

        set => _instanceID = value;
    }

    public int SerialID
    {
        get => _serialID;

        set => _serialID = value;
    }

    // ================================================================================ Method

    // ============================================================ Constructor

    protected Unit()
    {
        
    }

    protected Unit(string instanceID, int serialID)
    {
        InstanceID = instanceID;
        SerialID = serialID;

        Refresh();
    }

    // ============================================================ Unit

    public abstract void Refresh();
}
