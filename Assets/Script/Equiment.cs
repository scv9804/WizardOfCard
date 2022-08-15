using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equiment : MonoBehaviour
{
	public static Equiment Inst{ get; set; }

	private void Awake()
	{
		Inst = this;
	}

	


}
