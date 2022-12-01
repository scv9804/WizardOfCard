using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory inst;
	private UseAccept useAccept;
	public ItemData itemdata;
	public Item_inven item;
	private int targetEquipSlot;

	void Awake()
	{
		inst = this;
		useAccept = GetComponent<UseAccept>();
		itemdata = GetComponent<ItemData>();
	}

	public GameObject inventoryPanel;
	public GameObject slotPanel;
	public GameObject equipslotPanel;
	public GameObject QuickUIPanel;
	[SerializeField]ItemDataBase database;
	public GameObject inventorySlot;
	public GameObject inventorySlot_q;
	public GameObject inventoryItem;

	private int slotAmount;
	public List<Item_inven> items = new List<Item_inven>();
	public List<Item_inven> equipitems = new List<Item_inven>();
	public List<Item_inven> quickitems = new List<Item_inven>();
	public List<Item_inven> quickitemsUI = new List<Item_inven>();
	public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> equipslots = new List<GameObject>();
	public List<GameObject> quickslots = new List<GameObject>();
	public List<GameObject> quickslotsUI = new List<GameObject>();

	void Start()
	{
		slotAmount = 10;
		//inventoryPanel = GameObject.Find("inventorys");
		//slotPanel = inventoryPanel.transform.Find("Slots").gameObject;

		//½½·Ô»ý¼ºÇÏ±â
		for (int i = 0; i < slotAmount; i++)
		{
			MakeSlot(i);
		}
		for (int i = 0; i < 5; i++)
		{
			MakeEquipSlot(i);
		}
		for (int i = 0; i < 3; i++)
		{
			MakeQuickSlot(i);
		}
	}

	public void MakeSlot(int i)
    {
		items.Add(new Item_inven());
		slots.Add(Instantiate(inventorySlot));
		slots[i].GetComponent<Slot>().id = i;
		slots[i].transform.SetParent(slotPanel.transform);
	}
	public void MakeEquipSlot(int i)
	{
		equipitems.Add(new Item_inven());
		equipslots.Add(Instantiate(inventorySlot_q));
		equipslots[i].transform.SetParent(equipslotPanel.transform.GetChild(i).transform);
		equipslots[i].transform.localPosition = Vector2.zero;
		equipslots[i].GetComponent<Slot_q>().id = i+10;
	}
	public void MakeQuickSlot(int i)
    {
		quickitems.Add(new Item_inven());
		quickslots.Add(Instantiate(inventorySlot_q));
		quickitemsUI.Add(new Item_inven());
		quickslots[i].transform.SetParent(equipslotPanel.transform.GetChild(i+5).transform);
		quickslots[i].transform.localPosition = Vector2.zero;
		quickslots[i].GetComponent<Slot_q>().id = i + 15;
		quickslotsUI.Add(Instantiate(inventorySlot_q));
		quickslotsUI[i].transform.SetParent(QuickUIPanel.transform.GetChild(i + 3).transform);
		quickslotsUI[i].transform.localPosition = Vector2.zero;
		quickslotsUI[i].GetComponent<Slot_q>().id = i + 100;
	}
	public void AddItem(int id)
	{
		Item_inven itemToAdd = database.FetchItemById(id);
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == -1)
			{
				items[i] = itemToAdd;
				GameObject itemObj = Instantiate(inventoryItem);
				itemObj.GetComponent<ItemData>().item = itemToAdd;
				itemObj.GetComponent<ItemData>().item.OwnPlayer = true;
				itemObj.GetComponent<ItemData>().slotId = i;
				itemObj.transform.SetParent(slots[i].transform);
				itemObj.transform.localPosition = Vector2.zero;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.name = "Item: " + itemToAdd.Title;
				slots[i].name = "Slot: " + itemToAdd.Title;
				break;
			}
		}
		Debug.Log(itemToAdd.Title + "Added");
	}

	bool CheckIfItemIsInInventory(Item_inven item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == item.Id)
			{
				return true;
			}
		}

		return false;
	}
	public void ItemUse(Item_inven item)
	{
		if (item != null && !item.Equipable && item.OwnPlayer)
		{
			if (itemdata.slotId < 10)
			{
				SetQuickSlot(item);
				useAccept.Deactivate();
			}
			else if (itemdata.slotId >= 15)
			{
				HealItemUsed(item);
				useAccept.Deactivate();
			}
		}
		else if (item != null && item.Equipable && itemdata.slotId < 10 && item.OwnPlayer)
		{
			EquipItem(item);
			useAccept.Deactivate();
		}
		else if (item != null && item.Equipable && itemdata.slotId >= 10 && item.OwnPlayer)
		{
			UnequipItem(item);
			useAccept.Deactivate();
		}
	}
	public void SetQuickSlot(Item_inven item)
	{
		Item_inven QuickitemAdd = item;
		for (int i = 0; i < 3; i++)
		{
			if (quickitems[i].Id == -1)
			{
				itemdata.transform.SetParent(quickslots[i].transform);
				quickitems[i] = item;
				itemdata.transform.localPosition = Vector2.zero;
				items[itemdata.slotId] = new Item_inven();
				useAccept.Deactivate();
				itemdata.slotId = quickslots[i].GetComponent<Slot_q>().id;

				GameObject itemObj = Instantiate(inventoryItem);
				itemObj.GetComponent<ItemData>().item = QuickitemAdd;
				itemObj.GetComponent<ItemData>().item.OwnPlayer = true;
				itemObj.GetComponent<ItemData>().slotId = i + 100;
				itemObj.transform.SetParent(quickslotsUI[i].transform);
				itemObj.transform.localPosition = Vector2.zero;
				itemObj.GetComponent<Image>().sprite = QuickitemAdd.Sprite;
				quickitemsUI[i] = QuickitemAdd;
				break;
			}

		}
	}
	public void HealItemUsed(Item_inven item)
	{
		PlayerEntity.Inst.Status_Health += item.Value;

		if (PlayerEntity.Inst.Status_Health > PlayerEntity.Inst.Status_MaxHealth)
		{
			PlayerEntity.Inst.Status_Health = PlayerEntity.Inst.Status_MaxHealth;
		}
		if (itemdata.slotId < 100)
		{
			Destroy(QuickUIPanel.transform.GetChild(itemdata.slotId - 12).transform.GetChild(0).GetChild(0).gameObject);
			quickitems[itemdata.slotId - 15] = new Item_inven();
			quickitemsUI[itemdata.slotId - 15] = new Item_inven();
		}
		else if (itemdata.slotId >= 100)
		{
			Destroy(equipslotPanel.transform.GetChild(itemdata.slotId - 95).transform.GetChild(0).GetChild(0).gameObject);
			quickitems[itemdata.slotId - 100] = new Item_inven();
			quickitemsUI[itemdata.slotId - 100] = new Item_inven();
		}
		Destroy(itemdata.gameObject);
		useAccept.Deactivate();
	}
	public void EquipItem(Item_inven item)
	{
		switch (item.Type)
		{
			case "Weapon":
				Debug.Log("¹«±â ÀåÂø");
				targetEquipSlot = 0;
				break;
			case "Ear":
				Debug.Log("±Í°ÉÀÌ ÀåÂø");
				targetEquipSlot = 1;
				break;
			case "Hat":
				Debug.Log("¸Ó¸® ÀåÂø");
				targetEquipSlot = 2;
				break;
			case "Suit":
				Debug.Log("¿Ê ÀåÂø");
				targetEquipSlot = 3;
				break;
			case "Ring":
				Debug.Log("¹ÝÁö ÀåÂø");
				targetEquipSlot = 4;
				break;
		}
		if (equipitems[targetEquipSlot].Id != -1)
		{
			Transform equipeditem = equipslots[targetEquipSlot].transform.GetChild(0);
			equipeditem.GetComponent<ItemData>().slotId = itemdata.slotId;
			equipeditem.transform.SetParent(slots[itemdata.slotId].transform);
			equipeditem.transform.position = slots[itemdata.slotId].transform.position;
		}
		itemdata.transform.SetParent(equipslots[targetEquipSlot].transform);
		equipitems[targetEquipSlot] = item;
		itemdata.transform.localPosition = Vector2.zero;
		items[itemdata.slotId] = new Item_inven();
		useAccept.Deactivate();
		itemdata.slotId = equipslots[targetEquipSlot].GetComponent<Slot_q>().id;
		equipslotPanel.transform.GetChild(targetEquipSlot + 8).gameObject.SetActive(false);
	}
	public void UnequipItem(Item_inven item)
	{
		equipslotPanel.transform.GetChild(itemdata.slotId - 2).gameObject.SetActive(true);
		equipitems[itemdata.slotId - 10] = new Item_inven();
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == -1)
			{
				Debug.Log(i);
				itemdata.transform.SetParent(slots[i].transform);
				itemdata.transform.position = slots[i].transform.position;
				items[i] = item;
				itemdata.slotId = i;
				break;
			}
		}
		useAccept.Deactivate();
	}
}
