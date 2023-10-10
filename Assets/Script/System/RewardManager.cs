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
		DontDestroyOnLoad(this);
	}

	//[SerializeField] List<Item_inven> itemList = new List<Item_inven>();
	//[SerializeField] List<Item_inven> randomitemList = new List<Item_inven>();
	[SerializeField] List<GameObject> rewardObjectList = new List<GameObject>();
	//[SerializeField] List<Item_inven> rewardList = new List<Item_inven>();
	//[SerializeField] List<Card> rewardCardList = new List<Card>();
	[SerializeField] List<GameObject> rewardCardObjectList = new List<GameObject>();
	GameObject moneyObject;
	int rewardMoney;

	//[SerializeField] ItemDataBase database;
	[SerializeField] GameObject rewardWindow;
	[SerializeField] Button acceptButton;
	[SerializeField] RewardScrollView rewardSpawn;
	//[SerializeField] Inventory inven;
	[SerializeField] GameObject rewardCard;

	private void Start()
	{
		SetRandomRewardTable();
	}
	public void GameClear()
	{
		UIManager.Inst.MapClearUI();
		acceptButton.onClick.AddListener(AddClearReword);
/*		if (itemList.Count != 0)
		{
			SetReward();
		}
		else*/
		{
			SetRandomReward();
		}
	//	SetCardReward();
	}

	//모든보상 설정
	public void AddReward(SpawnPattern _pattern)
	{
		// 아이템 설정
		if (_pattern.Reward_item.Length != 0)
		{
			for (int i = 0; i < _pattern.Reward_item.Length; i++)
			{
				//itemList.Add(database.database[_pattern.Reward_item[i]]);
			}
		}
		else
		{
			//int randitem = UnityEngine.Random.Range(0, database.notEquiDataBase.Count);
			//itemList.Add(database.notEquiDataBase[randitem]);
		}

		// 돈설정
		if (!_pattern.MoneyRandom)
		{
			rewardMoney = _pattern.Reward_Money;
		}
		else
		{
			rewardMoney = UnityEngine.Random.Range(_pattern.Reward_Money - _pattern.Reward_Money / 2
				, _pattern.Reward_Money + _pattern.Reward_Money / 2);
		}

		//카드설정

	}

	//돈설정
	void SetMoney()
	{
		moneyObject = rewardSpawn.SetReward(rewardMoney);
	}
	
	void SetRandomMoney()
	{
		rewardMoney = UnityEngine.Random.Range(10,30);
		moneyObject = rewardSpawn.SetReward(rewardMoney);
	}

	//아이템설정
	void SetReward()
	{
		//for (int i = 0;i < itemList.Count ; i++)
		//{
		//	rewardObjectList.Add(rewardSpawn.SetReward(itemList[i]));
		//	rewardList.Add(itemList[i]);
		//}
		//itemList.Clear();
		SetMoney();
	}
	
	void SetToolTip()
	{
		//tooltip = inv.GetComponent<Tooltip>();
	}

	//랜덤 아이템 설정
	public void SetRandomReward()
	{
/*		int  count = UnityEngine.Random.Range(0, 2);

		for (int i = 0;i < count ; i++)
		{
			int rand = UnityEngine.Random.Range(0, randomitemList.Count);
			rewardObjectList.Add(rewardSpawn.SetReward(randomitemList[rand]));
			rewardList.Add(randomitemList[rand]);
		}*/
		SetRandomMoney();
	}

	//랜덤 테이블 설정
	void SetRandomRewardTable()
	{
		//for (int indexCount = 0; indexCount < database.database.Count; indexCount++)
		//{
		//	for (int addRare = 0; addRare < database.database[indexCount].Rarity ; addRare++ )
		//	{
		//		randomitemList.Add(database.database[indexCount]);
		//	}
		//}

		//for (int i = 0; i < randomitemList.Count; i++)
		//{
		//	int rand = UnityEngine.Random.Range(i, randomitemList.Count);
		//	Item_inven temp = randomitemList[i];
		//	randomitemList[i] = randomitemList[rand];
		//	randomitemList[rand] = temp;
		//}
	}

	void SetCardReward()
	{
		int randcardcount = Random.Range(1, 2);

		for (int i = 0; i < randcardcount; i++)
		{
			//int randCard = Random.Range(0, CardManager.Inst.itemSO.items.Length - 1);
			//var temt = Instantiate(rewardCard);

			//Card card = CardManager.Inst.itemSO.items[randCard].card;

			//temt.transform.GetChild(0).GetComponent<TMP_Text>().text = card.i_manaCost.ToString();
			//temt.transform.GetChild(1).GetComponent<TMP_Text>().text = card.GetCardName();
			//temt.transform.GetChild(2).GetComponent<TMP_Text>().text = card.GetCardExplain();
			//temt.transform.GetChild(3).GetComponent<Image>().sprite = card.CardIconImage;

			//rewardCardList.Add(card);
			//rewardCardObjectList.Add(rewardSpawn.SetReward(temt));
		}
	}

	//보 상 넣어주기
	public void GiveReward()
	{
		for (int i = 0; i<rewardObjectList.Count; i++)
		{
			Toggle temptoggle = rewardObjectList[i].GetComponentInChildren<Toggle>();

			if (temptoggle.isOn)
			{
				//inven.AddItem(rewardList[i].Id);
				Debug.Log("잘들어갔어요!");
			}
		}

		Toggle toggle = moneyObject.GetComponentInChildren<Toggle>();

		if (toggle.isOn)
		{
			CharacterStateStorage.Inst.money += rewardMoney;

			//UIManager.Inst.PlayerMoneyUIRefresh();
		}

		//for (int i = 0; i < rewardCardList.Count; i++)
		//{
		//	Toggle cardtoggle = rewardCardObjectList[i].GetComponentInChildren<Toggle>();
		//	if (cardtoggle.isOn)
		//	{
		//		CardManager.Inst.AddSelectCard_Deck(rewardCardList[i]);
		//	}
		//}

		try
		{
			UIManager.Inst.MinimapActive();
		}
		catch
		{
			Debug.Log(206 + "버그수정중");
		}
	

		rewardSpawn.destroyRewardObejct();
		moneyObject = null;
		//rewardCardList.Clear();
		rewardCardObjectList.Clear();
		//rewardList.Clear();
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

[System.Serializable]
public class SpawnPattern
{
	//[Tooltip("3개까지 가능")] public Enemy[] enemy;
	public int[] Reward_item;
	public int Reward_Money;
	public int[] Reward_Card;
	public bool MoneyRandom;
}