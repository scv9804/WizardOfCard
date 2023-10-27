using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ================================================================================ UnitObject

public abstract class UnitObject<TUnit> : SerializedMonoBehaviour where TUnit : Unit
{
    // ================================================================================ Field

    // ============================================================ UnitObject

    [SerializeField, TitleGroup("ºä")]
    private GameObject _view;

    [SerializeField, TitleGroup("¸ðµ¨")]
    private TUnit _model;

    // ================================================================================ Property

    // ============================================================ UnitObject

    protected GameObject View
    {
        get => _view;

        set => _view = value;
    }

    protected TUnit Model
    {
        get => _model;

        set => _model = value;
    }

    // ================================================================================ Method

    // ============================================================ UnitObject

    public virtual void Initialize(TUnit unit)
    {
        Model = unit;
    }
}
