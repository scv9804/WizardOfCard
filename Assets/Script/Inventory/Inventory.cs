using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory inst;
	void Awake()
	{
		inst = this;
	}

	public GameObject inventoryPanel;
	public GameObject slotPanel;
	public GameObject equipslotPanel;
	[SerializeField]ItemDataBase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	private int slotAmount;
	public List<Item_inven> items = new List<Item_inven>();
	public List<Item_inven> equipitems = new List<Item_inven>();
	public List<Item_inven> quickitems = new List<Item_inven>();
	public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> equipslots = new List<GameObject>();
	public List<GameObject> quickslots = new List<GameObject>();

	void Start()
	{
		slotAmount = 10;
		//inventoryPanel = GameObject.Find("inventorys");
		//slotPanel = inventoryPanel.transform.Find("Slots").gameObject;

		//슬롯생성하기
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

		//테스트용 아이템 추가
		AddItem(0);
		AddItem(1);
		AddItem(2);
		AddItem(3);
		AddItem(4);

		Debug.Log(items[7].Title);
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
		equipslots.Add(Instantiate(inventorySlot));
		equipslots[i].transform.SetParent(equipslotPanel.transform.GetChild(i).transform);
		equipslots[i].transform.localPosition = Vector2.zero;
		equipslots[i].GetComponent<Slot>().id = i+10;
	}
	public void MakeQuickSlot(int i)
    {
		quickitems.Add(new Item_inven());
		quickslots.Add(Instantiate(inventorySlot));
		quickslots[i].transform.SetParent(equipslotPanel.transform.GetChild(i+5).transform);
		quickslots[i].transform.localPosition = Vector2.zero;
		quickslots[i].GetComponent<Slot>().id = i + 15;
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

}
