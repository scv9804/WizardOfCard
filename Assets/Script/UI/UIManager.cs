using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}
	private void Start()
	{
		//mainCamera = mainCam.GetComponent<CameraData>();
		//mapCamera = mapCam.GetComponent<CameraData>();
		SetClose();
		Reward_UI.gameObject.SetActive(false);
	}

	[Header("방이동 버튼")]
	public Button roomMoveButton_L;
	public Button roomMoveButton_R;
	public Button roomMoveButton_U;
	public Button roomMoveButton_D;

	[Header("메인 UI")]
	public TMP_Text DeckCountTMP_UI;
	public TMP_Text CemeteryCountTMP_UI;
	public TMP_Text HealthTMP_UI;
	public TMP_Text myturn_UI_TMP;

	public TMP_Text money_TMP;
	public TMP_Text ManaTMP_UI;
	public GameObject optionUI;
	public GameObject minimapUI;
	public GameObject inventoryUI;
	public GameObject deckUI;
	public GameObject CemeteryUI;
	public Image turnEndButtonSpriteImage;

	[Header("온오프 UI")]
	public GameObject CardCancleArea;
	public GameObject optionCancleArea;
	public GameObject minimapCancleArea;
	public GameObject inventoryCancleArea;
	public GameObject gameClearBack_UI;
	public GameObject Reward_UI;


	[Header("옵션 UI")]
	[SerializeField] GameObject postProcessing;

	[SerializeField] Camera mainCam;
	[SerializeField] Camera mapCam;

	[SerializeField] CameraData mainCamera;
	[SerializeField] CameraData mapCamera;

	bool isDeckUse;
	bool isCardUse;
	bool isOptionUse;
	bool isMinimapUse;
	bool isCemeteryUse;
	bool isInventoryUse;
	bool ispostProcessing = true;
	bool isUIUse;

	//<<22-10-26 장형용 :: 리롤 버튼 사용 중 추가 사용 못 하게 추가>>
	public bool canHandRefresh;

	[SerializeField] LevelGeneration levelGeneration;

	Coroutine tryEndTurnCoroutine;

	public void AntiAliasing_FXAA()
	{
		mainCamera.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
		mapCamera.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
	}
	public void AntiAliasing_SMAA()
	{
		mainCamera.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
		mapCamera.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
	}

	public void AntiAliasing_None()
	{
		mapCamera.antialiasing = AntialiasingMode.None;
		mainCamera.antialiasing = AntialiasingMode.None;
	}

	public void PostProcessing()
	{
		ispostProcessing = !ispostProcessing;
		postProcessing.SetActive(ispostProcessing);
	}




	private void FixedUpdate()
	{
		if (TurnManager.Inst.myTurn)
		{
			myturn_UI_TMP.text = "내 턴";
		}
		else
		{
			myturn_UI_TMP.text = "상대 턴";
		}

	}


	public void TurnEndButton()
	{
		if (TurnManager.Inst.myTurn && CardManager.Inst.e_CardStats == E_CardStats.CanAll)
		{
			//LevelGeneration.Inst.EndTurn();

			// <<22-10-27 장형용 :: 추가>>
			tryEndTurnCoroutine = StartCoroutine(TryEndTurn());
		}
	}

	// <<22-10-27 장형용 :: 추가>>
	public IEnumerator TryEndTurn()
	{
		CardManager.Inst.e_CardStats = E_CardStats.Cannot;

		yield return new WaitAllCardUsingDone();

		if(!EntityManager.Inst.IsAlreadyAllDead())
        {
			LevelGeneration.Inst.EndTurn();
		}

		yield return null;
	}

	public void TurnEndButtonActivae()
	{

		if (TurnManager.Inst.myTurn && CardManager.Inst.myDeck.Count != 0)
		{
			Color color = Color.white;
			turnEndButtonSpriteImage.color = color;
		}
		else
		{
			Color color = Color.gray;
			turnEndButtonSpriteImage.color = color;
		}
	}

	public bool IsUIUse 
	{ 
		get { return isUIUse; }
		set { isUIUse = value; }
	}

	public bool CanUseUI()
	{
		if (isUIUse)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void CemeteryRefresh()
	{
		if (TurnManager.Inst.myTurn && CardManager.Inst.myCemetery.Count != 0 )
			CardManager.Inst.CemeteryRefesh();		
	}

	public void HandRefresh()
	{
		// <<22-10-26 장형용 :: canHandRefresh 조건 추가>>
		if (TurnManager.Inst.myTurn 
			&& CardManager.Inst.myDeck.Count != 0
			&& canHandRefresh)
		{
			//CardManager.Inst.HandRefresh();

			StartCoroutine(CardManager.Inst.HandRefresh());
		}
	}

	
	
	public void SetDeckCountUI(int _deckCount)
	{
		DeckCountTMP_UI.text = _deckCount.ToString();
	}

	public void SetCemeteryCountUI(int _cemeteryCount)
	{
		CemeteryCountTMP_UI.text = _cemeteryCount.ToString();
	}

	public void SetManaUI()
	{
		ManaTMP_UI.text = EntityManager.Inst.playerEntity.Status_Aether.ToString() + "/" + EntityManager.Inst.playerEntity.Status_MaxAether.ToString();
	}


	public void MapClearUI()
	{
		gameClearBack_UI.SetActive(true);
	}

	public void MapClearUIAccpetButton()
	{
	//	RewordManager.Inst.AddClearReword();
		gameClearBack_UI.SetActive(false);
	}


	#region minimaps

	public void LeftRoomMove()
	{
		LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		levelGeneration.existRoomCheck();
		level.MoveRoom(0);		
	}
	public void RightRoomMove()
	{
		LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		level.MoveRoom(1);		
	}

	public void UpRoomMove()
	{
		LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		level.MoveRoom(2);
	}

	public void DownRoomMove()
	{
		LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
		level.MoveRoom(3);
	}

	public void ButtonActivate()
	{
		levelGeneration.existRoomCheck();
		if (levelGeneration.i_Room_L)
		{
			roomMoveButton_L.gameObject.SetActive(true);
			Debug.Log("방선택");
		}
		if (levelGeneration.i_Room_R)
		{
			roomMoveButton_R.gameObject.SetActive(true);
			Debug.Log("방선택");
		}
		if (levelGeneration.i_Room_U)
		{
			roomMoveButton_U.gameObject.SetActive(true);
			Debug.Log("방선택");
		}
		if (levelGeneration.i_Room_D)
		{
			roomMoveButton_D.gameObject.SetActive(true);
			Debug.Log("방선택");
		}
	}

	#endregion



	#region ToggleButtons

	void SetStateUI()
	{
		minimapUI.SetActive(isMinimapUse);
		optionUI.SetActive(isOptionUse);
		inventoryUI.SetActive(isInventoryUse);
		deckUI.SetActive(isDeckUse);
		CemeteryUI.SetActive(isCemeteryUse);

		optionCancleArea.gameObject.SetActive(isOptionUse);
		inventoryCancleArea.gameObject.SetActive(isInventoryUse);
		minimapCancleArea.gameObject.SetActive(isMinimapUse);
		CardCancleArea.gameObject.SetActive(isCardUse);
	}

	public void SetClose()
	{
		isCardUse = true;
		isMinimapUse = false;
		isOptionUse = false;
		isInventoryUse = false;
		isCemeteryUse = false;
		isDeckUse = false;

		SetStateUI();
		CardManager.Inst.SetCardStateBack();
	}

	public void ButtonDeActivate()
	{
		roomMoveButton_L.gameObject.SetActive(false);
		roomMoveButton_R.gameObject.SetActive(false);
		roomMoveButton_U.gameObject.SetActive(false);
		roomMoveButton_D.gameObject.SetActive(false);
	}

	public void MinimapToggel()
	{
		if (isMinimapUse)
		{
			SetClose();
		}
		else if (isUIUse)
		{
			isMinimapUse = !isMinimapUse;

			if (optionUI.activeSelf == true)
			{
				isOptionUse = false;
			}
			if (inventoryUI.activeSelf == true)
			{
				isInventoryUse = false;
			}
			if (deckUI.activeSelf == true)
			{
				isDeckUse = false;
			}
			if (CemeteryUI.activeSelf == true)
			{
				isCemeteryUse = false;
			}

			isCardUse = false;
			SetStateUI();
			CardManager.Inst.SetCardStateCannot();
		}
	}

	public void OptionToggle()
	{
		if (isOptionUse)
		{
			SetClose();
		}
		else if (isUIUse)
		{
			isOptionUse = !isOptionUse;


			if (minimapUI.activeSelf == true)
			{
				isMinimapUse = false;
			}
			if (inventoryUI.activeSelf == true)
			{
				isInventoryUse = false;
			}
			if (deckUI.activeSelf == true)
			{
				isDeckUse = false;
			}
			if (CemeteryUI.activeSelf == true)
			{
				isCemeteryUse = false;
			}

			isCardUse = false;
			SetStateUI();
			CardManager.Inst.SetCardStateCannot();
		}	
	}

	public void InventoryToggle()
	{
		if (isInventoryUse)
		{
			SetClose();
		}
		else if (isUIUse)
		{
			isInventoryUse = !isInventoryUse;


			if (minimapUI.activeSelf == true)
			{
				isMinimapUse = false;
			}
			if (optionUI.activeSelf == true)
			{
				isOptionUse = false;
			}
			if (deckUI.activeSelf == true)
			{
				isDeckUse = false;
			}
			if (CemeteryUI.activeSelf == true)
			{
				isCemeteryUse = false;
			}

			isCardUse = false;
			SetStateUI();
			CardManager.Inst.SetCardStateCannot();
		}
	}

	public void DeckToggle()
	{
		if (isDeckUse)
		{
			SetClose();
		}
		else if (isUIUse)
		{
			isDeckUse = !isDeckUse;


			if (minimapUI.activeSelf == true)
			{
				isMinimapUse = false;
			}
			if (optionUI.activeSelf == true)
			{
				isOptionUse = false;
			}
			if (inventoryUI.activeSelf == true)
			{
				isInventoryUse = false;
			}
			if (CemeteryUI.activeSelf == true)
			{
				isCemeteryUse = false;
			}

			isCardUse = false;
			SetStateUI();
			CardManager.Inst.SetCardStateCannot();
		}
	}

	public void CemeteryToggle()
	{
		if (isCemeteryUse)
		{
			SetClose();
		}
		else if (isUIUse)
		{
			isCemeteryUse = !isCemeteryUse;


			if (minimapUI.activeSelf == true)
			{
				isMinimapUse = false;
			}
			if (optionUI.activeSelf == true)
			{
				isOptionUse = false;
			}
			if (inventoryUI.activeSelf == true)
			{
				isInventoryUse = false;
			}
			if (deckUI.activeSelf == true)
			{
				isDeckUse = false;
			}

			isCardUse = false;
			SetStateUI();
			CardManager.Inst.SetCardStateCannot();
		}
	}

	#endregion


	public void GameOverButton()
    {
		Application.Quit();
	}



}