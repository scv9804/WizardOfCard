using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== Game.ManagerBehaviour

    public static partial class Game
    {
        public class ManagerBehaviour<TManager> : MonoSingleton<TManager> where TManager : ManagerBehaviour<TManager>
        {
            // ==================================================================================================== Method

            // =========================================================================== Event

            protected override void Awake()
            {
                base.Awake();

                Game.Instance.ReadAllData();
            }

            protected override void OnApplicationQuit()
            {
                base.OnApplicationQuit();

                Game.Instance.ReadAllData();
            }

            // =========================================================================== Singleton

            protected override bool Initialize()
            {
                bool isEmpty = base.Initialize();

                if (isEmpty)
                {
                    name = "Game Manager";

                    DontDestroyOnLoad(gameObject);
                }

                return isEmpty;
            }
        }
    }
}
