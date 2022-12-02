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
		float rand = Utility.Choose(percentage);
		Debug.Log(rand);
		//float Random
		if (rand == 0)
		{
			int randitem = UnityEngine.Random.Range(0,ItemDataBase.Inst.equiDataBase.Count);
			Inventory.inst.AddItem(randitem);
		}
		else
		{
			EntityManager.Inst.playerEntity.Status_Health -= 7;
		}

		eventWindow.SetActive(false);
	}
}
