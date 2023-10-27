using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// ================================================================================ ItemManager

public class ItemManager : Singleton<ItemManager>
{
    // ================================================================================ Constance

    // ============================================================ Item

    //public const string OWN = "OWN";
    //public const string DECK = "DECK";
    //public const string HAND = "HAND";
    //public const string DISCARD = "DISCARD";
    //public const string EXCLUDE = "EXCLUDE";

    //public const string SHOP = "SHOP";
    //public const string EVENT = "EVENT";
    //public const string REWARD = "REWARD";

    // ================================================================================ Field

    // ============================================================ Item

    // ======================================== Inventory

    [SerializeField, TitleGroup("인벤토리")]
    private Item[] _inventory = new Item[25];

    // ======================================== Equipment

    [SerializeField, TitleGroup("장착 아이템")]
    private Item _cloth;

    [SerializeField, TitleGroup("장착 아이템")]
    private Item _earring;

    [SerializeField, TitleGroup("장착 아이템")]
    private Item _hat;

    [SerializeField, TitleGroup("장착 아이템")]
    private Item _ring;

    [SerializeField, TitleGroup("장착 아이템")]
    private Item _wand;

    // ======================================== Consumable

    [SerializeField, TitleGroup("소모품 아이템")]
    private Item[] _quick = new Item[3];

    // ================================================================================ Property

    // ============================================================ Item

    // ======================================== Inventory

    public Item[] Inventory
    {
        get => _inventory;

        set => _inventory = value;
    }
}