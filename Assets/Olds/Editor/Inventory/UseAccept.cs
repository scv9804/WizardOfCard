using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAccept : MonoBehaviour
{
	public Item_inven item;
	private string data;
	public ItemData itemData;
	public Inventory inv;
	[SerializeField]
	private GameObject useAccept;

	void Start()
	{
		inv = GameObject.Find("InventorySystem").GetComponent<Inventory>();
		useAccept.SetActive(false);
	}

	public void Activate(Item_inven item)
	{
		this.item = item;
		ConstructDataString();
		useAccept.SetActive(true);
	}

	public void Deactivate()
	{
		useAccept.SetActive(false);
	}

	public void ConstructDataString()
	{
		if (item != null && !item.Equipable)
		{
			data = item.Title + "을(를) 사용 하시겠습니까?";
			useAccept.transform.GetChild(0).GetComponent<Text>().text = data;
		}
		else if (item != null && item.Equipable && itemData.slotId < 10)
		{
			data = item.Title + "을(를) 장착 하시겠습니까?";
			useAccept.transform.GetChild(0).GetComponent<Text>().text = data;
		}
		else if (item != null && item.Equipable && itemData.slotId >= 10)
		{
			data = item.Title + "을(를) 장착해제 하시겠습니까?";
			useAccept.transform.GetChild(0).GetComponent<Text>().text = data;
		}
	}

	public void Ybutton()
	{
		inv.ItemUse(item);
	}
}
