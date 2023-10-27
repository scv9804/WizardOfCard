using Sirenix.OdinInspector;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// ================================================================================ ItemData

[CreateAssetMenu(menuName = "TEMP/Data/Item")]
public class ItemData : SerializedScriptableObject
{
    // ================================================================================ Field

    // ============================================================ Item

    [SerializeField, TitleGroup("������ ������")]
    private string _name;

    //[SerializeField, TitleGroup("������ ������")]
    //private ItemType _type;

    [SerializeField, TitleGroup("������ ������"), TextArea]
    private string _description;

    // ============================================================ Ability

    //[SerializeField, TitleGroup("ȿ�� ������")]
    //private List<ItemAbilityData> _ability = new List<ItemAbilityData>();

    // ================================================================================ Property

    // ============================================================ Item

    public string Name
    {
        get => _name;
    }

    //public ItemType Type
    //{
    //    get => _type;
    //}

    public string Description
    {
        get => _description;
    }

    // ============================================================ Ability

    //public IReadOnlyList<ItemAbilityData> Ability => _ability;
}