using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

using BETA;

using Sirenix.OdinInspector;

public class ShopScirpt : MonoBehaviour
{
	[Header("상점 don't destroy")]
	[SerializeField] GameObject shop;

	//[Header("필수설정 상점넣기")]
	//[SerializeField] ItemDataBase database;

	[Header("오브젝트들 설정")]
	[SerializeField] GameObject[] shopAllObject;
	[SerializeField] GameObject shopPanelObject;
	[SerializeField] GameObject shopOwner;
	[SerializeField] GameObject speechBubble;

	[SerializeField] Button shopOwnerButton;
	[SerializeField] Button shopExitButton;
	[SerializeField] Button ManaUpPurchaseButton;

	[SerializeField] TMP_Text shopOwnerTMP;
	[SerializeField] TMP_Text ManaPriceTMP;
	[SerializeField] string[] ShopOwnerspeechArray;

	//[SerializeField] ItemDataBase allItemData;

	[Header("아이템 관련")]
	[SerializeField] GameObject cardSpawnParent;
	[SerializeField] GameObject[] itemSapwnParents;
	[SerializeField] TMP_Text[] priceTMP;

	//List<Item_inven> shopItemList = new List<Item_inven>();
	List<GameObject> solditems = new List<GameObject>();

	Vector3 OriginSize;

	bool isSettingOver = false;
	bool isShopActive = false;
	int manaPrice = 10;

	StringBuilder sb = new StringBuilder();

	WaitForSeconds speechBubbleDelay = new WaitForSeconds(1.5f);

	#region 기본 ON OFF 설정
	private void Start()
	{	
		//shopOwnerButton.onClick.AddListener(OpenShop);
		//shopExitButton.onClick.AddListener(CloseShop);
		//ManaUpPurchaseButton.onClick.AddListener(ManaLevelUp);
		OriginSize = speechBubble.transform.localScale;
		SetUpgradeManaCost();
	}

	public void EnterShop()
	{

		foreach (var item in shopAllObject)
		{
			item.SetActive(true);
		}
		for (int i = 0; i < priceTMP.Length; i++)
		{
			priceTMP[i].gameObject.SetActive(true);
		}
		shopOwner.SetActive(true);
		//CardManager.Inst.SetCardStateCannot();
		StartCoroutine(Repeat());

		SetShop();
	}

	public void ExitShop()
	{
		foreach (var item in shopAllObject)
		{
			item.SetActive(false);
		}
		for (int i = 0; i < priceTMP.Length; i++)
		{
			priceTMP[i].gameObject.SetActive(false);
		}
		shopOwner.SetActive(false);
		//CardManager.Inst.SetCardStateBack();
	}

	public void OpenShop()
	{
		shopPanelObject.SetActive(true);

		// <<22-11-28 장형용 :: 추가>>
		MusicManager.inst.PlayShopSound();
	}
	public void CloseShop()
	{
		shopPanelObject.SetActive(false);

		// <<22-11-28 장형용 :: 추가>>
		MusicManager.inst.PlayShopSound();
	}

	#endregion






	#region 상점 기능 구현부
	IEnumerator Repeat()
	{
		do
		{
			yield return StartCoroutine(SetSpeech());
			if (!shopOwner.activeInHierarchy)
			{
				break;
			}
		} while (true);
	}
	IEnumerator SetSpeech()
	{
		speechBubble.SetActive(true);
		speechBubble.transform.localScale = Vector3.zero;
		speechBubble.transform.DOScale(OriginSize, 0.5f).SetEase(Ease.OutBack);
		int rand = UnityEngine.Random.Range(0, ShopOwnerspeechArray.Length - 1);
		shopOwnerTMP.text = ShopOwnerspeechArray[rand];
		yield return speechBubbleDelay;
		speechBubble.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(0.5f);
		speechBubble.SetActive(false);
		yield return new WaitForSeconds(5f);
	}

	//WIP.CardShopPile test = new WIP.CardShopPile();

