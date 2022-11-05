using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptalbe Object/EnemySO")]
	public class EnemySO : ScriptableObject
	{
		public Enemy[] enemy;
	}

