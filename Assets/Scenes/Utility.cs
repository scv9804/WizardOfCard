using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// ================================================================================ RandomUtility

public static class RandomUtility
{
    // ================================================================================ Method

    // ============================================================ Random

    //È®·ü »Ì±â
    public static float Choose(this float[] probs)
    {
        var total = 0.0f;

        foreach (var elem in probs)
        {
            total += elem;
        }

        var randomPoint = UnityEngine.Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}

// ================================================================================ DebugUtility

public static class DebugUtility
{
    // ================================================================================ Method

    // ============================================================ Debug

    public static T Log<T>(this T messege)
    {
        if (Application.isEditor)
        {
            Debug.Log(messege);
        }

        return messege;
    }

    public static T LogWarning<T>(this T messege)
    {
        if (Application.isEditor)
        {
            Debug.LogWarning(messege);
        }

        return messege;
    }

    public static T LogError<T>(this T messege)
    {
        if (Application.isEditor)
        {
            Debug.LogError(messege);
        }

        return messege;
    }
}

// ================================================================================ FormatUtility

public static class FormatUtility
{
    // ================================================================================ Method

    // ============================================================ Formatting

    public static string Bold(this string messege)
    {
        return $"<b>{messege}</b>";
    }

    public static string Italics(this string messege)
    {
        return $"<i>{messege}</i>";
    }

    public static string Size(this string messege, int size)
    {
        return $"<size={size}>{messege}</size>";
    }

    public static string Color(this string messege, string code)
    {
        return $"<color={code}>{messege}</color>";
    }
}

// ================================================================================ Instance

//public static class Instances
//{
//    // ================================================================================ Constance

//    // ============================================================ Instance

//    public const int MAX_INSTANCE_COUNT = 10000;

//    public const string INSTANCE_ID_FORMAT = "D4";

//    // ================================================================================ Field

//    // ============================================================ Instance

//    private static Dictionary<string, int> s_instances = new Dictionary<string, int>();

//    private static Stack<string> s_chunk = new Stack<string>();

//    // ============================================================ CallBack

//    public static event Action<string> OnInstanceDeleted;

//    // ================================================================================ Method

//    // ============================================================ Constructor

//    static Instances()
//    {
//        s_instances.Clear();
//        s_chunk.Clear();

//        OnInstanceDeleted = null;

//        SetChunk(128);
//    }

//    // ============================================================ Instance

//    public static string Allocate(string instanceID)
//    {
//        instanceID = instanceID == null ? s_chunk.Pop() : instanceID;

//        if (s_chunk.Count == 0)
//        {
//            SetChunk(128);
//        }

//        if (!s_instances.ContainsKey(instanceID))
//        {
//            s_instances.Add(instanceID, 0);
//        }

//        s_instances[instanceID] += 1;

//        return instanceID;
//    }

//    public static void Dellocate(string instanceID)
//    {
//        s_instances[instanceID] -= 1;

//        if (s_instances[instanceID] == 0)
//        {
//            s_instances.Remove(instanceID);

//            OnInstanceDeleted?.Invoke(instanceID);
//        }
//    }

//    private static void SetChunk(int size)
//    {
//        int random;
//        string item;

//        for (var i = 0; i < size; i++)
//        {
//            random = UnityEngine.Random.Range(0, MAX_INSTANCE_COUNT);
//            item = random.ToString(INSTANCE_ID_FORMAT);

//            if (!s_instances.ContainsKey(item) && !s_chunk.Contains(item))
//            {
//                s_chunk.Push(item);
//            }
//        }
//    }

//    public static void PrintChunk()
//    {
//        var stringBuilder = new System.Text.StringBuilder();

//        foreach (var item in s_chunk)
//        {
//            stringBuilder.Append($" {item}");
//        }

//        stringBuilder.Log();
//    }

//    public static List<string> GetChunk()
//    {
//        var chunk = new List<string>();

//        foreach (var item in s_chunk)
//        {
//            chunk.Add(item);
//        }

//        return chunk;
//    }
//}

// ================================================================================ TemplateUtility

//public static class Macro
//{
//    // ================================================================================ Method

//    // ============================================================ Template

//    public static void Foreach<TKey>(this Action<TKey> callback, params TKey[] keys)
//    {
//        foreach (var key in keys)
//        {
//            callback?.Invoke(key);
//        }
//    }
//}

// ================================================================================ LibraryUtility

//public static class LibraryUtility
//{
//    // ================================================================================ Method

//    // ============================================================ Library

//    public static void Foreach<TKey, TValue>(this Library<TKey, TValue> library, Action<TKey> callback, params TKey[] keys)
//    {
//        foreach (var key in keys)
//        {
//            callback?.Invoke(key);
//        }
//    }
//}

// ================================================================================ ResourceUtility

public static class ResourceUtility
{
    // ================================================================================ Method

    // ============================================================ Resource

    public static T IntegrityCheck<T>(this T resource, string path) where T : UnityEngine.Object
    {
        return resource == null ? Resources.Load<T>(path) : resource;
    }
}

// ================================================================================ ????????

// ============================================================ ??????

// ======================================== ????