	void SetShop()
	{		
		if (!isSettingOver)
		{
			//test.Initialize("SoldCardSort", true);
			//카드 세팅
			for (int i = 0; i < 5; i++)
			{
				//var rand = UnityEngine.Random.Range(0, WIP.CardManager.Instance.Database.Cards.Count);
				//var temtName = WIP.GameManager.Instance.Allocate(WIP.InstanceType.Card);

				//var card = WIP.Card.Create(temtName, rand);

				//test.Add(card);

				//var temtobject = test._cardObjects.Find((cardObject) => cardObject.InstanceID == temtName);
				//temtobject.GroupName = i.ToString();
				//var temt = Instantiate(cardPrefab);


				//temt.name = i.ToString();
				//temt.transform.GetChild(0).GetComponent<TMP_Text>().text = 75.ToString();
				//temt.transform.SetParent(cardSpawnParent.transform);


				//temtobject.GetComponent<Button>().onClick.AddListener(() => SetBuyCard(card, temt));


			}

			//랜덤 아이템 세팅
			for (int i = 0; i < 3; i++)
			{
				int temp = i;
				if (i < 2)
				{
					//int rand = UnityEngine.Random.Range(0, database.notEquiDataBase.Count);
					//Item_inven itemToAdd = database.FetchItemById(database.notEquiDataBase[rand].Id);

					//GameObject itemObj = Instantiate(itemPrefab);
					//itemObj.GetComponent<ItemData>().item = itemToAdd;
					//itemObj.GetComponent<ItemData>().item.OwnPlayer = false;
					//itemObj.transform.SetParent(itemSapwnParents[i].transform);
					//itemObj.transform.localPosition = Vector2.zero;
					//itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					//itemObj.name = "Item: " + itemToAdd.Title;
					//itemObj.transform.localScale *= 3;
					//itemObj.AddComponent<Button>();
					//itemObj.GetComponent<Button>().onClick.AddListener(() => SetBuyItem(itemToAdd, temp));
					//solditems.Add(itemObj);
					//priceTMP[i].text = itemToAdd.Price.ToString();
				}
				else
				{
					//int rand = UnityEngine.Random.Range(0, database.equiDataBase.Count);
					//Item_inven itemToAdd = database.FetchItemById(database.equiDataBase[rand].Id);

					//GameObject itemObj = Instantiate(itemPrefab);
					//itemObj.GetComponent<ItemData>().item = itemToAdd;
					//itemObj.GetComponent<ItemData>().item.OwnPlayer = false;
					//itemObj.transform.SetParent(itemSapwnParents[i].transform);
					//itemObj.transform.localPosition = Vector2.zero;
					//itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					//itemObj.name = "Item: " + itemToAdd.Title;
					//itemObj.transform.localScale *= 3;
					//itemObj.AddComponent<Button>();
					//itemObj.GetComponent<Button>().onClick.AddListener(() => SetBuyItem(itemToAdd, temp));
					//solditems.Add(itemObj);
					//priceTMP[i].text = itemToAdd.Price.ToString();
				}

			}
			isSettingOver = true;
		}
	}

	//void SetBuyCard(WIP.Card _card, GameObject _object)
	//{
	//	if (75 <= CharacterStateStorage.Inst.money)
	//	{
	//		WIP.CardManager.Instance.Deck.Add(_card);
	//		CharacterStateStorage.Inst.money -= 75;
	//		UIManager.Inst.PlayerMoneyUIRefresh();

	//		Destroy(_object);
	//	}
	//}

	//void SetBuyItem(Item_inven item, int i)
	//{
	//	if (item.Price <= CharacterStateStorage.Inst.money)
	//	{
	//		CharacterStateStorage.Inst.money -= item.Price;
	//		UIManager.Inst.PlayerMoneyUIRefresh();
	//		Inventory.inst.AddItem(item.Id);

	//		solditems[i].GetComponent<ItemData>().TooltipDeActive();
	//		Destroy(solditems[i]);
	//		priceTMP[i].gameObject.SetActive(false);

	//		// <<22-11-28 장형용 :: 추가>>
	//		MusicManager.inst.PlayBuyingSound();
	//	}
	//}

	//마나 업그레이드 비용은 10원부터 시작해서 10원씩 증가함 대충 1300골드 정도 들어감 일부러 좀 적은 가격으로 구성함
	public void ManaLevelUp()
	{
		//if (CharacterStateStorage.Inst.money >= manaPrice)
		//{
		//	CharacterStateStorage.Inst.money -= manaPrice;
		//	CharacterStateStorage.Inst.aether++;
		//	UIManager.Inst.PlayerMoneyUIRefresh();
		//	SetManaLevelUp();

		//	// <<22-11-28 장형용 :: 추가>>
		//	MusicManager.inst.PlayBuyingSound();
		//}

		var mana = EntityManager.Instance.StatsContainer.Mana.statValue;

		if (mana == 20)
        {
			return;
        }

		if (EntityManager.Instance.Money >= manaPrice)
        {
			EntityManager.Instance.Money -= manaPrice;

			EntityManager.Instance.StatsContainer.Mana.ChangeStatValue(mana + 1);
		}
	}

	//void SetManaLevelUp()
	//{
	//	//manaPrice = CharacterStateStorage.Inst.aether * 10 + 10;
	//	if (manaPrice >= 170)
	//	{
	//		ManaUpPurchaseButton.onClick.RemoveAllListeners();
	//		ManaPriceTMP.text = "재고 없음!";
	//	}
	//	else
	//	{
	//		ManaPriceTMP.text = "마나 활성\n " + manaPrice + "\n 정수!";
	//	}
	//}

	public void SetUpgradeManaCost()
    {
		if (EntityManager.Instance.StatsContainer == null)
        {
			return;
        }

		var mana = EntityManager.Instance.StatsContainer.Mana.statValue;

		var price = (mana + 1) * 10;
		var messege = string.Empty;

		if (mana == 20)
        {
			messege = "재고 없음!";
        }
        else
        {
			messege = $"마나 활성\n {price} \n 정수!";
		}

		ManaPriceTMP.text = messege;
	}

	#endregion


	//public void ClearShopCard()
	//{
	//	test.Clear();
	//}


}
