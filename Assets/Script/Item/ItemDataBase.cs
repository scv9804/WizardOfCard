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
		Debug.Log(database.Count);
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
			newItem.Power = (int)itemData[i]["stats"]["power"];
			newItem.Defense = (int)itemData[i]["stats"]["defense"];
			newItem.Vitality = (int)itemData[i]["stats"]["vitality"];
			newItem.Healing = (int)itemData[i]["Healing"];
			newItem.Description = itemData[i]["description"].ToString();
			newItem.Equipable = (bool)itemData[i]["equipable"];
			newItem.Rarity = (int)itemData[i]["rarity"];
			newItem.Slug = itemData[i]["slug"].ToString();
			newItem.Sprite = Resources.Load<Sprite>("potion/" + newItem.Slug);

			database.Add(newItem);
		}
	}
}

[System.Serializable]
public class Item_inven
{
	public int Id;
	public string Title;
	public int Value;
	public int Power;
	public int Defense;
	public int Vitality;
	public int Healing;
	public string Description;
	public bool Equipable;
	public int Rarity;
	public string Slug;
	public Sprite Sprite;

	public Item_inven()
	{
		this.Id = -1;
	}
}
