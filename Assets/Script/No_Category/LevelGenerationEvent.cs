using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ==================================================================================================== LevelGenerationEvent

public class LevelGenerationEvent : SerializedMonoBehaviour
{
    // ==================================================================================================== Field

    // =========================================================================== EventDispatcher

    // ======================================================= Stage

    [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
    private EventDispatcher _onStageStart;

    // ======================================================= Room

    [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
    private EventDispatcher<bool> _onShopEnter;

    // ==================================================================================================== Property

    // =========================================================================== EventDispatcher

    // ======================================================= Stage

    public EventDispatcher OnStageStart
    {
        get => _onStageStart;

        private set => _onStageStart = value;
    }

    // ======================================================= Room

    public EventDispatcher<bool> OnShopEnter
    {
        get => _onShopEnter;

        private set => _onShopEnter = value;
    }
}