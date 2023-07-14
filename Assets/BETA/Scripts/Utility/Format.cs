using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace BETA
{
    // ==================================================================================================== Format

    public static class Format
    {
        // ==================================================================================================== Method

        // =========================================================================== Format

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