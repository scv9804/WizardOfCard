using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Singleton
{
    // ==================================================================================================== SingletonMonoBehaviour

    public abstract class SingletonMonoBehaviour<TSingleton> : SerializedMonoBehaviour where TSingleton : SingletonMonoBehaviour<TSingleton>
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
                    if (IsEmpty)
                    {
                        GameObjectUtility.Create<TSingleton>();
                    }

                    return s_instance;
                }
            }

            private set
            {
                s_instance = value;
            }
        }

        protected static bool IsEmpty
        {
            get
            {
                return s_instance == null;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        static SingletonMonoBehaviour()
        {
            Instance = null;
        }

        // =========================================================================== Event

        private void Awake()
        {
            Initialize();
        }

        private void OnApplicationQuit()
        {
            
        }

        // =========================================================================== Singleton

        protected virtual bool Initialize()
        {
            var isEmpty = IsEmpty;

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

        protected virtual bool Finalize()
        {
            var isEmpty = IsEmpty;

            if (!isEmpty)
            {
                Instance = null;
            }

            return isEmpty;
        }
    }
}