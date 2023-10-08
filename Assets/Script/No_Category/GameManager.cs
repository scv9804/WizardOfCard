using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

//

public class GameManager : Singleton<GameManager>
{
    //

    //

    public static event Action OnApplicationQuit;

    public static event Action OnGameStart;

    //

    //

    static GameManager()
    {
        OnApplicationQuit = null;

        OnGameStart = null;
    }

    //

    protected override void Initialize()
    {
        base.Initialize();

        SceneManager.sceneLoaded -= OnSceneWasLoaded;
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading")
        {
            return;
        }

        "¾À º¯ÇÔ?".Log();
    }

    public void Loading(string name, Action callback = null)
    {
        if (UIManager.Inst != null && UIManager.Inst.maincanvas.isActiveAndEnabled)
        {
            UIManager.Inst.maincanvas.enabled = false;
        }

        SceneManager.LoadScene("Loading");

        StartCoroutine(Main(name, callback));

        #region IEnumerator Main(string name, Action callback);

        IEnumerator Main(string name, Action callback)
        {
            yield return null;

            var progressBar = GameObject.Find("Progress").GetComponent<Image>();

            var op = SceneManager.LoadSceneAsync(name);

            op.allowSceneActivation = false;
            op.completed += (op) =>
            {
                callback?.Invoke();
            };

            yield return StartCoroutine(DrawLoadingBar(op, progressBar));
        }

        #endregion

        #region IEnumerator DrawLoadingBar(AsyncOperation op, Image progressBar);

        IEnumerator DrawLoadingBar(AsyncOperation op, Image progressBar)
        {
            var timer = 0.0f;

            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;

                if (op.progress < 0.9f)
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

                    if (progressBar.fillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                    if (progressBar.fillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;

                        yield return null;
                    }
                }
            }
        } 

        #endregion
    }

    public void GameStart()
    {
        OnGameStart?.Invoke();
    }

    public void Quit()
    {
        OnApplicationQuit?.Invoke();
    }
}
