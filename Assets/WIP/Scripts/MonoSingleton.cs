using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== MonoSingleton

    public abstract class MonoSingleton<TSingleton> : MonoBehaviour where TSingleton : MonoSingleton<TSingleton>
    {
        // ==================================================================================================== Field

        // =========================================================================== Singleton

        private static TSingleton s_instance;

        private static object s_lock = new object();

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        public static TSingleton Instance
        {
            get
            {
                lock (s_lock)
                {
                    if (s_instance is null)
                    {
                        Create();
                    }

                    return s_instance;
                }
            }

            private set
            {
                s_instance = value;
            }
        }

        protected abstract string Name
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        protected virtual void Awake()
        {
            if (s_instance is null)
            {
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
        }

        // =========================================================================== Singleton

        private static void Create()
        {
            var gameObject = new GameObject();

            TSingleton instance = gameObject.AddComponent<TSingleton>();

            instance.name = instance.Name;
        }

        public virtual void Initialize()
        {
            Instance = this as TSingleton;
        }
    }
}
