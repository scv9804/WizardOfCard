using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBase_04 : RoomEventListScript
{
	[Header("대화문")]
	[SerializeField] protected DialogSystem dialogSystem01;

	[Header("확률")]
	[SerializeField, Tooltip("두 개")] float[] percentage;

	public override GameObject Event()
	{
		SpawnStatue();
		StartCoroutine(Diaglog());
		return changeObject;
	}

	IEnumerator Diaglog()
	{
		yield return new WaitForSeconds(0.5f);

		yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

		SetObejects();
		setEventWindow();
		SetButton();
	}

	public void SetButton()
	{
		refuseButton.onClick.RemoveAllListeners();
		acceptButton.onClick.RemoveAllListeners();

		refuseButton.onClick.AddListener(() => eventWindow.SetActive(false));
		acceptButton.onClick.AddListener(() => AddReward());
	}


	public void AddReward()
	{
		// 20230928 장형용 @@ 수정
		//float rand = Utility.Choose(percentage);
		var rand = percentage.Choose();

		Debug.Log(rand);
		//float Random
		//if (rand == 0)
		//{
		//	for (int i = 0; i < ItemDataBase.Inst.database.Count; i++)
		//	{
		//		if(ItemDataBase.Inst.database[i].Id == 19)
		//		Inventory.inst.AddItem(ItemDataBase.Inst.database[i].Id);
		//	}
		//}
		//else
		//{
  //          EntityManager.Inst.playerEntity.Status_Health -= 7;
  //      }

		eventWindow.SetActive(false);
	}
}
