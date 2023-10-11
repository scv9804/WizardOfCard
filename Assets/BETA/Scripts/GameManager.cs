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

        //public static event Action OnGameQuit;

        //public static event Action OnGameStart;
        //public static event Action OnGameEnd;

        //public static event Action OnStageStart;
        //public static event Action OnStageEnd;

        //public static event Action OnBattleStart;
        //public static event Action OnBattleEnd;

        [SerializeField, TitleGroup("게임매니저 이벤트")]
        private GameManagerEvent _events;

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        //static GameManager()
        //{
        //    OnGameQuit = null;

        //    OnGameStart = null;
        //    OnGameEnd = null;

        //    OnStageStart = null;
        //    OnStageEnd = null;

        //    OnBattleStart = null;
        //    OnBattleEnd = null;
        //}

        // =========================================================================== Event

        private void OnEnable()
        {
            _events.OnGameQuit.Listener += GameQuit;
        }

        private void OnDisable()
        {
            _events.OnGameQuit.Listener -= GameQuit;
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
                while (!op.isDone)
                {
                    progressBar.fillAmount = op.progress < 0.9f ? op.progress : 1f;

                    if (progressBar.fillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;

                        yield return null;
                    }

                    yield return null;
                }
            }

            #endregion
        }

        //

        public void StartNewGame()
        {
            GameStart();
            StageStart();
        }

        public void GameStart()
        {
            //OnGameStart?.Invoke();

            _events.OnGameStart.Launch();
        }

        public void GameEnd()
        {
            //OnGameEnd?.Invoke();

            _events.OnGameEnd.Launch();
        }

        public void StageStart()
        {
            //OnStageStart?.Invoke();

            _events.OnStageStart.Launch();
        }

        public void StageEnd()
        {
            //OnStageEnd?.Invoke();

            _events.OnStageEnd.Launch();
        }

        public void BattleStart()
        {
            //OnBattleStart?.Invoke();

            _events.OnBattleStart.Launch();
        }

        public void BattleEnd()
        {
            //OnBattleEnd?.Invoke();

            _events.OnBattleEnd.Launch();
        }

        public void GameQuit()
        {
            //OnGameQuit?.Invoke();

            //_events.OnGameQuit.Launch();

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

// OnSceneLoaded => Start => OnGameStart => OnStageStart