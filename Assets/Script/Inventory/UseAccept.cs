using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAccept : MonoBehaviour
{
	private Item_inven item;
	private string data;
	public ItemData itemData;
	[SerializeField]
	private GameObject useAccept;

	void Start()
	{
		useAccept.SetActive(false);
	}

	public void Activate(Item_inven item)
	{
		this.item = item;
		ConstructDataString();
		useAccept.SetActive(true);
		Debug.Log(item);
	}

	public void Deactivate()
	{
		useAccept.SetActive(false);
	}

	public void ConstructDataString()
	{
		data = item.Title + "을(를) 사용 하시겠습니까?";
		useAccept.transform.GetChild(0).GetComponent<Text>().text = data;
	}

	public void Ybutton()
	{
		itemData.ItemUse(item);
	}
}
