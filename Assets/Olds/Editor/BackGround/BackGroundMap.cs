using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMap : MonoBehaviour
{
	private void OnMouseUp()
	{
		UIManager.Inst.SetClose();
	}

}
