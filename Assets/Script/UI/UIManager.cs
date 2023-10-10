using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

using BETA;
using BETA.Singleton;
using BETA.UI;

using Sirenix.OdinInspector;

using TMPro;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	public static UIManager Inst { get; private set; }

	private void Start()
	{
		SetClose();

		//Reward_UI.gameObject.SetActive(false);

		SetRoomButtons();
	}
	
	[Header("메인 켄버스")]
	public Canvas maincanvas;

	[Header("방 이동 버튼")]
	public Dictionary<string, GameObject> MoveButtons = new Dictionary<string, GameObject>()
	{
		{ "LEFT", null },
		{ "RIGHT", null },
		{ "UP", null },
		{ "DOWN", null }
	};

	[SerializeField, TitleGroup("UI 핸들러")]
	private Dictionary<string, UIHandler> _handlers = new Dictionary<string, UIHandler>();

	[Header("옵션 UI")]
	[SerializeField] GameObject postProcessing;

	[SerializeField, TitleGroup("UI매니저 이벤트")]
	private UIManagerEvent _events;

	// 장형용 :: 20231009 :: 좀 바꿀게요~
	protected override bool Initialize()
    {
		var isEmpty = base.Initialize();

		if (isEmpty)
		{
			Inst = this;

            DontDestroyOnLoad(this);

            SceneManager.sceneLoaded -= OnSceneWasLoaded;
            SceneManager.sceneLoaded += OnSceneWasLoaded;
        }

		return isEmpty;
	}

	// 장형용 :: 20231009 :: 좀 바꿀게요~
	protected override bool Finalize()
	{
		var isEmpty = base.Initialize();

		if (!isEmpty)
        {
            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }

		return isEmpty;
	}

	private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
		SetRoomButtons();
	}

	public void SetRoomButtons()
    {
		//foreach (var conponent in MoveButtons)
		//{
		//	MoveButtons[conponent.Key] = null;
		//}

		var controller = GameObject.Find("Room UI Controller")?.GetComponent<UIController>();

		controller.Require(() =>
		{
			foreach (var conponent in controller.CO)
			{
				MoveButtons[conponent.Key] = conponent.Value;
			}

            RefreshMoveButtons();
        });
	}

	public void RefreshMoveButtons()
    {
		foreach (var button in MoveButtons)
		{
			if (button.Value == null)
            {
				return;
            }

			var handler = button.Value.GetComponent<UIHandler>();

			handler.Refresh();
		}
	}

	public void DisableMoveButtons()
	{
		foreach (var button in MoveButtons)
		{
			//button.Value.Require(() =>
			//{
			//	button.Value.SetActive(false);
			//});

			if (button.Value == null)
            {
				return;
            }

			button.Value.SetActive(false);
		}
	}

	public void CardArrange()
    {
		_events.OnCardArrange.Launch();
    }

	public void MapClearUI()
	{
		//gameClearBack_UI.SetActive(true);
	}

	public void MinimapActive()
	{
		//minimapUI.SetActive(true);
		//minimapCancleArea.SetActive(true);
	}


	public void PlayerMoneyUIRefresh()
	{
		//money_TMP.text = CharacterStateStorage.Inst.money.ToString("D3");
	}

	#region minimaps

	public void LeftRoomMove()
	{
		//LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		//levelGeneration.existRoomCheck();
		//level.MoveRoom(0);

		LevelGeneration.Instance.existRoomCheck();
		LevelGeneration.Instance.MoveRoom(0);
	}
	public void RightRoomMove()
	{
		//LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		//level.MoveRoom(1);

		LevelGeneration.Instance.existRoomCheck();
		LevelGeneration.Instance.MoveRoom(1);
	}

	public void UpRoomMove()
	{
		//LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		//level.MoveRoom(2);

		LevelGeneration.Instance.existRoomCheck();
		LevelGeneration.Instance.MoveRoom(2);
	}

	public void DownRoomMove()
	{
		//LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		//level.MoveRoom(3);

		LevelGeneration.Instance.existRoomCheck();
		LevelGeneration.Instance.MoveRoom(3);
	}

	#endregion



	#region ToggleButtons

	void SetStateUI()
	{
		//minimapUI.SetActive(isMinimapUse);
		//optionUI.SetActive(isOptionUse);
		//inventoryUI.SetActive(isInventoryUse);
		//deckUI.SetActive(isDeckUse);
		//CemeteryUI.SetActive(isCemeteryUse);
		//Reward_UI.SetActive(isRewardUse);

		// << 23-06-10 장형용 :: 제거 >>
		//optionCancleArea.gameObject.SetActive(isOptionUse);
		//inventoryCancleArea.gameObject.SetActive(isInventoryUse);
		//minimapCancleArea.gameObject.SetActive(isMinimapUse);
		//CardCancleArea.gameObject.SetActive(isCardUse);
	}

	public void SetClose()
	{
		//isCardUse = true;
		//isMinimapUse = false;
		//isOptionUse = false;
		//isInventoryUse = false;
		//isCemeteryUse = false;
		//isDeckUse = false;
		//isRewardUse = false;

		SetStateUI();
		//CardManager.Inst.SetCardStateBack();
	}

	//public void ButtonDeActivate()
	//{
	//	roomMoveButton_L.gameObject.SetActive(false);
	//	roomMoveButton_R.gameObject.SetActive(false);
	//	roomMoveButton_U.gameObject.SetActive(false);
	//	roomMoveButton_D.gameObject.SetActive(false);
	//}

	#endregion


	public void GameOverButton()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
	}
}