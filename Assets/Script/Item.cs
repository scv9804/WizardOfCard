using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemCode;
    public string itemName;
    public string itemExplain;
    public int itemCount;
    public Sprite itemIcon;
    public Utility_enum.ItemType itemType;

 

    public void Setup(Item_Inven item, int _itemCount)
    {
        itemCode = item.itemCode;
        itemName = item.itemName;
        itemExplain = item.itemExplain;
        itemCount = _itemCount;
        itemIcon = item.itemIcon;
    }
}
