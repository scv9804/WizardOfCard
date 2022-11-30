using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class ItemDataBase : MonoBehaviour
{

	public List<Item_inven> database = new List<Item_inven>();
	private JsonData itemData;

	void Awake()
	{
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Script/Item/Items.json"));
		ConstructItemDatabase();
	}

	public Item_inven FetchItemById(int id)
	{

		for (int i = 0; i < database.Count; i++)
		{
			if (database[i].Id == id)
			{
				return database[i];
			}
		}

		return null;
	}

	void ConstructItemDatabase()
	{
		for (int i = 0; i < itemData.Count; i++)
		{
			Item_inven newItem = new Item_inven();
			newItem.Id = (int)itemData[i]["id"];
			newItem.Title = itemData[i]["title"].ToString();
			newItem.Value = (int)itemData[i]["value"];
			newItem.Stats = (int)itemData[i]["stats"];
			newItem.Description = itemData[i]["description"].ToString();
			newItem.Equipable = (bool)itemData[i]["equipable"];
			newItem.Type = itemData[i]["ItemType"].ToString();
			newItem.Rarity = (int)itemData[i]["rarity"];
			newItem.Price = (int)itemData[i]["price"];
			newItem.Slug = itemData[i]["slug"].ToString();
			newItem.Sprite = Resources.Load<Sprite>("ItemImages/" + newItem.Slug);

			database.Add(newItem);
		}
	}
}

public class Item_inven
{
	public int Id { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public int Stats { get; set; }
	public string Description { get; set; }
	public bool OwnPlayer { get; set; }
	public bool Equipable { get; set; }
	public string Type { get; set; }
	public int Rarity { get; set; }
	public int Price { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

	public Item_inven()
	{
		this.Id = -1;
	}
}