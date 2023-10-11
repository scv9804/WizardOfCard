using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using TacticsToolkit;

// ==================================================================================================== RewardManagerEvent

public class RewardManagerEvent : SerializedMonoBehaviour
{
    // ==================================================================================================== Field

    // =========================================================================== EventDispatcher

    // ======================================================= Battle

    [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
    private EventDispatcher<EnemyController> _onEnemyDie;

    [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
    private EventDispatcher _onEnemyAllDead;

    [SerializeField, TitleGroup("�������� ���� �̺�Ʈ")]
    private EventDispatcher _onBattleEnd;

    // ==================================================================================================== Property

    // =========================================================================== EventDispatcher

    // ======================================================= Battle

    public EventDispatcher<EnemyController> OnEnemyDie
    {
        get => _onEnemyDie;

        private set => _onEnemyDie = value;
    }

    public EventDispatcher OnEnemyAllDead
    {
        get => _onEnemyAllDead;

        private set => _onEnemyAllDead = value;
    }

    public EventDispatcher OnBattleEnd
    {
        get => _onBattleEnd;

        private set => _onBattleEnd = value;
    }
}