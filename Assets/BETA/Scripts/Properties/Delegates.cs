using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

using UnityEngine.EventSystems;

namespace BETA.Delegates
{
    // ==================================================================================================== CardObjectEvent

    public delegate void CardObjectEvent(CardObject cardObject, PointerEventData eventData);

    // ==================================================================================================== ModelDataBindEvent

    public delegate TRuntimeData ModelDataBindEvent<TRuntimeData>(string instanceID) where TRuntimeData : RuntimeData;
}
