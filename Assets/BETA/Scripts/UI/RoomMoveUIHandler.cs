using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TacticsToolkit;

using TMPro;

using UnityEngine.UI;

namespace BETA.UI
{
    // ==================================================================================================== RoomMoveUIHandler

    public class RoomMoveUIHandler : UIHandler
    {
        // ==================================================================================================== Field

        // =========================================================================== LevelGeneration

        [SerializeField, TitleGroup("이동 방향")]
        private int _direction;

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Start()
        {
            Refresh();
        }

        // =========================================================================== UI

        public override void Refresh()
        {
            LevelGeneration.Instance.existRoomCheck();

            var isActive = LevelGeneration.Instance.IsMovable[_direction];

            gameObject.SetActive(isActive);
        }
    }
}