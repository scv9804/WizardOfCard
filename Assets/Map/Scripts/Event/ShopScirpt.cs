using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopScirpt : MonoBehaviour
{
	[Header("���� don't destroy")]
	[SerializeField] GameObject shop;

	[Header("�ʼ����� �����ֱ�")]
	[SerializeField] ItemDataBase database;

	[Header("������Ʈ�� ����")]
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

	[SerializeField] ItemDataBase allItemData;

	[Header("������ ����")]
	[SerializeField] GameObject cardSpawnParent;
	[SerializeField] GameObject[] itemSapwnParents;
	[SerializeField] TMP_Text[] priceTMP;


	[Header("������ ������")]
	[SerializeField] GameObject itemPrefab;
	[SerializeField] GameObject cardPrefab;

	List<Item_inven> shopItemList = new List<Item_inven>();
	List<GameObject> solditems = new List<GameObject>();

	Vector3 OriginSize;

	bool isSettingOver = false;
	bool isShopActive = false;
	int manaPrice = 10;

	StringBuilder sb = new StringBuilder();

	WaitForSeconds speechBubbleDelay = new WaitForSeconds(1.5f);

	#region �⺻ ON OFF ����
	private void Start()
	{
		shopOwnerButton.onClick.AddListener(OpenShop);
		shopExitButton.onClick.AddListener(CloseShop);
		ManaUpPurchaseButton.onClick.AddListener(ManaLevelUp);
		OriginSize = speechBubble.transform.localScale;
		SetManaLevelUp();
		DontDestroyOnLoad(shop);
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
		CardManager.Inst.SetCardStateCannot();
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
		CardManager.Inst.SetCardStateBack();
	}

	public void OpenShop()
	{
		shopPanelObject.SetActive(true);

		// <<22-11-28 ������ :: �߰�>>
		MusicManager.inst.PlayShopSound();
	}
	public void CloseShop()
	{
		shopPanelObject.SetActive(false);

		// <<22-11-28 ������ :: �߰�>>
		MusicManager.inst.PlayShopSound();
	}

	#endregion


	#region ���� ��� ������
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

	void SetShop()
	{
		if (!isSettingOver)
		{
			//ī�� ����
			for (int i = 0; i < 5; i++)
			{
/*				int randCard = UnityEngine.Random.Range(0, CardManager.Inst.itemSO.items.Length - 1);
				var temt = Instantiate(cardPrefab);
				temt.transform.GetChild(0).GetComponent<TMP_Text>().text = CardManager.Inst.itemSO.items[randCard].card.i_manaCost.ToString();
				temt.transform.GetChild(1).GetComponent<TMP_Text>().text = CardManager.Inst.itemSO.items[randCard].card.st_cardName;
				temt.transform.GetChild(2).GetComponent<TMP_Text>().text = CardManager.Inst.itemSO.items[randCard].card.GetCardExplain();
				temt.transform.GetChild(3).GetComponent<TMP_Text>().text = 75.ToString();
				temt.transform.GetChild(4).GetComponent<Image>().sprite = CardManager.Inst.itemSO.items[randCard].card.CardIconImage;
				temt.transform.SetParent(cardSpawnParent.transform);
				temt.AddComponent<Button>();
				temt.GetComponent<Button>().onClick.AddListener(() => SetBuyCard(CardManager.Inst.itemSO.items[randCard].card, temt));*/

				
			}

			
			//���� ������ ����
			for (int i = 0; i < 3; i++)
			{
				int temp = i;
				if (i < 2)
				{
					int rand = UnityEngine.Random.Range(0, database.notEquiDataBase.Count);
					Item_inven itemToAdd = database.FetchItemById(database.notEquiDataBase[rand].Id);

					GameObject itemObj = Instantiate(itemPrefab);
					itemObj.GetComponent<ItemData>().item = itemToAdd;
					itemObj.GetComponent<ItemData>().item.OwnPlayer = false;
					itemObj.transform.SetParent(itemSapwnParents[i].transform);
					itemObj.transform.localPosition = Vector2.zero;
					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					itemObj.name = "Item: " + itemToAdd.Title;
					itemObj.transform.localScale *= 3;
					itemObj.AddComponent<Button>();
					itemObj.GetComponent<Button>().onClick.AddListener(() => SetBuyItem(itemToAdd, temp));
					solditems.Add(itemObj);
					priceTMP[i].text = itemToAdd.Price.ToString();
				}
				else
				{
					int rand = UnityEngine.Random.Range(0, database.equiDataBase.Count);
					Item_inven itemToAdd = database.FetchItemById(database.equiDataBase[rand].Id);

					GameObject itemObj = Instantiate(itemPrefab);
					itemObj.GetComponent<ItemData>().item = itemToAdd;
					itemObj.GetComponent<ItemData>().item.OwnPlayer = false;
					itemObj.transform.SetParent(itemSapwnParents[i].transform);
					itemObj.transform.localPosition = Vector2.zero;
					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					itemObj.name = "Item: " + itemToAdd.Title;
					itemObj.transform.localScale *= 3;
					itemObj.AddComponent<Button>();
					itemObj.GetComponent<Button>().onClick.AddListener(() => SetBuyItem(itemToAdd, temp));
					solditems.Add(itemObj);
					priceTMP[i].text = itemToAdd.Price.ToString();
				}

			}
			isSettingOver = true;
		}
	}

	void SetBuyCard(Card _card, GameObject _object)
	{
		if (75 <= CharacterStateStorage.Inst.money)
		{
			CardManager.Inst.AddSelectCard_Deck(_card);
			CharacterStateStorage.Inst.money -= 75;
			UIManager.Inst.PlayerMoneyUIRefresh();
			
			Destroy(_object);
		}
	}

	void SetBuyItem(Item_inven item, int i)
	{
		if (item.Price <=  CharacterStateStorage.Inst.money)
		{
			CharacterStateStorage.Inst.money -= item.Price;
			UIManager.Inst.PlayerMoneyUIRefresh();
			Inventory.inst.AddItem(item.Id);

			solditems[i].GetComponent<ItemData>().TooltipDeActive();
			Destroy(solditems[i]);
			priceTMP[i].gameObject.SetActive(false);

			// <<22-11-28 ������ :: �߰�>>
			MusicManager.inst.PlayBuyingSound();
		}
	}

	//���� ���׷��̵� ����� 10������ �����ؼ� 10���� ������ ���� 1300��� ���� �� �Ϻη� �� ���� �������� ������
	void ManaLevelUp()
	{
		if (CharacterStateStorage.Inst.money >= manaPrice)
		{
			CharacterStateStorage.Inst.money -= manaPrice;
			CharacterStateStorage.Inst.aether++;
			UIManager.Inst.PlayerMoneyUIRefresh();
			SetManaLevelUp();

			// <<22-11-28 ������ :: �߰�>>
			MusicManager.inst.PlayBuyingSound();
		}
	}

	void SetManaLevelUp()
	{
		manaPrice = CharacterStateStorage.Inst.aether * 10 + 10;
		if (manaPrice >= 170)
		{
			ManaUpPurchaseButton.onClick.RemoveAllListeners();
			ManaPriceTMP.text = "��� ����!";
		}
		else
		{
			ManaPriceTMP.text = "���� Ȱ��\n " + manaPrice + " ����!";
		}
	}

	#endregion


}
