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

	public void ButtonDeActivate()
	{
		roomMoveButton_L.gameObject.SetActive(false);
		roomMoveButton_R.gameObject.SetActive(false);
		roomMoveButton_U.gameObject.SetActive(false);
		roomMoveButton_D.gameObject.SetActive(false);
	}

	public void MinimapToggel()
	{
		if (minimapUI.activeSelf == true)
		{
			minimapUI.SetActive(false);
		}
		else if(minimapUI.activeSelf == false)
		{
			minimapUI.SetActive(true);
		}


		if (optionUI.activeSelf == true)
		{
			optionUI.SetActive(false);
		}
		if (inventoryUI.activeSelf == true)
		{
			inventoryUI.SetActive(false);
		}
	}

	public void OptionToggle()
	{
		if (optionUI.activeSelf == true)
		{
			optionUI.SetActive(false);
		}
		else if (optionUI.activeSelf == false)
		{
			optionUI.SetActive(true);
		}


		if (minimapUI.activeSelf == true)
		{
			minimapUI.SetActive(false);
		}
		if (inventoryUI.activeSelf == true)
		{
			inventoryUI.SetActive(false);
		}
	}

	public void InventoryToggle()
	{
		if (inventoryUI.activeSelf == true)
		{
			inventoryUI.SetActive(false);
		}
		else if (optionUI.activeSelf == false)
		{
			inventoryUI.SetActive(true);
		}


		if (minimapUI.activeSelf == true)
		{
			minimapUI.SetActive(false);
		}
		if (optionUI.activeSelf == true)
		{
			optionUI.SetActive(false);
		}
	}

	#endregion


	public void InventoryCancleButton()
	{
		inventoryUI.gameObject.SetActive(false);
	}

	public void GameOverButton()
    {
		Application.Quit();
	}



}
