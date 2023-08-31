using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace BETA.Singleton
{
#if UNITY_EDITOR

#else
    public class SingletonMonoBehaviour<TSingleton> : SerializedMonoBehaviour where TSingleton : SingletonMonoBehaviour<TSingleton>
    {

    }
#endif
}