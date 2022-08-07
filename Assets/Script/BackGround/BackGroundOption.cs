using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundOption : MonoBehaviour
{
	private void OnMouseUp()
	{
		UIManager.Inst.optionUI.SetActive(false);
	}
}
