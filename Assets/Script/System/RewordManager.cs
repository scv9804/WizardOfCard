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
	public List<GameObject> rewardObjectList = new List<GameObject>();
	public List<Item_inven> rewardList = new List<Item_inven>();

	[SerializeField] ItemDataBase database;
	[SerializeField] GameObject rewardWindow;
	[SerializeField] Button acceptButton;
	[SerializeField] RewardScrollView rewardSpawn;
	[SerializeField] Inventory inven;

	private void Start()
	{
		GameClear();
	}

	public void GameClear()
	{
		Debug.Log("보상");
		UIManager.Inst.MapClearUI();
		acceptButton.onClick.AddListener(AddClearReword);
		SetRewardTable();
		SetRandomReward();
	}

	void SetRandomReward()
	{
		rewardObjectList.Add(rewardSpawn.SetReward(itemList[0]));
		rewardList.Add(itemList[0]);
		itemList.RemoveAt(0);
	}

	void SetRewardTable()
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
	public void GiveReward()
	{
		for (int i = 0; i<rewardObjectList.Count; i++)
		{
			Toggle temptoggle = rewardObjectList[i].GetComponentInChildren<Toggle>();
			if (temptoggle.isOn)
			{
				inven.AddItem(rewardList[i].Id);
				Debug.Log("잘들어갔어요!");
			}
		}

		rewardList.Clear();
		rewardObjectList.Clear();
		rewardSpawn.ClearViewList();
	}

	public void AddClearReword() 
	{


		acceptButton.onClick.RemoveListener(AddClearReword);
	}
}
