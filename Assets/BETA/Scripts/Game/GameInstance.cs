using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== Game.Instance

    public static partial class Game
    {
        public static class Instance
        {
            // ==================================================================================================== Constant

            // =========================================================================== Instance

            public const string NEW_INSTANCE = null;

            private const int _MAX_INSTANCE_COUNT = 10000;

            // =========================================================================== Identifier

            private const string _INSTANCE_ID_FORMAT = "D4";

            // ==================================================================================================== Field

            // =========================================================================== Instance

            private static Dictionary<string, int> _instances = new Dictionary<string, int>();

            // ==================================================================================================== Method

            // =========================================================================== Instance

            public static string Allocate(string instanceID)
            {
                if (instanceID is NEW_INSTANCE)
                {
                    do
                    {
                        instanceID = Random.Range(0, _MAX_INSTANCE_COUNT).ToString(_INSTANCE_ID_FORMAT);
                    }
                    while (_instances.ContainsKey(instanceID));
                }

                if (!_instances.ContainsKey(instanceID))
                {
                    _instances.Add(instanceID, 0);
                }

                EditorDebug.EditorLog($"{"[SYSTEM]".Color("#F0F8FF").Bold()} INSTANCE {instanceID} ALLOCATED");

                _instances[instanceID] += 1;

                return instanceID;
            }

            public static void Deallocate(string instanceID)
            {
                if (!_instances.ContainsKey(instanceID))
                {
                    EditorDebug.EditorLogError($"{"[SYSTEM]".Color("#F0F8FF").Bold()} 404 NOT FOUND. INSTANCE {instanceID} COULD NOT BE FOUND");

                    return;
                }

                _instances[instanceID] -= 1;

                if (_instances[instanceID] is 0)
                {
                    _instances.Remove(instanceID);
                }

                EditorDebug.EditorLog($"{"[SYSTEM]".Color("#F0F8FF").Bold()} INSTANCE {instanceID} DEALLOCATED");
            }

            public static bool IsContains(string instanceID)
            {
                return _instances.ContainsKey(instanceID);
            }

            // =========================================================================== BETA

            public static void ReadAllData()
            {
                foreach (var record in _instances)
                {
                    EditorDebug.EditorLog($"{"Instance ID".Color("#00FFFF").Bold()}: {record.Key}, {"Count".Color("#00FFFF").Bold()}: {record.Value}");
                }
            }
        }
    }
}
