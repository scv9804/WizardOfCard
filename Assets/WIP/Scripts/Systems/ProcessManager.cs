using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // 턴 매니저랑 통합해야 하나?
    public class ProcessManager : MonoSingleton<ProcessManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Process

        private List<IEnumerator> _process = new List<IEnumerator>();

        [Header("현재 처리 순번")]
        [SerializeField] private int _standby = 1;

        // =========================================================================== Terminate

        private IEnumerator _isTerminate = null;

        // ==================================================================================================== Property

        // =========================================================================== Singleton

        protected override string Name
        {
            get
            {
                return "Process Manager";
            }
        }

        // =========================================================================== Process

        public int Standby
        {
            get
            {
                return _standby;
            }

            private set
            {
                _standby = value;
            }
        }

        public bool IsRunning
        {
            get
            {
                return !_process.Contains(_isTerminate);
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Update()
        {
            ////////////////////////////////////////////////// BETA
            if (Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(Terminate());
            }
            ////////////////////////////////////////////////// BETA
        }

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        // =========================================================================== Process

        public Coroutine AddTask(IEnumerator task)
        {
            Coroutine routine;

            if (IsRunning)
            {
                _process.Add(task);

                routine = StartCoroutine(Processing(task));
            }
            else
            {
                routine = null;
            }

            return routine;
        }

        private IEnumerator Processing(IEnumerator task)
        {
            yield return StartCoroutine(Wait());

            yield return StartCoroutine(task);

            yield return Standby += 1;

            // 작업 하나 종료할 때마다 Terminate 발동 여부 검사할지 고민 중

            IEnumerator Wait()
            {
                int standby = _process.Count;

                #region ONLY_UNITY_EDITOR :: {task.ToString()}의 처리 순번: {standby}
#if UNITY_EDITOR
                Debug.Log($"{task.ToString()}의 처리 순번: {standby}");
#endif
                #endregion

                yield return new WaitUntil(() =>
                {
                    return standby == Standby;
                });
            }
        }

        public IEnumerator Terminate()
        {
            if (!IsRunning)
            {
                yield break;
            }

            yield return AddTask(_isTerminate = Main());

            IEnumerator Main()
            {
                _process.Clear();
                Standby = 0;

                yield return _isTerminate = null;
            }
        }
    }
}
