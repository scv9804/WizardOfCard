using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() 
    { 
		Inst = this; 
      DontDestroyOnLoad(this.gameObject); // 이 부분이 씬 로드하면서 버그가 걸림 ... 왜인지는 조금 더 연구해 볼 필요가 있음.
    }

    [Header("Develop")]


    [Tooltip("시작 카드 개수를 정합니다.")]
    [SerializeField]public int i_StartCardsCount;

    [SerializeField]
    NotificationPanel notificationPanel;

    [Header("Properties")]
    [SerializeField]
    //특수전일때만 false로 할 생각임. 항상 선공일 수도 있음.
    public bool myTurn = true;
    // 게임이 끝나면 True로 바꿔서 Entity 클릭 방지.
    public bool isLoding;

    WaitForSeconds delay_07 = new WaitForSeconds(0.7f);
    WaitForSeconds enemyDelay = new WaitForSeconds(1.7f);



    //event 안붙임, GameManager에서 관리하기 편하게 하기 위해
    public static Action onAddCard;
    public static Action <bool> onStartTurn;
	public static Action enemyActions;

	bool IsCombatScene = false;

	public bool isCombatScene { get { return IsCombatScene; } set { IsCombatScene = value; } }

    // 턴시작
    public IEnumerator Co_StartTurn()
    {
		if (IsCombatScene)
		{
			isLoding = true;
			onStartTurn?.Invoke(myTurn);

			if (myTurn == true)
			{
				UIManager.Inst.TurnEndButtonActivae();				
				TurnNotification_Bool(true);
				CardManager.Inst.SetCardStateCannot();			
				yield return delay_07;
				onAddCard?.Invoke();
				yield return delay_07;
				EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
				UIManager.Inst.IsUIUse = true;
				isLoding = false;
			}
			else
			{
				TurnNotification_Bool(false);
				yield return enemyDelay;
				enemyActions?.Invoke();
				UIManager.Inst.IsUIUse = false;
				isLoding = false;
			}
			EntityManager.Inst.StartAllSpineAnimation();
			CardManager.Inst.SetECardState();
		}

		/* ㅇㅇ 좆댐 다뿌숴그냥 뿌숴뿌숴
		if ((room.RoomEventType == 1 || room.RoomEventType == 0) && !room.isStartRoom)
		{
			if (myTurn == true)
			{
				UIManager.Inst.TurnEndButtonActivae();
				//GameManager.Inst.Notification("내 턴");
				TurnNotification_Bool(true);
				CardManager.Inst.SetCardStateCannot();
				yield return delay_07;
				onAddCard?.Invoke();
				yield return delay_07;
				EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
				UIManager.Inst.IsUIUse = true;
				isLoding = false;
			}
			else
			{
				TurnNotification_Bool(false);
				yield return delay_07;
				yield return delay_07;
				UIManager.Inst.IsUIUse = false;
				isLoding = false;
			}
			CardManager.Inst.SetECardState();

		}
		else
		{
			CardManager.Inst.e_CardStats = E_CardStats.CanMouseOver;
			EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
			UIManager.Inst.IsUIUse = true;
			isLoding = false;
			Debug.Log("전투방아님 //TurnManager");
		}
		*/

	}

	// 턴종료시 호출, 턴 바꾸려면 얘호출하면 줄줄이 호출임
/*	public void OnAddCard()
	{
        onAddCard?.Invoke();
    }*/

    public void TurnNotification_Bool(bool _myTurn)
    {
		notificationPanel = GameObject.Find("Notification Panel").GetComponent<NotificationPanel>();
		notificationPanel.Show(_myTurn);
    }
}
