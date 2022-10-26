using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IDropHandler
{
	public int id;
	private Inventory inv;

	void Start()
	{
		inv = GameObject.Find("InventorySystem").GetComponent<Inventory>();
		this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	public void OnDrop(PointerEventData eventData)
	{
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
		Debug.Log(id);
		if (inv.items[id].Id == -1)
		{
			Debug.Log(inv.items[id].Id);
			inv.items[droppedItem.slotId] = new Item_inven();
			inv.items[id] = droppedItem.item;
			droppedItem.slotId = id;
		}
		else if (droppedItem.slotId != id)
		{
			Debug.Log(inv.items[id].Id);
			Transform item = this.transform.GetChild(0);
			Debug.Log(item);
			item.GetComponent<ItemData>().slotId = droppedItem.slotId;
			item.transform.SetParent(inv.slots[droppedItem.slotId].transform);
			item.transform.position = inv.slots[droppedItem.slotId].transform.position;

			droppedItem.slotId = id;
			droppedItem.transform.SetParent(this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items[droppedItem.slotId] = item.GetComponent<ItemData>().item;
			inv.items[id] = droppedItem.item;
		}
	}
}