using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEvent_00 : RoomEventListScript
{
	[SerializeField] DialogSystem[] dialogList;


	int fadeTime;
	bool isfade = false;
	bool canClick = true;
	bool active = true;
	bool isStart = false;
	int i = 0;
	[Header("오브젝트")]
	[SerializeField] GameObject cardArea;
	[SerializeField] GameObject costArea;
	[SerializeField] GameObject deckArea;
	[SerializeField] GameObject cemeteryArea;
	[SerializeField] GameObject handrefreshArea;
	[SerializeField] GameObject cemerefreshArea;
	[SerializeField] GameObject turnEndArea;
	[SerializeField] GameObject healthArea;
	[SerializeField] GameObject shieldArea;
	[SerializeField] GameObject turnArea;
	[SerializeField] GameObject QuickSlotArea;
	[SerializeField] GameObject stageArea;
	[SerializeField] GameObject mapArea;
	[SerializeField] GameObject invenArea;
	[SerializeField] GameObject optionArea;


	public override GameObject Event()
	{
		StartCoroutine(TutorialDialog());


		return null;
	}

	 IEnumerator FadeInOut(GameObject obj)
	{
		isStart = true;
		i = 0;
		obj.SetActive(true);
		isfade = false;
		Debug.Log(1);
		while (i < 5)
		{
			i++;
			if (isfade)	yield break;
			obj.SetActive(active);
			active = !active;
			Debug.Log(2);
			yield return new WaitForSeconds(0.6f);
		}
		yield return new WaitForSeconds(0.3f);
		active = true;
		isfade = true;
		
	}


	IEnumerator TutorialDialog()
	{
		yield return new WaitForSeconds(0.5f);

		yield return new WaitUntil(() => dialogList[0].UpdateDialog());

		yield return new WaitUntil(() => Fade(cardArea));
		ResetBool(cardArea);

		yield return new WaitUntil(() => dialogList[1].UpdateDialog());
		yield return new WaitUntil(() => Fade(costArea));
		ResetBool(costArea);


		yield return new WaitUntil(() => dialogList[2].UpdateDialog());
		yield return new WaitUntil(() => Fade(deckArea));
		ResetBool(deckArea);

		yield return new WaitUntil(() => dialogList[3].UpdateDialog());
		yield return new WaitUntil(() => Fade(cemeteryArea));
		ResetBool(cemeteryArea);

		yield return new WaitUntil(() => dialogList[4].UpdateDialog());
		yield return new WaitUntil(() => Fade(handrefreshArea));
		ResetBool(handrefreshArea);

		yield return new WaitUntil(() => dialogList[5].UpdateDialog());
		yield return new WaitUntil(() => Fade(cemerefreshArea));
		ResetBool(cemerefreshArea);

		yield return new WaitUntil(() => dialogList[6].UpdateDialog());
		yield return new WaitUntil(() => Fade(turnEndArea));
		ResetBool(turnEndArea);

		yield return new WaitUntil(() => dialogList[7].UpdateDialog());
		yield return new WaitUntil(() => Fade(healthArea));
		ResetBool(healthArea);

		yield return new WaitUntil(() => dialogList[8].UpdateDialog());
		yield return new WaitUntil(() => Fade(shieldArea));
		ResetBool(shieldArea);

		yield return new WaitUntil(() => dialogList[9].UpdateDialog());
		yield return new WaitUntil(() => Fade(turnArea));
		ResetBool(turnArea);

		yield return new WaitUntil(() => dialogList[10].UpdateDialog());
		yield return new WaitUntil(() => Fade(QuickSlotArea));
		ResetBool(QuickSlotArea);

		yield return new WaitUntil(() => dialogList[10].UpdateDialog());
		yield return new WaitUntil(() => Fade(stageArea));
		ResetBool(stageArea);


		yield return new WaitUntil(() => dialogList[10].UpdateDialog());
		yield return new WaitUntil(() => Fade(mapArea));
		ResetBool(mapArea);


		yield return new WaitUntil(() => dialogList[10].UpdateDialog());
		yield return new WaitUntil(() => Fade(invenArea));
		ResetBool(invenArea);


		yield return new WaitUntil(() => dialogList[10].UpdateDialog());
		yield return new WaitUntil(() => Fade(optionArea));
		ResetBool(optionArea);

		Destroy(this);

		System.GC.Collect();
	}

	void ResetBool(GameObject obj) // 불타입 초기화
	{
		Destroy(obj);
		isStart = false;
		obj = null;
		active = true;
		isfade = false;
	}

	bool Fade(GameObject obj)
	{
		if (!isStart)
		{
			StartCoroutine(FadeInOut(obj));
		}


		if (Input.GetMouseButtonDown(0))
		{
			canClick = !canClick;
			if (canClick)
			{
				Debug.Log("??");
				isfade = true;
			}
		}

		if (isfade)
		{
			return true;
		}
		else
		{
			return false;
		}
		
	}


}
