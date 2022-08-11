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
		try
		{
			if (isUse)
			{
				Inventory.Inst.RemoveItem(slotnum);
			}
		}
		catch
		{
			Debug.Log((slotnum + 1) + "번쨰의 "+"Slot안에 Item이 없습니다.");
		}

	}
}
