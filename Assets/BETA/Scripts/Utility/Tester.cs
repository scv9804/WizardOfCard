using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    // ==================================================================================================== Tester

    public class Tester : MonoBehaviour
    {
        // ==================================================================================================== Field

        // ==================================================================================================== Method

        // =========================================================================== Event

        void Start()
        {
            CardManager.Instance.OnCardManagerAwake();

            //var data = Card.Original.Data.Create(0, 0);

            //EditorDebug.EditorLog(data.Name);
            //EditorDebug.EditorLog(data.Cost);
            //EditorDebug.EditorLog(data.Description);
        }

        [Serializable] public class Data<T>
        {
            public T Value;
        }
    } 
}
