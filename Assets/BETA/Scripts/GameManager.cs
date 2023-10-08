using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;

using Sirenix.OdinInspector;

using System;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BETA
{
    // ==================================================================================================== GameManager

    public sealed class GameManager : SingletonMonoBehaviour<GameManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [FoldoutGroup("게임 설정")]
        public GameConfigs Configs;

        // =========================================================================== GameEvent

        public static event Action OnGameQuit;

        public static event Action OnGameStart;
        public static event Action OnGameEnd;

        public static event Action OnBattleStart;
        public static event Action OnBattleEnd;

        // ==================================================================================================== Method

        // =========================================================================== Constance

        static GameManager()
        {
            OnGameQuit = null;
        }

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Game Manager";

                DontDestroyOnLoad(gameObject);

                Configs = Resources.Load<GameConfigs>("Data/GameConfigs");
            }

            return isEmpty;
        }

        //

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

                //yield return StartCoroutine(DrawLoadingBar(op, progressBar));

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

            #region IEnumerator DrawLoadingBar(AsyncOperation op, Image progressBar);

            //IEnumerator DrawLoadingBar(AsyncOperation op, Image progressBar)
            //{
            //    var timer = 0.0f;

            //    while (!op.isDone)
            //    {
            //        yield return null;

            //        timer += Time.deltaTime;

            //        if (op.progress < 0.9f)
            //        {
            //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

            //            if (progressBar.fillAmount >= op.progress)
            //            {
            //                timer = 0f;
            //            }
            //        }
            //        else
            //        {
            //            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

            //            if (progressBar.fillAmount == 1.0f)
            //            {
            //                op.allowSceneActivation = true;

            //                yield return null;
            //            }
            //        }
            //    }
            //}

            #endregion
        }

        public void GameStart()
        {
            OnGameStart?.Invoke();
        }

        public void GameEnd()
        {
            OnGameEnd?.Invoke();
        }

        public void BattleStart()
        {
            OnBattleStart?.Invoke();
        }

        public void BattleEnd()
        {
            OnBattleEnd?.Invoke();
        }

        public void Quit()
        {
            OnGameQuit?.Invoke();

            #region Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif 
            #endregion
        }
    }
}