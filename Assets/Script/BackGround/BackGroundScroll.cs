using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
	[SerializeField] GameObject[] Objcet_Trees;
	[SerializeField] GameObject Object_BackTree_1;
	[SerializeField] GameObject Object_BackTree_2;
	[SerializeField] GameObject Object_BackTree_3;

	[SerializeField] GameObject Object_Ground_1;
	[SerializeField] GameObject Object_Ground_2;

	[SerializeField] GameObject EndPos;

	bool is_moving;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			is_moving = true;
		}
		if (is_moving)
			Invoke("BackGround_StageMove", 0.01f);
	}


	public void BackGround_StageMove() 
	{
		/*	foreach (var Check in Objcet_Trees)
			{
				Check.gameObject.transform.position += Vector3.left;
			}*/


		Object_Ground_1.transform.position += Vector3.left * Time.deltaTime * 10f;
		Object_Ground_2.transform.position += Vector3.left * Time.deltaTime * 10f;


		if (Object_Ground_1.gameObject.transform.position.x <= -26.0f)
		{
			Object_Ground_1.gameObject.transform.position = new Vector3(26.0f, 0f ,1f);
		}
		if (Object_Ground_2.gameObject.transform.position.x < -26.0f)
		{
			Object_Ground_2.gameObject.transform.position = new Vector3(26.0f, 0f, 1f);
			is_moving = false;
		}
	}




}
