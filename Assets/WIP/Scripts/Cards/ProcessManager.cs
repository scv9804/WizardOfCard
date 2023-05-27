using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    public class ProcessManager : MonoSingleton<ProcessManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Process

        private List<IEnumerator> _process = new List<IEnumerator>();

        [Header("현재 처리 순번")]
        [SerializeField] private int _standby = 1;

        [Header("작업 추가 가능 여부")]
        [SerializeField] private bool _isRunning = true;

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
                return _isRunning;
            }

            private set
            {
                _isRunning = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Singleton

        public override void Initialize()
        {
            base.Initialize();

            DontDestroyOnLoad(gameObject);
        }

        // =========================================================================== Process

        public Coroutine AddTask(IEnumerator prework, IEnumerator main)
        {
            if (IsRunning)
            {
                _process.Add(main);

                return StartCoroutine(Processing(prework, main));
            }
            else
            {
                return null;
            }
        }

        public IEnumerator Processing(IEnumerator prework, IEnumerator main)
        {
            if (prework != null)
            {
                yield return StartCoroutine(prework);
            }

            yield return StartCoroutine(WaitForProcess(main.ToString()));

            yield return StartCoroutine(main);

            yield return Standby += 1;
        }

        public IEnumerator WaitForProcess(string name)
        {
            int standby = _process.Count;

            Debug.Log($"{name}의 처리 순번: {standby}");

            yield return new WaitUntil(() =>
            {
                return standby == Standby;
            });
        }

        public IEnumerator Terminate()
        {
            yield return AddTask(Prework(), Main());

            IEnumerator Prework()
            {
                yield return IsRunning = false;
            }

            IEnumerator Main()
            {
                _process.Clear();
                Standby = 0;

                yield return IsRunning = true;
            }
        }
    }
}
