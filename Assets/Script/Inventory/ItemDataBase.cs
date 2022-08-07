using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
	public static ItemDataBase inst;
	private void Awake()
	{
		inst = this;
	}
	public List <Item_Inven> item_Invens = new List<Item_Inven>();
}
