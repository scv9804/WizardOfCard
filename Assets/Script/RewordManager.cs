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
		itemdata = ItemDataBase.inst;
	}

	ItemDataBase itemdata;


	[SerializeField] Image rewordImage;
	[SerializeField] GameObject content;

	[SerializeField] TMP_Text rewordName_TMP;
	[SerializeField] TMP_Text rewordCount_TMP;

	public void GameClear()
	{
		Debug.Log("보상");
		UIManager.Inst.MapClearUI();
		SetReword();
	}


	void SetReword()
	{
		rewordName_TMP.text = itemdata.item_Invens[0].itemExplain;
		rewordCount_TMP.text = "x1";
		rewordImage.sprite = itemdata.item_Invens[0].itemIcon;
	}

	public void AddClearReword() 
	{
		Debug.Log("누름진입");
		Inventory.Inst.AcquireItem(itemdata.item_Invens[0]);
		Debug.Log("탈출");
	}
	
}
