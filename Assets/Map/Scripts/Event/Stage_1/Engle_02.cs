using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engle_02 : RoomEventListScript
{
	[Header("´ëÈ­¹®")]
	[SerializeField] protected DialogSystem dialogSystem01;

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
		refuseButton.onClick.AddListener(() => eventWindow.SetActive(false));
		acceptButton.onClick.AddListener(() => AddReward());
	}

	public void AddReward()
	{
		EntityManager.Inst.playerEntity.Status_MaxHealth += 3;
		EntityManager.Inst.playerEntity.Status_Health += 3;
		EntityManager.Inst.playerEntity.karma += 1;
		eventWindow.SetActive(false);
	}

}
