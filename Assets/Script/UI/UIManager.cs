using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		SetClose();
	}


	public Button roomMoveButton_L;
	public Button roomMoveButton_R;
	public Button roomMoveButton_U;
	public Button roomMoveButton_D;

	public TMP_Text DeckCountTMP_UI;
	public TMP_Text CemeteryCountTMP_UI;
	public TMP_Text HealthTMP_UI;
	public TMP_Text myturn_UI_TMP;

	public TMP_Text ManaTMP_UI;
	public GameObject optionUI;
	public GameObject minimapUI;
	public GameObject inventoryUI;
	public GameObject deckUI;
	public GameObject CemeteryUI;

	public GameObject CardCancleArea;
	public GameObject optionCancleArea;
	public GameObject minimapCancleArea;
	public GameObject inventoryCancleArea;
	public GameObject gameClearBack_UI;
	public GameObject Reword_UI;


	bool isDeckUse;
	bool isCardUse;
	bool isOptionUse;
	bool isMinimapUse;
	bool isCemeteryUse;
	bool isInventoryUse;
	bool isUIUse;

	[SerializeField] LevelGeneration levelGeneration;

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
		if (TurnManager.Inst.myTurn )
			CardManager.Inst.CemeteryRefesh();		
	}

	public void HandRefresh()
	{
		if (TurnManager.Inst.myTurn )
			CardManager.Inst.HandRefresh();
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
		RewordManager.Inst.AddClearReword();
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

		Debug.Log("??");
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
	//	CardManager.Inst.SetCardStateBack();
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
