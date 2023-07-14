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
        }
    } 
}
