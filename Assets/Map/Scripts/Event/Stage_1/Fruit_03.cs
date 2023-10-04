using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_03 : RoomEventListScript
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
		//	EntityManager.Inst.playerEntity.Status_MaxHealth += 2;
		//	if (EntityManager.Inst.playerEntity.Status_Health + 5 > EntityManager.Inst.playerEntity.Status_MaxHealth)
		//	{
		//		EntityManager.Inst.playerEntity.Status_Health = EntityManager.Inst.playerEntity.Status_MaxHealth;
		//	}
		//	else
		//	{
		//		EntityManager.Inst.playerEntity.Status_Health += 5;
		//	}
		//}
		//else
		//{
		//	EntityManager.Inst.playerEntity.Status_Health -= 3;
		//}

		eventWindow.SetActive(false);
	}
}
