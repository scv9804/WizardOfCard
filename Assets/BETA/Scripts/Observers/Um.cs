using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

using UnityEngine.Events;

namespace BETA
{
    public class Um : MonoBehaviour
    {
        public EventDispatcher<int> OnTurnLeft;

        private void Awake()
        {
            OnTurnLeft.Listener += LeftTurnCheck;
        }

        private void OnDestroy()
        {
            OnTurnLeft.Listener -= LeftTurnCheck;
        }

        private void Start()
        {
            OnTurnLeft.Launch(5);
        }

        private void LeftTurnCheck(int turn)
        {
            $"{turn}≈œ ≥≤¿∫ {name}...".Log();
        }
    } 
}