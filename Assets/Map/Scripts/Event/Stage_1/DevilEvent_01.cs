using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevilEvent_01 : RoomEventListScript
{
	[SerializeField] Sprite devilStatueSprite;
	[SerializeField] SpriteRenderer changedSpriteRenderer;
	[SerializeField] GameObject changeObject;
	[SerializeField] DialogSystem dialogSystem01;
	[SerializeField] Button acceptButton;
	[SerializeField] Button refuseButton;

	[Header("이벤트 설명")]
	[SerializeField,TextArea] string explainEvent;
	[SerializeField,TextArea] string eventReward;
	[SerializeField,TextArea] string eventPay;

	[Header("이벤트 창")]
	[SerializeField] GameObject eventWindow;

	public override GameObject Event()
	{
		SpawnDevilstatue();
		StartCoroutine(Diaglog());
		//acceptButton.onClick.AddListener(Devile_Event);
		return changeObject;
	}

	public override void ExitRoom()
	{
		changeObject.SetActive(false);
	}


	void Devile_Event()
	{		
		EntityManager.Inst.playerEntity.karma += 1;	
		acceptButton.onClick.RemoveListener(Devile_Event);
	}

	IEnumerator Diaglog()
	{
		yield return new WaitForSeconds (0.5f);

		yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

		eventWindow.SetActive(true);	
	}

	public void SpawnDevilstatue()
	{
		changedSpriteRenderer.sprite = devilStatueSprite;
		changeObject.SetActive(true);		
	}
}
