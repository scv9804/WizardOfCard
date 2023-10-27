using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// ================================================================================ Singleton

public abstract class Singleton<TSingleton> : SerializedMonoBehaviour where TSingleton : Singleton<TSingleton>
{
    // ================================================================================ Field

    // ============================================================ Singleton

    /// <summary>
    /// 싱글턴 인스턴스입니다.
    /// </summary>
    private static TSingleton s_instance;

    // ================================================================================ Property

    // ============================================================ Singleton

    /// <summary>
    /// 싱글턴 인스턴스입니다. (프로퍼티)
    /// </summary>
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

    // ================================================================================ Method

    // ============================================================ Event

    private void Awake()
    {
        if (IsNull())
        {
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // ============================================================ Singleton

    /// <summary>
    /// 싱글턴 인스턴스와 관련 데이터를 초기화합니다.
    /// </summary>
    protected virtual void Initialize()
    {
        Instance = GetComponent<TSingleton>();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 싱글턴 인스턴스가 <c>null</c>이면 <c>true</c>를 반환합니다.
    /// </summary>
    private static bool IsNull()
    {
        return Instance == null;
    }
}
