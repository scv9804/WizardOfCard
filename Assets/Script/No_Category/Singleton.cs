using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ================================================================================ Singleton

public abstract class Singleton<TSingleton> : SerializedMonoBehaviour where TSingleton : Singleton<TSingleton>
{
    // ================================================================================ Field

    // ============================================================ Singleton

    private static TSingleton s_instance;

    // ================================================================================ Property

    // ============================================================ Singleton

    public static TSingleton Instance
    {
        get
        {
            return s_instance;
        }

        private set
        {
            s_instance = value;
        }
    }

    public static bool IsNull
    {
        get
        {
            return Instance == null;
        }
    }

    // ================================================================================ Method

    // ============================================================ Constructor

    static Singleton()
    {
        Instance = null;
    }

    // ============================================================ Event

    private void Awake()
    {
        if (IsNull)
        {
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============================================================ Singleton

    protected virtual void Initialize()
    {
        Instance = GetComponent<TSingleton>();

        DontDestroyOnLoad(gameObject);
    }
}
