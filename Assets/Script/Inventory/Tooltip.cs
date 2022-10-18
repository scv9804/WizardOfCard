using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	private Item_inven item;
	private string data;
	[SerializeField]
	private GameObject tooltip;

	void Start()
	{
		//tooltip = GameObject.Find("Tooltip");
		tooltip.SetActive(false);
	}

	void Update()
	{
		if (tooltip.activeSelf)
		{
			tooltip.transform.position = Input.mousePosition;
			//Debug.Log(Input.mousePosition);
		}
	}

	public void Activate(Item_inven item)
	{
		this.item = item;
		ConstructDataString();
		tooltip.SetActive(true);
	}

	public void Deactivate()
	{
		tooltip.SetActive(false);
	}

	public void ConstructDataString()
	{
		data = "<color=#FFEC58FF><b>" + item.Title + "</b></color>\n\n" + item.Description
			+ "\nHealing: " + item.Healing;
		tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
	}

}
