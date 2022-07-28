using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Item
{
	public int itemCode;
	public Card card;
	public GameObject card_object;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptalbe Object/ItemSO")]
public class ItemSO : ScriptableObject
{
	public Item[] items;
} 