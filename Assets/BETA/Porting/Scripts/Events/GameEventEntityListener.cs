using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TacticsToolkit;

using UnityEngine.Events;

namespace BETA.Porting
{
    // ==================================================================================================== GameEventEntityListener

    public class GameEventEntityListener : GameEventListener<TacticsToolkit.Entity>
    {
        [SerializeField] private GameEventEntity eventGameObject = null;
        [SerializeField] private UnityEvent<TacticsToolkit.Entity> response = null;

        public override GameEvent<TacticsToolkit.Entity> Event => eventGameObject;
        public override UnityEvent<TacticsToolkit.Entity> Response => response;
    } 
}
