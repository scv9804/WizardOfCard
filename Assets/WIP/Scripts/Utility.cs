using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace WIP
{
    // ==================================================================================================== Utility

    public static class Utility
    {
        // ==================================================================================================== Method

        // =========================================================================== Object Group

        public static Transform GetObjectGroup(string name, Action<GameObject> option = null)
        {
            GameObject gameObject = GameObject.Find(name);

            if (gameObject is null)
            {
                gameObject = new GameObject(name);

                option?.Invoke(gameObject);
            }

            return gameObject.transform;
        }
    }
}
