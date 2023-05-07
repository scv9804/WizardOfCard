using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;


public abstract class EnemyAttackTest : ScriptableObject
{
	public virtual IEnumerator attackNewOne(XSBattleMgr mgr) 
	{
		yield return null;
	}	

}
