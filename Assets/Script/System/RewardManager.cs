using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Inst { get; set; }

	private void Awake()
	{
		Inst = this;
	}

	public List<Item_inven> itemList = new List<Item_inven>();
	public List<Item_inven> randomitemList = new List<Item_inven>();
	public List<GameObject> rewardObjectList = new List<GameObject>();
	public List<Item_inven> rewardList = new List<Item_inven>();

	[SerializeField] ItemDataBase database;
	[SerializeField] GameObject rewardWindow;
	[SerializeField] Button acceptButton;
	[SerializeField] RewardScrollView rewardSpawn;
	[SerializeField] Inventory inven;

	private void Start()
	{
		SetRandomRewardTable();
	}
	public void GameClear()
	{
		UIManager.Inst.MapClearUI();
		acceptButton.onClick.AddListener(AddClearReword);
		if (itemList.Count != 0)
		{
			SetReward();
		}
		else
		{
			SetRandomReward();
		}
	}

	public void AddReward(int ID)
	{
		itemList.Add(database.database[ID]);
	}

	void SetReward()
	{
		for (int i = 0;i < itemList.Count ; i++)
		{
			rewardObjectList.Add(rewardSpawn.SetReward(itemList[i]));
			rewardList.Add(itemList[i]);
		}
		itemList.Clear();
	}
	
	public void SetRandomReward()
	{
		int rand = UnityEngine.Random.Range(0, randomitemList.Count);
		rewardObjectList.Add(rewardSpawn.SetReward(randomitemList[rand]));
		rewardList.Add(randomitemList[rand]);
	}

	void SetRandomRewardTable()
	{
		for (int indexCount = 0; indexCount < database.database.Count; indexCount++)
		{
			for (int addRare = 0; addRare < database.database[indexCount].Rarity ; addRare++ )
			{
				randomitemList.Add(database.database[indexCount]);
			}
		}

		for (int i = 0; i < randomitemList.Count; i++)
		{
			int rand = UnityEngine.Random.Range(i, randomitemList.Count);
			Item_inven temp = randomitemList[i];
			randomitemList[i] = randomitemList[rand];
			randomitemList[rand] = temp;
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
		rewardWindow.SetActive(false);
	}

	public void AddClearReword() 
	{
		acceptButton.onClick.RemoveListener(AddClearReword);
		GiveReward();
	}
}
