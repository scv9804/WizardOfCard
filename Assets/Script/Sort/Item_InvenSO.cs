using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_Inven
{
    public string itemName;
    public string itemExplain;   
    public int itemCode;
    public Sprite itemIcon;
    public Utility_enum.ItemType itemType;
    public List<ItemEffect> efts;

    public bool Use()
	{
        bool isUsed = false;
        isUsed = true;
		foreach (ItemEffect eft in efts)
		{
            isUsed = eft.ExcuteRole();
		}
        return isUsed;
	}
}
