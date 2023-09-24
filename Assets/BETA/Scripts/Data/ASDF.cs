using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA.BETA
{
    public class SampleScriptableObject
    {

    }

    public class SampleMonoBehaviour
    {

    }

    public class Ability : SampleScriptableObject
    {
        public AbilityScriptableData[] Data;

        public void Invoke()
        {

        }
    }

    public abstract class AbilityAction : SampleScriptableObject
    {


        public abstract void Invoke();
    }

    public abstract class AbilityScriptableData : SampleScriptableObject
    {
        public AbilityAction Action;

        public abstract void Invoke();
    }

    public struct AbilityParameter
    {

    }

    public class SampleRuntimeData
    {
        public string InstanceID
        {
            get; set;
        }

        public int SerialID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int Cost
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }



        public int Damage
        {
            get; set;
        }

        public int Shield
        {
            get; set;
        }

        public int Heal
        {
            get; set;
        }

        public int Draw
        {
            get; set;
        }
    }


















    public class Entity : SampleMonoBehaviour
    {

    }
}