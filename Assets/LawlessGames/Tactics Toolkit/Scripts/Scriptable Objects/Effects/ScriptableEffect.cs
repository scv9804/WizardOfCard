using System;
using UnityEngine;

namespace TacticsToolkit
{
    //ScriptableEffects can be attached to both tiles and abilities. 
    [CreateAssetMenu(fileName = "ScriptableEffect", menuName = "ScriptableObjects/ScriptableEffect")]
    public class ScriptableEffect : ScriptableObject
    {
        public EffectName Name;
        public Stats statKey;
        public Operation Operator;
        public float Duration;
        public int Value;
        public bool Type;

        public Stats GetStatKey()
        {
            return statKey;
        }
    }
}
