using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// UI, Result, GameOver
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private ItemSO item;

    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnManager.onAddCard?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TurnManager.Inst.EndTurn();
        }

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
            level.MoveRoom(0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
            level.MoveRoom(1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
            level.MoveRoom(2);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LevelGeneration level = GameObject.Find("LevelGenerator").GetComponent<LevelGeneration>();
            level.MoveRoom(3);	
        }

        // 카드 드로우 관련 항목.
        if (Input.GetKeyDown(KeyCode.R))
        {
            CardManager.Inst.AddCard();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CardManager.Inst.AddDeck();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CardManager.Inst.DeckShuffle();
        }
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.Co_StartGame());
        EntityManager.Inst.SpawnEnemyEntity();
        EntityManager.Inst.SpawnPlayerEntity();
    }

    public void GameTick()
	{
        EntityManager.Inst.CheckDieEveryEnemy();
	}


    public IEnumerator GameOverScene()
	{
        Debug.Log("GameOver");
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameOverScene");
	}

}
