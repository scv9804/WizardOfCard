using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;

namespace Reworked
{
    public class Terminal : MonoBehaviour
    {
        public CardDataConsole[] Consoles;

        private void Awake()
        {
            Game.Clear();
        }
    } 

    [Serializable] public class CardDataConsole
    {
        public string Key;

        public Card.Data Data;
    }
}