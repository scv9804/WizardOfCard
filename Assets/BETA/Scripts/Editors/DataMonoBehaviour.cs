using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

public class DataMonoBehaviour : SerializedMonoBehaviour
{
    public int ID;

    public string Name;

    public string Description;

    public void Deserialize(int serialID)
    {
        //if (serialID == ID)
        //{
        //    gameObject.SetActive(true);

        //    Debug.Log(Description);
        //}
    }

    private void OnDestroy()
    {
        //Debug.Log(ID);
    }
}