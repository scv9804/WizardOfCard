using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundInven : MonoBehaviour
{
	private void OnMouseUp()
	{
		UIManager.Inst.SetClose();
	}
}
