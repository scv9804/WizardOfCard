using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IPointerUpHandler
{
	public Item_Inven item;
	public Image itemIcon;
	public int slotnum;

	public void UpdateSlotUI()
	{
		itemIcon.sprite = item.itemIcon;
		itemIcon.gameObject.SetActive(true);
	}
	public void RemoveSlot()
	{
		item = null;
		itemIcon.gameObject.SetActive(false);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		bool isUse = item.Use();
		if (isUse)
		{
			Inventory.Inst.RemoveItem(slotnum);
		}
	}
}
