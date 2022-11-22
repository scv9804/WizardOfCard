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
	[Header("필수설정 상점넣기")]
	[SerializeField] ItemDataBase database;

	[Header("오브젝트들 설정")]
	[SerializeField] GameObject []shopAllObject;
	[SerializeField] GameObject shopPanelObject;
	[SerializeField] GameObject shopOwner;
	[SerializeField] GameObject speechBubble;

	[SerializeField] Button shopOwnerButton;
	[SerializeField] Button shopExitButton;

	[SerializeField] TMP_Text shopOwnerTMP;
	[SerializeField] string[] ShopOwnerspeechArray;

	[Header("아이템 관련")]
	[SerializeField] GameObject cardSpawnParent;
	[SerializeField] GameObject [] itemSapwnParents;
	[SerializeField] TMP_Text[] priceTMP; 


	[Header("아이템 프리팹")]
	[SerializeField] GameObject itemPrefab;
	[SerializeField] GameObject cardPrefab;

	List<Item_inven> shopItemList = new List<Item_inven>();
	List<GameObject> solditems = new List<GameObject>();

	Vector3 OriginSize;

	bool isShopActive = false;
	int manaPirce = 10;

	StringBuilder sb = new StringBuilder();

	WaitForSeconds speechBubbleDelay = new WaitForSeconds(1.5f);

	#region 기본 ON OFF 설정
	private void Start()
	{
		shopOwnerButton.onClick.AddListener(OpenShop);
		shopExitButton.onClick.AddListener(CloseShop);
		OriginSize = speechBubble.transform.localScale;
		SetShop(2);
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
		SetShop(1);
	}

	public void ExitShop()
	{
		foreach (var item in shopAllObject)
		{
			item.SetActive(true);
		}
		CardManager.Inst.SetCardStateBack();
	}

	public void OpenShop()
	{
		shopPanelObject.SetActive(true);
	}
	public void CloseShop()
	{
		shopPanelObject.SetActive(false);
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
		speechBubble.transform.DOScale(OriginSize ,0.5f).SetEase(Ease.OutBack);
		int rand = UnityEngine.Random.Range(0,ShopOwnerspeechArray.Length-1);
		shopOwnerTMP.text = ShopOwnerspeechArray[rand];
		yield return speechBubbleDelay;
		speechBubble.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(0.5f);
		speechBubble.SetActive(false);
		yield return new WaitForSeconds(5f);
	}

	void SetShop(int id)
	{
		//카드 세팅
		for (int i = 0; i < 5; i++)
		{
			int randCard = UnityEngine.Random.Range(0, CardManager.Inst.itemSO.items.Length-1);
			var temt = Instantiate(cardPrefab);
			temt.transform.GetChild(0).GetComponent<TMP_Text>().text = CardManager.Inst.itemSO.items[randCard].card.i_manaCost.ToString();
			temt.transform.GetChild(1).GetComponent<TMP_Text>().text = CardManager.Inst.itemSO.items[randCard].card.st_cardName;
			//temt.transform.GetChild(2).GetComponent<TMP_Text>().text = ;   //일단 막힘 이거 근본 수정해야하는데 좀더 상의 해서 바꿀필요 있음.
			temt.transform.GetChild(3).GetComponent<TMP_Text>().text = 75.ToString();
			temt.transform.SetParent(cardSpawnParent.transform);
			//temt.GetComponent<Material>();
			//temt.GetComponentInChildren<TMP_Text>().text = ;
		}


		//아이템 세팅
		Item_inven itemToAdd = database.FetchItemById(id);
		for (int i = 0; i < 3; i++)
		{
			int temp = i;
			GameObject itemObj = Instantiate(itemPrefab);
			itemObj.GetComponent<ItemData>().item = itemToAdd;
			itemObj.GetComponent<ItemData>().item.OwnPlayer = false;
			itemObj.transform.SetParent(itemSapwnParents[i].transform);
			itemObj.transform.localPosition = Vector2.zero;
			itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
			itemObj.name = "Item: " + itemToAdd.Title;
			itemObj.transform.localScale *= 3;
			itemObj.AddComponent<Button>();
			itemObj.GetComponent<Button>().onClick.AddListener(()=> SetBuyItem(temp));
			solditems.Add(itemObj);
			priceTMP[i].text = itemToAdd.Price.ToString();
		}
	}

	void SetBuyItem(int i)
	{
		Debug.Log(i);
		if (int.Parse(priceTMP[i].text) <= EntityManager.Inst.playerEntity.money)
		{
			EntityManager.Inst.playerEntity.money -= int.Parse(priceTMP[i].text);
			UIManager.Inst.PlayerMoneyUIRefresh();
			Inventory.inst.AddItem(solditems[i].GetComponent<ItemData>().id);

			solditems[i].GetComponent<ItemData>().TooltipDeActive();
			Destroy(solditems[i]);
			priceTMP[i].gameObject.SetActive(false);
		}
	}

	//마나 업그레이드 비용은 10원부터 시작해서 10원씩 증가함 대충 1300골드 정도 들어감 일부러 좀 적은 가격으로 구성함
	void ManaLevelUp()
	{
		if (EntityManager.Inst.playerEntity.money <= manaPirce)
		{
			EntityManager.Inst.playerEntity.money -= manaPirce;
			EntityManager.Inst.playerEntity.Status_MaxAether = EntityManager.Inst.playerEntity.Status_MaxAether + 1;
		}
	}

	#endregion


}
