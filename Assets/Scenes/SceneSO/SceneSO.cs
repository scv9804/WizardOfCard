using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(menuName = "SceneSO", fileName = "SceneSO")]
public class SceneSO : ScriptableObject
{
	/// <summary>
	/// 사용법.
	/// 그냥 씬이름 다 떄려넣으면 됨
	/// 전투방 랜덤 호출은 저거 아래에 있는거 딱봐도 호출하면 되겠제?
	/// </summary>

	[Serializable][SerializeField]
	private class SceneNameList
	{
		public List<string> sceneName;
	}
	[SerializeField] private List<SceneNameList> SceneList;


	public void CallBattleScene(int _stage)
	{
		int Random = UnityEngine.Random.Range(0, SceneList[_stage-1].sceneName.Count);

		LoadSceneManager.LoadScene(SceneList[_stage - 1].sceneName[Random]);
	}
}
