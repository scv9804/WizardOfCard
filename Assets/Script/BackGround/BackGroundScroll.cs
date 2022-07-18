using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
	[SerializeField] GameObject[] Objcet_Trees;
	[SerializeField] GameObject Object_BackTree_1;
	[SerializeField] GameObject Object_BackTree_2;
	[SerializeField] GameObject Object_BackTree_3;

	[SerializeField] GameObject EndPos;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			StartCoroutine(BackGround_StageMove());
		}
	}


	public IEnumerator BackGround_StageMove() 
	{
		foreach (var Check in Objcet_Trees)
		{
			Check.gameObject.transform.position += Vector3.left;
		}

	/*	if ()
		{

		}*/

		yield return true;	
	}




}
