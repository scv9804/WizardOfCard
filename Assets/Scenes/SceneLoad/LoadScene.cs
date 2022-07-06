using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    protected static LoadScene instance;
    public static LoadScene Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadScene>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }



    [SerializeField]
    private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField]
    private Image progressBar;

    private string loadSceneName;



    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static LoadScene Create()
    {
        var SceneLoaderPrefab = Resources.Load<LoadScene>("SceneLoader");
        return Instantiate(SceneLoaderPrefab);
    }

}
