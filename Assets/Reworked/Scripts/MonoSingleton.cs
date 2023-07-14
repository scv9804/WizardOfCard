using UnityEngine;

namespace Reworked
{
    // ==================================================================================================== MonoSingleton

    public class MonoSingleton<TSingleton> : MonoBehaviour where TSingleton : MonoSingleton<TSingleton>
    {
        // ==================================================================================================== Field

        // =========================================================================== Singleton

        private static TSingleton s_instance;

        // =========================================================================== Thread Safe

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

        // ==================================================================================================== Method

        // =========================================================================== Event

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnApplicationQuit()
        {
            Clear();
        }

        // =========================================================================== Singleton

        private static void Create()
        {
            var gameObject = new GameObject();

            TSingleton instance = gameObject.AddComponent<TSingleton>();
        }

        protected virtual bool Initialize()
        {
            bool isEmpty = s_instance is null;

            if (isEmpty)
            {
                Instance = this as TSingleton;
            }
            else
            {
                Destroy(gameObject);
            }

            return isEmpty;
        }

        protected virtual void Clear()
        {
            Instance = null;
        }
    }
}
