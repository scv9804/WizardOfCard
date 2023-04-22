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

        public const string SINGLETON_GROUP_NAME = "===== Managers =====";

        private static TSingleton s_instance;

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        public static TSingleton Instance
        {
            get
            {
                if (s_instance is null)
                {
                    Create();
                }

                return s_instance;
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

        // ================================================== Life Cycle

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

            transform.SetParent(Utility.GetObjectGroup(SINGLETON_GROUP_NAME, (instance) => DontDestroyOnLoad(instance)));
        }
    }
}
