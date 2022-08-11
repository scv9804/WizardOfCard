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


	public Button roomMoveButton_L;
	public Button roomMoveButton_R;
	public Button roomMoveButton_U;
	public Button roomMoveButton_D;

	public TMP_Text DeckCountTMP_UI;
	public TMP_Text CemeteryCountTMP_UI;
	public TMP_Text HealthTMP_UI;

	public TMP_Text ManaTMP_UI;
	public GameObject optionUI;
	public GameObject minimapUI;
	public GameObject inventoryUI;

	public GameObject CardCancleArea;
	public GameObject optionCancleArea;
	public GameObject minimapCancleArea;
	public GameObject inventoryCancleArea;

	bool isCardUse = true;
	bool isOptionUse = false;
	bool isMinimapUse = false;
	bool isInventoryUse = false;
	

	[SerializeField] LevelGeneration levelGeneration;




	public void CemeteryRefresh()
	{
		CardManager.Inst.CemeteryRefesh();			
	}

	public void HandRefresh()
	{
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
		}
		if (levelGeneration.i_Room_R)
		{
			roomMoveButton_R.gameObject.SetActive(true);
		}
		if (levelGeneration.i_Room_U)
		{
			roomMoveButton_U.gameObject.SetActive(true);
		}
		if (levelGeneration.i_Room_D)
		{
			roomMoveButton_D.gameObject.SetActive(true);
		}

	}

	#endregion



	#region ToggleButtons

	void SetStateUI()
	{
		minimapUI.SetActive(isMinimapUse);
		optionUI.SetActive(isOptionUse);
		inventoryUI.SetActive(isInventoryUse);

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
		isMinimapUse = !isMinimapUse; 

		if (optionUI.activeSelf == true)
		{
			isOptionUse = false;
		}
		if (inventoryUI.activeSelf == true)
		{
			isInventoryUse = false;
		}
		isCardUse = false;
		SetStateUI();
		CardManager.Inst.SetCardstateCannot();
	}

	public void OptionToggle()
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
		isCardUse = false;
		SetStateUI();
		CardManager.Inst.SetCardstateCannot();
	}

	public void InventoryToggle()
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
		isCardUse = false;
		SetStateUI();
		CardManager.Inst.SetCardstateCannot();
	}
	#endregion


	public void GameOverButton()
    {
		Application.Quit();
	}



}
