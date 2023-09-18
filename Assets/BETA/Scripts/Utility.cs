using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;
using BETA.Porting;

using System;

namespace BETA
{
    // ==================================================================================================== ArrowUtility

    public static class ArrowUtility
    {
        // ==================================================================================================== Method

        // =========================================================================== Arrow

        public static Arrow GetDirection(this OverlayTile current, OverlayTile previous, OverlayTile next)
        {
            var arrow = Arrow.NONE;

            var previousDirection = previous != null ? current.Location2D - previous.Location2D : new Vector2Int(0, 0);
            var nextDirection = next != null ? next.Location2D - current.Location2D : new Vector2Int(0, 0);
            var currentDirection = previousDirection != nextDirection ? previousDirection + nextDirection : nextDirection;

            if (currentDirection.y == 1)
            {
                arrow.Add(Arrow.UP);
            }

            if (currentDirection.y == -1)
            {
                arrow.Add(Arrow.DOWN);
            }

            if (currentDirection.x == -1)
            {
                arrow.Add(Arrow.LEFT);
            }

            if (currentDirection.x == 1)
            {
                arrow.Add(Arrow.RIGHT);
            }

            if (next == null)
            {
                arrow.Add(Arrow.END);
            }

            // UP => DOWN => UP_LFET => DOWN_LEFT => LEFT => UP_RIGHT => DOWN_RIGHT => RIGHT => END => END => END => END
            // -1 => 0 => 1 => ? => ? => 2 => ? => ? => 3 => 8 => 9 => 10 => 11

            // UP       = +1
            // DOWN     = +2
            // LEFT     = +3 / +4
            // RIGHT    = +4 / +6

            return arrow;
        }

        public static Arrow Add(this Arrow target, Arrow arrow)
        {
            return target |= arrow;
        }

        public static Arrow Remove(this Arrow target, Arrow arrow)
        {
            return target &= arrow;
        }

        public static bool Contains(this Arrow target, Arrow arrow)
        {
            return (target & arrow) != 0;
        }

        public static int GetSpriteIndex(this Arrow target)
        {
            var index = -1;

            if (target.Contains(Arrow.UP))
            {
                index += 1;
            }

            if (target.Contains(Arrow.DOWN))
            {
                index += 2;
            }

            if (target.Contains(Arrow.LEFT))
            {
                index += index < 0 ? 3 : 4;
            }

            if (target.Contains(Arrow.RIGHT))
            {
                index += index < 0 ? 4 : 6;
            }

            if (target.Contains(Arrow.END))
            {
                index += 8;
            }

            return index;
        }
    }

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
}

// ==================================================================================================== Big

// =========================================================================== Middle

// ================================================== Small