using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ DataManager

public class DataManager : Singleton<DataManager>
{
    // ================================================================================ Constance

    // ============================================================ Instance

    public const int MAX_INSTANCE_COUNT = 10000;

    public const string INSTANCE_ID_FORMAT = "D4";

    // ================================================================================ Field

    //

    //public static event Action<string, int> OnDataCreate;
    //public static event Action<string> OnDataDelete;

    // ============================================================ Instance

    [ShowInInspector, TitleGroup("인스턴스 참조")]
    private Dictionary<string, int> _reference = new Dictionary<string, int>();

    [ShowInInspector, TitleGroup("인스턴스 참조")]
    private Stack<string> _chuck = new Stack<string>();

    // ================================================================================ Property

    // ============================================================ Instance

    public Dictionary<string, int> Reference
    {
        get
        {
            return _reference;
        }
    }

    // ================================================================================ Method

    // ============================================================ Singleton

    protected override void Initialize()
    {
        base.Initialize();

        Generate(128);
    }

    // ============================================================ Chuck

    private void Generate(int size)
    {
        var max = MAX_INSTANCE_COUNT - _reference.Count;
        var count = max > size ? size : max;

        for (var i = 0; i < count; i++)
        {
            var hash = 0;
            var ID = "####";

            do
            {
                hash = UnityEngine.Random.Range(0, MAX_INSTANCE_COUNT);
                ID = hash.ToString(INSTANCE_ID_FORMAT);
            }
            while (_chuck.Contains(ID) || Reference.ContainsKey(ID));

            _chuck.Push(ID);
        }
    }

    // ============================================================ Instance

    public string Allocate(string instanceID = null)
    {
        if (instanceID == null)
        {
            instanceID = _chuck.Pop();

            if (_chuck.Count == 0)
            {
                Generate(128);
            }
        }

        if (!Reference.ContainsKey(instanceID))
        {
            Reference.Add(instanceID, 0);
        }

        Reference[instanceID] += 1;

        return instanceID;
    }

    public void Deallocate(string instanceID)
    {
        Reference[instanceID] -= 1;

        if (Reference[instanceID] == 0)
        {
            Reference.Remove(instanceID);
        }
    }

    // ============================================================ Data

    //public TData Subscribe<TData>() where TData : IInstanceData
    //{
    //    return null;
    //}

    // ============================================================ DataBase

    private TDataBase Access<TDataBase>()
    {
        return GetComponent<TDataBase>();
    }
}