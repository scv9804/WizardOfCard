using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot_q : MonoBehaviour
{
	public int id;
	private Inventory inv;

	void Start()
	{
		inv = GameObject.Find("InventorySystem").GetComponent<Inventory>();
		this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}
}