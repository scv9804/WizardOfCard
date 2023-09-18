using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using TacticsToolkit;

namespace BETA.Porting
{
    // ==================================================================================================== GameEventEntity

    [CreateAssetMenu(fileName = "GameEventEntity", menuName = "Porting/GameEvent/Entity")]
    public class GameEventEntity : GameEvent<TacticsToolkit.Entity>
    {
        public TacticsToolkit.Entity Entity;
    } 
}
