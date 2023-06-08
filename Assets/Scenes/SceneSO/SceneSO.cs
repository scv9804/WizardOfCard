using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[CreateAssetMenu(menuName = "SceneSO", fileName = "SceneSO")]
public class SceneSO : ScriptableObject
{
	/// <summary>
	/// ����.
	/// �׳� ���̸� �� ���������� ��
	/// ������ ���� ȣ���� ���� �Ʒ��� �ִ°� ������ ȣ���ϸ� �ǰ���?
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
