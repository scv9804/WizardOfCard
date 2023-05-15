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
      DontDestroyOnLoad(this.gameObject); // 戚 採歳戚 樟 稽球馬檎辞 獄益亜 杏顕 ... 訊昔走澗 繕榎 希 尻姥背 瑳 琶推亜 赤製.
    }

    [Header("Develop")]


    [Tooltip("獣拙 朝球 鯵呪研 舛杯艦陥.")]
    [SerializeField]public int i_StartCardsCount;

    [SerializeField]
    NotificationPanel notificationPanel;

    [Header("Properties")]
    [SerializeField]
    //働呪穿析凶幻 false稽 拝 持唖績. 牌雌 識因析 呪亀 赤製.
    public bool myTurn = true;
    // 惟績戚 魁蟹檎 True稽 郊蚊辞 Entity 適遣 号走.
    public bool isLoding;

    WaitForSeconds delay_07 = new WaitForSeconds(0.7f);



    //event 照細績, GameManager拭辞 淫軒馬奄 畷馬惟 馬奄 是背
    public static Action onAddCard;
    public static Action <bool> onStartTurn;
	public static Action enemyActions;

	public bool IsCombatScene = false;


    // 渡獣拙
    public IEnumerator Co_StartTurn(Room room)
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
				Debug.Log("しいしいし");
				yield return delay_07;
				EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
				UIManager.Inst.IsUIUse = true;
				isLoding = false;
			}
			else
			{
				TurnNotification_Bool(false);
				enemyActions?.Invoke();
				UIManager.Inst.IsUIUse = false;
				isLoding = false;
			}
			CardManager.Inst.SetECardState();
		}




		/* しし 楚器 陥姿修益撹 姿修姿修
		if ((room.RoomEventType == 1 || room.RoomEventType == 0) && !room.isStartRoom)
		{
			if (myTurn == true)
			{
				UIManager.Inst.TurnEndButtonActivae();
				//GameManager.Inst.Notification("鎧 渡");
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
			Debug.Log("穿燈号焼還 //TurnManager");
		}
		*/

	}

	// 渡曽戟獣 硲窒, 渡 郊荷形檎 剰硲窒馬檎 匝匝戚 硲窒績
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
