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
	[SerializeField] string explainEvent;
	[SerializeField] Button acceptButton;
	[SerializeField] Button refuseButton;

	public override GameObject Event()
	{
		SpawnDevilstatue();
		StartCoroutine(Diaglog());
		acceptButton.onClick.AddListener(Devile_Event);
		return changeObject;
	}

	void SetExplain()
	{

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

		
	}

	public void SpawnDevilstatue()
	{
		changedSpriteRenderer.sprite = devilStatueSprite;
		changeObject.SetActive(true);		
	}
}
