using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
	[SerializeField] GameObject[] Objcet_Trees;
	[SerializeField] GameObject[] Object_BackTree_1;
	[SerializeField] GameObject[] Object_BackTree_2;
	[SerializeField] GameObject[] Object_BackTree_3;

	[SerializeField] GameObject Object_Ground_1;
	[SerializeField] GameObject Object_Ground_2;

	[SerializeField] GameObject EndPos;

	Dictionary<GameObject, Transform> dic_originTransForm;

	bool is_moving;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			is_moving = true;
		}
		if (is_moving)
			BackGround_StageMove();
	}


	public void BackGround_StageMove() 
	{
		foreach (var Check in Objcet_Trees)
		{
			Check.gameObject.transform.position += Vector3.left * Time.deltaTime * 10f;
			if (Check.gameObject.transform.position.x <= -26.0f)
				Check.transform.position = new Vector3(26.0f, 0f, RandomTreePos_Z());
		}

		foreach (var Check in Object_BackTree_1)
		{
			Check.gameObject.transform.position += Vector3.left * Time.deltaTime * 10f;
			if (Check.gameObject.transform.position.x <= -26.0f)
				Check.transform.position = new Vector3(26.0f, 0f, 2.0f);
		}

		foreach (var Check in Object_BackTree_2)
		{
			Check.gameObject.transform.position += Vector3.left * Time.deltaTime * 5f;
			if (Check.gameObject.transform.position.x <= -26.0f)
				Check.transform.position = new Vector3(26.0f, 0f, 3.0f);
		}

		foreach (var Check in Object_BackTree_3)
		{
			Check.gameObject.transform.position += Vector3.left * Time.deltaTime * 1f;
			if (Check.gameObject.transform.position.x <= -26.0f)
				Check.transform.position = new Vector3(26.0f, 0f, 4.0f);
		}

		Object_Ground_1.transform.position += Vector3.left * Time.deltaTime * 10f;
		Object_Ground_2.transform.position += Vector3.left * Time.deltaTime * 10f;


		if (Object_Ground_1.gameObject.transform.position.x <= -26.0f)
		{
			Object_Ground_1.gameObject.transform.position = new Vector3(26.0f, 0f ,1f);
			is_moving = false;
		}
		if (Object_Ground_2.gameObject.transform.position.x < -26.0f)
		{
			Object_Ground_2.gameObject.transform.position = new Vector3(26.0f, 0f, 1f);
			is_moving = false;
		}
	}

	float RandomTreePos_Z()
	{
		float pos_z;
		pos_z = Random.Range(1.2f, 1.9f);

		return pos_z;
	}


}
