using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Interfaces;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // ==================================================================================================== UnitObject

    public abstract class UnitObject<TRuntimeData> : SerializedMonoBehaviour, IUnitObject
    {




        //public static TUnitObject Create<TUnitObject>(Unit<TRuntimeData> unit) where TUnitObject : UnitObject<TRuntimeData>
        //{
        //    var unitObject = GameObject.Instantiate();

        //    Initialize(unit);
        //}

        //public abstract void Initialize(Unit<TRuntimeData> unit);
    } 
}
