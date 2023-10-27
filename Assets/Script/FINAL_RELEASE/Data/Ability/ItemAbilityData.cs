using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

using TacticsToolkit;

// ================================================================================ ItemAbilityData

[Serializable]
public abstract class ItemAbilityData
{
    // ================================================================================ Method

    // ============================================================ EventListener

    public abstract void AddListener();

    public abstract void RemoveListener();
}

// ================================================================================ EquipmentAbilityData

[Serializable]
public class EquipmentAbilityData : ItemAbilityData
{
    // ================================================================================ Field

    // ============================================================ Ability

    [SerializeField, TitleGroup("효과 데이터")]
    private List<string> _ability;

    // ============================================================ EventListener

    [SerializeField, TitleGroup("아이템 관련 이벤트")]
    private Event _itemEquip;

    [SerializeField, TitleGroup("아이템 관련 이벤트")]
    private Event _itemUnequip;

    // ================================================================================ Method

    // ============================================================ EventListener

    public override void AddListener()
    {
        foreach (var ability in _ability)
        {
            _itemEquip.AddListener(null);
            _itemUnequip.AddListener(null);
        }
    }

    public override void RemoveListener()
    {
        foreach (var ability in _ability)
        {
            _itemEquip.RemoveListener(null);
            _itemUnequip.RemoveListener(null);
        }
    }
}

// ================================================================================ ConsumableAbilityData

[Serializable]
public class ConsumableAbilityData : ItemAbilityData
{
    // ================================================================================ Field

    // ============================================================ Ability

    [SerializeField, TitleGroup("효과 데이터")]
    private List<string> _ability;

    // ============================================================ EventListener

    [SerializeField, TitleGroup("아이템 관련 이벤트")]
    private Event _itemUse;

    // ================================================================================ Method

    // ============================================================ EventListener

    public override void AddListener()
    {
        foreach (var ability in _ability)
        {
            _itemUse.AddListener(null);
        }
    }

    public override void RemoveListener()
    {
        foreach (var ability in _ability)
        {
            _itemUse.RemoveListener(null);
        }
    }
}

// 소모품: 사용
// 장비: 장착/해제

// 소모품/장비 별로 효과 분리
// 아이템 매니저에 데이터 갖다 놓고 이벤트에 연결해서 사용
// ?????

// 아닌가 그냥 스트래티지 써야하나

// ================================================================================ ChangePlayerStat_TEMP

//[Serializable]
//public class ChangePlayerStat_TEMP : ItemAbilityData
//{
//    // ================================================================================ Field

//    // ============================================================ Item

//    [SerializeField, TitleGroup("설명 포맷")]
//    private string _format;

//    // ============================================================ Ability

//    [SerializeField, TitleGroup("능력치 변동")]
//    private Stats _type;

//    [SerializeField, TitleGroup("능력치 변동")]
//    private int _value;

//    // ================================================================================ Property

//    // ============================================================ Item

//    public string Format => _format;

//    // ============================================================ Ability

//    public Stats Type => _type;

//    public int Value => _value;

//    // ================================================================================ Method

//    // ============================================================ Ability

//    public override void OnAbilityActive()
//    {
//        var stat = BETA.EntityManager.Instance.StatsContainer.getStat(Type);

//        stat.statValue += Value;
//        stat.baseStatValue += Value;
//    }

//    public override void OnAbilityDeactive()
//    {
//        var stat = BETA.EntityManager.Instance.StatsContainer.getStat(Type);

//        stat.statValue -= Value;
//        stat.baseStatValue -= Value;
//    }
//}