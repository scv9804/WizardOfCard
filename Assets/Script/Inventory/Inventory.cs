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
	[SerializeField] Image wandImage;
	[SerializeField] Image bookImage;
	[SerializeField] Image hatImage;
	[SerializeField] Image suitImage;
	[SerializeField] Image ringImage;
	[SerializeField] Image posion_1;
	[SerializeField] Image posion_2;
	[SerializeField] Image posion_3;

	#region inven
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
	#endregion


	#region Equiment

	public void EqumentRefresh()
	{

	}

	#endregion

}



