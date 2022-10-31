using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEvent_00 : RoomEventListScript
{
	[SerializeField] DialogSystem dialogSystem01;
	[SerializeField] DialogSystem dialogSystem02;
	[SerializeField] DialogSystem dialogSystem03;

	int fadeTime;
	[Header("오브젝트")]
	[SerializeField] GameObject cardArea;



	public override GameObject Event()
	{
		StartCoroutine(TutorialDialog());


		return null;
	}

	 IEnumerator FadeInOut(GameObject obj)
	{
		int i = 0;
		obj.SetActive(true);
		while (i < 2)
		{
				i++;
				yield return new WaitForSeconds(0.3f);
				obj.SetActive(false);

				yield return new WaitForSeconds(0.3f);
				obj.SetActive(true);
		}
	}


	IEnumerator TutorialDialog()
	{
		yield return new WaitForSeconds(0.5f);

		yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

		yield return StartCoroutine(FadeInOut(cardArea));

		yield return new WaitForSeconds(0.5f);

		cardArea.SetActive(false);

		yield return new WaitUntil(() => dialogSystem02.UpdateDialog());

		
	}

	public void tutorial()
	{

	}


}
