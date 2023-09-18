using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== GameManager

    public sealed class GameManager : SingletonMonoBehaviour<GameManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [FoldoutGroup("게임 설정")]
        public GameConfigs Configs;

        // ==================================================================================================== Method

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
    }
}