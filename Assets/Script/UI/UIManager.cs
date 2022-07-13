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

	[SerializeField] GameObject Minimap;
	[SerializeField]LevelGeneration levelGeneration;




	public void CemeteryRefresh()
	{
		if (TurnManager.Inst.myTurn == true && EntityManager.Inst.playerEntity.i_aether > 0)
		{
			CardManager.Inst.ShuffleCemetery();
			EntityManager.Inst.playerEntity.i_aether -= 1;
		}
		
		if(TurnManager.Inst.myTurn == true && EntityManager.Inst.playerEntity.i_aether <= 0)
		{
			TurnManager.Inst.EndTurn();
		}			
	}

	public void HandRefresh()
	{
		if (TurnManager.Inst.myTurn == true && EntityManager.Inst.playerEntity.i_aether > 0)
		{
			CardManager.Inst.ShuffleHand();
			EntityManager.Inst.playerEntity.i_aether -= 1;
		}
		if (TurnManager.Inst.myTurn == true && EntityManager.Inst.playerEntity.i_aether <= 0)
		{
			TurnManager.Inst.EndTurn();
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
		ManaTMP_UI.text = EntityManager.Inst.playerEntity.i_aether.ToString() + "/" + EntityManager.Inst.playerEntity.MAXAETHER.ToString();
	}



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

	public void ButtonDeActivate()
	{
		roomMoveButton_L.gameObject.SetActive(false);
		roomMoveButton_R.gameObject.SetActive(false);
		roomMoveButton_U.gameObject.SetActive(false);
		roomMoveButton_D.gameObject.SetActive(false);
	}

	public void MinimapToggel()
	{
		if (Minimap.activeSelf)
		{
			Minimap.SetActive(false);
		}
		else
		{
			Minimap.SetActive(true);
		}		
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


	public void GameOverButton()
    {
		Application.Quit();
	}



}
