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
    /// �̱��� �ν��Ͻ��Դϴ�.
    /// </summary>
    private static TSingleton s_instance;

    // ================================================================================ Property

    // ============================================================ Singleton

    /// <summary>
    /// �̱��� �ν��Ͻ��Դϴ�. (������Ƽ)
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
    /// �̱��� �ν��Ͻ��� ���� �����͸� �ʱ�ȭ�մϴ�.
    /// </summary>
    protected virtual void Initialize()
    {
        Instance = GetComponent<TSingleton>();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// �̱��� �ν��Ͻ��� <c>null</c>�̸� <c>true</c>�� ��ȯ�մϴ�.
    /// </summary>
    private static bool IsNull()
    {
        return Instance == null;
    }
}
