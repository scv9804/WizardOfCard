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
	[SerializeField] GameObject shopAllObject;
	[SerializeField] GameObject shopPanelObject;
	[SerializeField] GameObject playerPanelObject;
	[SerializeField] GameObject shopOwner;
	[SerializeField] GameObject speechBubble;

	[SerializeField] Button shopOwnerButton;
	[SerializeField] Button shopExitButton;

	[SerializeField] TMP_Text shopOwnerTMP;
	[SerializeField] string[] ShopOwnerspeechArray;

	[Header("아이템 프리팹")]
	[SerializeField] GameObject itemPrefab;

	List<Item_inven> shopItemList = new List<Item_inven>();
	List<Item_inven> playerItemList = new List<Item_inven>();

	Vector3 OriginSize;

	bool isShopActive = false;

	WaitForSeconds speechBubbleDelay = new WaitForSeconds(1.5f);

	#region 기본 ON OFF 설정
	private void Start()
	{
		OriginSize = speechBubble.transform.localScale;
		shopOwnerButton.onClick.AddListener(()=>SetActiveShopPanel());
		shopExitButton.onClick.AddListener(()=>SetActiveShopPanel());
	}

	public void EnterShop()
	{
		shopAllObject.SetActive(true);
		shopOwner.SetActive(true);
		StartCoroutine(Repeat());
	}

	public void ExitShop()
	{
		shopAllObject.SetActive(false);
	}

	public void SetActiveShopPanel()
	{
		isShopActive = !isShopActive;
		shopPanelObject.SetActive(isShopActive);
		playerPanelObject.SetActive(isShopActive);
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

	void SetSellButton()
	{

	}

	void SetBuyButton()
	{
		
	}

	void SetShop()
	{
		
	}

	void SellItem()
	{

	}

	#endregion

}
