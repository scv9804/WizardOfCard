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
		AcquireItem(ItemDataBase.inst.item_Invens[0]);
		onChangeItem += ReDrowSlots;
	}

	public Slot [] quickSlot;
	public Slot [] slots;
	[SerializeField] Slot wandSlot;
	[SerializeField] Slot bookSlot;
	[SerializeField] Slot hatSlot;
	[SerializeField] Slot suitSlot;
	[SerializeField] Slot ringSlot;
	[SerializeField] Slot posion_1;
	[SerializeField] Slot posion_2;
	[SerializeField] Slot posion_3;

	#region inven
	public void ReDrowSlots()
	{
		for (int i =0; slots.Length > i; i++)
		{
			slots[i].UpdateSlotUI();
		}
		
		Debug.Log("슬롯 다시 그리기");
	}

	public void ReDrowEquiSlot()
	{
		wandSlot.UpdateSlotUI();
		bookSlot.UpdateSlotUI();
		hatSlot.UpdateSlotUI();
		suitSlot.UpdateSlotUI();
		ringSlot.UpdateSlotUI();
		posion_1.UpdateSlotUI();
		posion_2.UpdateSlotUI();
		posion_3.UpdateSlotUI();
	}

	void slotNumbering()
	{
		for (int i = 0; slots.Length > i; i++)
		{
			slots[i].slotnum = i;
		}
	}

	public void AcquireItem(Item_Inven _item)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item.itemName == "")
			{
				slots[i].item = _item;
				slots[i].UpdateSlotUI();
				return;
			}
		}
	}

	#endregion


	public void Equiment()
	{

	}


}
