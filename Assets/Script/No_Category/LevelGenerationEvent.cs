using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ==================================================================================================== LevelGenerationEvent

public class LevelGenerationEvent : SerializedMonoBehaviour
{
    // ==================================================================================================== Field

    // =========================================================================== EventDispatcher

    // ======================================================= Game

    [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
    private EventDispatcher _onGameEnd;

    // ======================================================= Stage

    [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
    private EventDispatcher _onStageStart;

    // ======================================================= Room

    [SerializeField, TitleGroup("���� ���� �̺�Ʈ")]
    private EventDispatcher<bool> _onShopEnter;

    // ==================================================================================================== Property

    // =========================================================================== EventDispatcher

    // ======================================================= Game

    public EventDispatcher OnGameEnd
    {
        get => _onGameEnd;

        private set => _onGameEnd = value;
    }

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