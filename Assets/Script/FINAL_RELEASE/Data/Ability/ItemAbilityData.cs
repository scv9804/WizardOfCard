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

    [SerializeField, TitleGroup("ȿ�� ������")]
    private List<string> _ability;

    // ============================================================ EventListener

    [SerializeField, TitleGroup("������ ���� �̺�Ʈ")]
    private Event _itemEquip;

    [SerializeField, TitleGroup("������ ���� �̺�Ʈ")]
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

    [SerializeField, TitleGroup("ȿ�� ������")]
    private List<string> _ability;

    // ============================================================ EventListener

    [SerializeField, TitleGroup("������ ���� �̺�Ʈ")]
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

// �Ҹ�ǰ: ���
// ���: ����/����

// �Ҹ�ǰ/��� ���� ȿ�� �и�
// ������ �Ŵ����� ������ ���� ���� �̺�Ʈ�� �����ؼ� ���
// ?????

// �ƴѰ� �׳� ��Ʈ��Ƽ�� ����ϳ�

// ================================================================================ ChangePlayerStat_TEMP

//[Serializable]
//public class ChangePlayerStat_TEMP : ItemAbilityData
//{
//    // ================================================================================ Field

//    // ============================================================ Item

//    [SerializeField, TitleGroup("���� ����")]
//    private string _format;

//    // ============================================================ Ability

//    [SerializeField, TitleGroup("�ɷ�ġ ����")]
//    private Stats _type;

//    [SerializeField, TitleGroup("�ɷ�ġ ����")]
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