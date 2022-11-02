using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewordManager : MonoBehaviour
{
    public static RewordManager Inst { get; set; }

	private void Awake()
	{
		Inst = this;
	}

	public List<Item_inven> itemList = new List<Item_inven>();

	[SerializeField] ItemDataBase database;
	[SerializeField] GameObject rewardWindow;
	[SerializeField] Button acceptButton;
	[SerializeField] RewardScrollView rewardSpawn;
	private void Start()
	{
		GameClear();
	}

	public void GameClear()
	{
		Debug.Log("º¸»ó");
		UIManager.Inst.MapClearUI();
		acceptButton.onClick.AddListener(AddClearReword);
		SetRewordTable();
		SetReward();
	}

	void SetReward()
	{
		rewardSpawn.SetReward(itemList[0]) ;
		itemList.RemoveAt(0);
	}

	void SetRewordTable()
	{
		for (int indexCount = 0; indexCount < database.database.Count; indexCount++)
		{
			for (int addRare = 0; addRare < database.database[indexCount].Rarity ; addRare++ )
			{
				itemList.Add(database.database[indexCount]);
			}
		}

		for (int i = 0; i < itemList.Count; i++)
		{
			int rand = UnityEngine.Random.Range(i, itemList.Count);
			Item_inven temp = itemList[i];
			itemList[i] = itemList[rand];
			itemList[rand] = temp;
		}
	}

	public void AddClearReword() 
	{


		acceptButton.onClick.RemoveListener(AddClearReword);
	}
}
