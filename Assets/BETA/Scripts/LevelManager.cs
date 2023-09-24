using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;

using Sirenix.OdinInspector;

// 리스트에서 뺴내서 만들거에요

namespace BETA
{
    // ==================================================================================================== LevelManager

    public sealed class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Level

        [FoldoutGroup("맵 설정")]
        private int _maxX = 4;

        [FoldoutGroup("맵 설정")]
        private int _maxY = 4;

        // ==================================================================================================== Method

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Level Manager";

                DontDestroyOnLoad(gameObject);
            }

            return isEmpty;
        }
    } 
}
