using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public abstract class AbilityAction : SerializedScriptableObject
{


    public abstract void Invoke(Character target, Character trtrt);
}