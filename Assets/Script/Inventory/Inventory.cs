using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory Inst { get; set; }

	public delegate void OnChangeItem();
	public OnChangeItem onChangeItem;

	const int INVENMAX = 25;

	private void Awake()
	{
		Inst = this;
	}

	private void Start()
	{
		slotNumbering();
		onChangeItem += ReDrowSlots;
	}

	public List<Item_Inven> items = new List<Item_Inven>();
	public Slot [] slots;

	
	

	public void ReDrowSlots()
	{
		for (int i = 0; slots.Length > i; i++)
		{
			slots[i].RemoveSlot();
		}
		for (int i =0; items.Count > i; i++)
		{
			slots[i].item = items[i];
			slots[i].UpdateSlotUI();
		}
	}

	void slotNumbering()
	{
		for (int i = 0; slots.Length > i; i++)
		{
			slots[i].slotnum = i;
		}
	}

	public bool Additem(Item_Inven _item)
	{
		if (items.Count < slots.Length)
		{
			items.Add(_item);
			if (onChangeItem != null)
				onChangeItem.Invoke();
			return true;
		}
		return false;
	}


	public void RemoveItem(int _index)
	{
		items.RemoveAt(_index);
		onChangeItem.Invoke();
	}

}



