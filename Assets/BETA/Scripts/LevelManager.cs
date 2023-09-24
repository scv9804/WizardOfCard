using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;

using Sirenix.OdinInspector;

// ����Ʈ���� ������ ����ſ���

namespace BETA
{
    // ==================================================================================================== LevelManager

    public sealed class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Level

        [FoldoutGroup("�� ����")]
        private int _maxX = 4;

        [FoldoutGroup("�� ����")]
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
