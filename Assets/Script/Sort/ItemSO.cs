using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Item_Card
{
	public string itemName;
	public int itemCode;
	public Card card;
	public GameObject card_object;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptalbe Object/ItemSO")]
public class ItemSO : ScriptableObject
{
	public Item_Card[] items;
} 