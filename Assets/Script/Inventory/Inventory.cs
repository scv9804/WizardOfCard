using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public GameObject inventoryPanel;
	public GameObject slotPanel;
	[SerializeField]ItemDataBase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	private int slotAmount;
	public List<Item_inven> items = new List<Item_inven>();
	public List<GameObject> slots = new List<GameObject>();

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
		//테스트용 아이템 추가
		AddItem(0);
		AddItem(1);
		AddItem(2);
		AddItem(0);
		AddItem(1);
		AddItem(2);

		Debug.Log(items[7].Title);
	}

	public void MakeSlot(int i)
    {
		items.Add(new Item_inven());
		slots.Add(Instantiate(inventorySlot));
		slots[i].GetComponent<Slot>().id = i;
		slots[i].transform.SetParent(slotPanel.transform);
	}
	public void AddItem(int id)
	{
		Item_inven itemToAdd = database.FetchItemById(id);
		/*if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))           //아이템 스택 안씀
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Id == id)
				{
					ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
					data.amount++;
					data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
					break;
				}
			}
		}
		else
		{*/
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Id == -1)
				{
					items[i] = itemToAdd;
					GameObject itemObj = Instantiate(inventoryItem);
					itemObj.GetComponent<ItemData>().item = itemToAdd;
					itemObj.GetComponent<ItemData>().slotId = i;
					itemObj.transform.SetParent(slots[i].transform);
					itemObj.transform.position = Vector2.zero;
					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					itemObj.name = "Item: " + itemToAdd.Title;
					slots[i].name = "Slot: " + itemToAdd.Title;
					break;
				}
			}
		//}
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
