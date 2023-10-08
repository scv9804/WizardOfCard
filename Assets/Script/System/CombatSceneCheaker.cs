using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneCheaker : MonoBehaviour
{
	private void OnEnable()
	{
		//TurnManager.Inst.isCombatScene = true;
		UIManager.Inst.maincanvas.enabled = true;
		StartCoroutine(LevelGeneration.Inst.Co_StartGame());
		//EntityManager.Inst.SetEnemyObjectArray();
	//	GameObject.Find("main").GetComponent<XSSLG.XSBattleMgr>().SetEntityDic();
		//CardManager.Inst.SetCardSpawnPos();
	}

}
