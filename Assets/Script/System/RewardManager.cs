using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using BETA;
using BETA.Data;
using BETA.Singleton;

using Sirenix.OdinInspector;

using UnityEngine.SceneManagement;

using TacticsToolkit;

public class RewardManager : SingletonMonoBehaviour<RewardManager>
{
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

	[SerializeField, TitleGroup("Ŭ���� �� ȹ�� �ݾ�")]
	private int _moneyReward;

	[SerializeField, TitleGroup("Ŭ���� �� ȹ�� �ݾ�")]
	private int _minEarnMoney = 4;

	[SerializeField, TitleGroup("Ŭ���� �� ȹ�� �ݾ�")]
	private int _maxEarnMoney = 9;

	[SerializeField, TitleGroup("������Ŵ��� �̺�Ʈ")]
	private RewardManagerEvent _events;

	public int MoneyReward
    {
		get => _moneyReward;

		set => _moneyReward = value;
	}

	private void Start()
	{
		SetRandomRewardTable();
	}

    private void OnEnable()
    {
		_events.OnEnemyDie.Listener += OnEnemyDie;

		_events.OnBattleEnd.Listener += OnBattleEnd;
	}

    private void OnDisable()
    {
		_events.OnEnemyDie.Listener -= OnEnemyDie;

		_events.OnBattleEnd.Listener -= OnBattleEnd;
	}

    protected override bool Initialize()
    {
		var isEmpty = base.Initialize();

		if (isEmpty)
        {
			DontDestroyOnLoad(this);

			SceneManager.sceneLoaded -= OnSceneWasLoaded;
			SceneManager.sceneLoaded += OnSceneWasLoaded;
		}

		return isEmpty;
	}

    protected override bool Finalize()
    {
		var isEmpty = base.Finalize();

		if (!isEmpty)
		{
			DontDestroyOnLoad(this);

			SceneManager.sceneLoaded -= OnSceneWasLoaded;
		}

		return isEmpty;
	}

    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
		//_moneyReward = 0;
	}

	private void OnEnemyDie(EnemyController enemy)
    {
		var earn = Random.Range(_minEarnMoney, _maxEarnMoney + 1) * 10;

		_moneyReward += earn;
	}

	private void OnBattleEnd()
    {
		//EntityManager.Instance.Money += _moneyReward;

		EntityManager.Instance.SetMoney(_moneyReward);

		var dataSet = BETA.DataManager.Instance.GetDataSet<CardDataSet>();

		var serialID = Random.Range(0, dataSet.Data.Length);

		BETA.CardManager.Instance.Cards[CardManager.OWN].Add(new BETA.Card(null, serialID));

		_moneyReward = 0;
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

	//��纸�� ����
	public void AddReward(SpawnPattern _pattern)
	{
		// ������ ����
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

		// ������
		if (!_pattern.MoneyRandom)
		{
			rewardMoney = _pattern.Reward_Money;
		}
		else
		{
			rewardMoney = UnityEngine.Random.Range(_pattern.Reward_Money - _pattern.Reward_Money / 2
				, _pattern.Reward_Money + _pattern.Reward_Money / 2);
		}

		//ī�弳��

	}

	//������
	void SetMoney()
	{
		moneyObject = rewardSpawn.SetReward(rewardMoney);
	}
	
	void SetRandomMoney()
	{
		rewardMoney = UnityEngine.Random.Range(10,30);
		moneyObject = rewardSpawn.SetReward(rewardMoney);
	}

	//�����ۼ���
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

	//���� ������ ����
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

	//���� ���̺� ����
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

	//�� �� �־��ֱ�
	public void GiveReward()
	{
		for (int i = 0; i<rewardObjectList.Count; i++)
		{
			Toggle temptoggle = rewardObjectList[i].GetComponentInChildren<Toggle>();

			if (temptoggle.isOn)
			{
				//inven.AddItem(rewardList[i].Id);
				Debug.Log("�ߵ����!");
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
			Debug.Log(206 + "���׼�����");
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
	//[Tooltip("3������ ����")] public Enemy[] enemy;
	public int[] Reward_item;
	public int Reward_Money;
	public int[] Reward_Card;
	public bool MoneyRandom;
}