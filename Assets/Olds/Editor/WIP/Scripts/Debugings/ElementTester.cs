using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ElementTester : MonoBehaviour
{

}

[Serializable] 
public class Element
{
    public int ID;
}

[Serializable]
public class NameElement : Element
{
    public string Name;
}
