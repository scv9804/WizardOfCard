using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;

using System;

namespace BETA
{
    // ==================================================================================================== CardKeywordUtility

    public static class CardKeywordUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== CardKeyword

        public static CardKeyword Add(this CardKeyword target, CardKeyword keyword)
        {
            return target |= keyword;
        }

        public static CardKeyword Remove(this CardKeyword target, CardKeyword keyword)
        {
            return target &= keyword;
        }

        public static bool Contains(this CardKeyword target, CardKeyword keyword)
        {
            return (target & keyword) != 0;
        }
    }

    // ==================================================================================================== IFFTypeUtility

    public static class IFFTypeUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== IFFType

        public static bool Contains(this IFFType target, IFFType type)
        {
            return (target & type) != 0;
        }
    }

    // ==================================================================================================== GameObjectUtility

    public static class GameObjectUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== GameObject

        public static TGameObject Create<TGameObject>(string name = null) where TGameObject : MonoBehaviour
        {
            var gameObject = new GameObject();

            return gameObject.AddComponent<TGameObject>();
        }

        public static void Sort<TGameObject>(this IList<TGameObject> gameObjects, Predicate<TGameObject> condition) where TGameObject : MonoBehaviour
        {
            for (var i = 0; i < gameObjects.Count; i++)
            {
                if (condition.Invoke(gameObjects[i]))
                {
                    gameObjects[i].transform.SetSiblingIndex(i + 100);
                }
                else
                {
                    gameObjects[i].transform.SetSiblingIndex(i);
                }
            }
        }
    }

    // ==================================================================================================== LogUtility

    public static class LogUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== Log

        public static T Print<T>(this T messege)
        {
            if (Application.isEditor)
            {
                Debug.Log(messege);
            }

            return messege;
        }

        public static T PrintWarning<T>(this T messege)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning(messege);
            }

            return messege;
        }

        public static T PrintError<T>(this T messege)
        {
            if (Application.isEditor)
            {
                Debug.LogError(messege);
            }

            return messege;
        }
    }

    // ==================================================================================================== FormattingUtility

    public static class FormattingUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== Formatting

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

    // ==================================================================================================== RequireUtility

    public static class RequireUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== Require

        public static void Require<T>(this T value, Action callback)
        {
            if (value != null)
            {
                callback?.Invoke();
            }
        }
    }
}

// ==================================================================================================== Big

// =========================================================================== Middle

// ================================================== Small