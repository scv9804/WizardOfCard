using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BETA
{
    // ==================================================================================================== EditorDebug

    public static class EditorDebug
    {
        // ==================================================================================================== Method

        // =========================================================================== Debug

        public static void EditorLog(object message)
        {
            #region Debug.Log(message);
#if UNITY_EDITOR
            Debug.Log(message);
#endif 
            #endregion
        }

        public static void EditorLogWarning(object message)
        {
            #region Debug.LogWarning(message);
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif 
            #endregion
        }

        public static void EditorLogError(object message)
        {
            #region Debug.LogError(message);
#if UNITY_EDITOR
            Debug.LogError(message);
#endif 
            #endregion
        }
    }
}
