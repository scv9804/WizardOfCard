using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BETA.Data
{
    // ==================================================================================================== ScriptableEventSystems

    public abstract class ScriptableEventSystems<TUnitObject> : SerializedScriptableObject
    {
        // ==================================================================================================== Field

        // =========================================================================== EventSystems

        public UnityEvent<TUnitObject, PointerEventData> Delegates;

        // ==================================================================================================== Method

        // =========================================================================== EventSystems

        public void Invoke(TUnitObject unitObject, PointerEventData eventData)
        {
            Delegates?.Invoke(unitObject, eventData);
        }
    }
}
