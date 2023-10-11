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

    [SerializeField, TitleGroup("스테이지 관련 이벤트")]
    private EventDispatcher<EnemyController> _onEnemyDie;

    [SerializeField, TitleGroup("스테이지 관련 이벤트")]
    private EventDispatcher _onEnemyAllDead;

    [SerializeField, TitleGroup("스테이지 관련 이벤트")]
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