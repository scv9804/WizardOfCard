using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Item_Card
{
	//public int itemCode; //<<22-11-05 장형용 :: 제거>>
	public Card card;
	public GameObject card_object;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptalbe Object/ItemSO")]
public class ItemSO : ScriptableObject
{
	public Item_Card[] items;
} 