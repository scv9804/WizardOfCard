using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace BETA.Editor
{
    public class AbilitySceneLoader : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene("CardAbility");
        }
    } 
}
