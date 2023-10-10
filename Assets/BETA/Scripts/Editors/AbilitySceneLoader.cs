using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace BETA.Editor
{
    public class AbilitySceneLoader : MonoBehaviour
    {
        public string SceneName;

        void Start()
        {
            SceneManager.LoadScene(SceneName);

            //LoadSceneManager.LoadScene(SceneName);
        }
    } 
}
