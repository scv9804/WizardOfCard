using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Item

[Serializable]
public class Item : Unit
{
    // ================================================================================ Field

    // ============================================================ Item

    [ShowInInspector, TitleGroup("아이템 데이터")]
    private string _name;

    [ShowInInspector, TitleGroup("아이템 데이터")]
    private string _description;

    // ================================================================================ Property

    // ============================================================ Item

    public string Name
    {
        get => _name;

        set => _name = value;
    }

    public string Description
    {
        get => _description;

        set => _description = value;
    }

    // ================================================================================ Method

    // ============================================================ Constructor

    private Item() : base()
    {

    }

    public Item(string instanceID, int serialID) : base(instanceID, serialID)
    {

    }

    // ============================================================ Unit

    public override void Refresh()
    {

    }
}

//// ================================================================================ ItemType

//[Serializable]
//public abstract class ItemType
//{

//}

//// ================================================================================ Equipment

//[Serializable]
//public class Equipment : ItemType
//{
//    //

//    //

//    private bool _isEquipable;

//    private bool _isEquiped;
//}

//// ================================================================================ Consumable

//[Serializable]
//public class Consumable : ItemType
//{
//    //

//    //

//    private bool _isUsable;
//}

//// ================================================================================ Special

//public class Special : ItemType
//{

//}