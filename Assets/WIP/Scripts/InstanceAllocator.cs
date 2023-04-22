using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

namespace WIP
{
    // ==================================================================================================== InstanceAllocator

    public static class InstanceAllocator
    {
        // ==================================================================================================== Field

        // =========================================================================== Identifier

        private static int s_nextAllocateInstanceID = 0;

        // =========================================================================== StringBuilder

        private static StringBuilder s_stringBuilder = new StringBuilder();

        // ==================================================================================================== Method

        // =========================================================================== Allocator

        public static void Initialize()
        {
            s_nextAllocateInstanceID = 0;
        }

        // =========================================================================== Identifier

        public static string Allocate(InstanceType type)
        {
            s_stringBuilder.Clear();

            switch (type)
            {
                case InstanceType.Boss:
                    s_stringBuilder.Append("D");
                    break;
                case InstanceType.Card:
                    s_stringBuilder.Append("C");
                    break;
                case InstanceType.Enemy:
                    s_stringBuilder.Append("E");
                    break;
                case InstanceType.Item:
                    s_stringBuilder.Append("I");
                    break;
                case InstanceType.Player:
                    s_stringBuilder.Append("P");
                    break;
            }

            s_stringBuilder.Append(s_nextAllocateInstanceID.ToString("D6"));

            s_nextAllocateInstanceID += 1;

            return s_stringBuilder.ToString();
        }
    }

    // ==================================================================================================== InstanceType

    public enum InstanceType
    {
        Boss,

        Card,

        Enemy,

        Item,

        Player
    }
}
