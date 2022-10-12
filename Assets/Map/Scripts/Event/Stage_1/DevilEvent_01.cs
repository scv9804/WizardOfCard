using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "EventList/Stage1/Event")]
public class DevilEvent_01 : RoomEventListScript
{
	[SerializeField] Sprite devilStatueSprite;
	[SerializeField] SpriteRenderer changedSpriteRenderer;
	[SerializeField] GameObject changeObject;
	[SerializeField] DialogSystem dialogSystem01;
	public override GameObject Event()
	{
		SpawnDevilstatue();
		StartCoroutine(Diaglog());
		return changeObject;
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
