using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

namespace BETA.Delegates
{
    // ==================================================================================================== ModelDataBindEvent

    public delegate TRuntimeData ModelDataBindEvent<TRuntimeData>(string instanceID) where TRuntimeData : RuntimeData;
}
