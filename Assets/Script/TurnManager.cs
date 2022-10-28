using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Inst { get; private set; }
    void Awake() 
    { Inst = this; 
      DontDestroyOnLoad(this.gameObject); // �� �κ��� �� �ε��ϸ鼭 ���װ� �ɸ� ... �������� ���� �� ������ �� �ʿ䰡 ����.
    }

    [Header("Develop")]


    [Tooltip("���� ī�� ������ ���մϴ�.")]
    [SerializeField]public int i_StartCardsCount;

    [SerializeField]
    NotificationPanel notificationPanel;

    [Header("Properties")]
    [HideInInspector]
    //Ư�����϶��� false�� �� ������. �׻� ������ ���� ����.
    public bool myTurn = true;
    // ������ ������ True�� �ٲ㼭 Entity Ŭ�� ����.
    public bool isLoding;

    WaitForSeconds delay_07 = new WaitForSeconds(0.7f);



    //event �Ⱥ���, GameManager���� �����ϱ� ���ϰ� �ϱ� ����
    public static Action onAddCard;
    public static Action <bool> onStartTurn;

    // �Ͻ���
    public IEnumerator Co_StartTurn(Room room)
    {
        isLoding = true;
        onStartTurn?.Invoke(myTurn);

        if ((room.RoomEventType == 1 || room.RoomEventType == 0) && !room.isStartRoom)
        {
            if (myTurn == true)
            {
                UIManager.Inst.TurnEndButtonActivae();
                //GameManager.Inst.Notification("�� ��");
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
                UIManager.Inst.TurnEndButtonActivae();
                // GameManager.Inst.Notification("��� ��");
                TurnNotification_Bool(false);
                yield return delay_07;
                yield return delay_07;
                StartCoroutine(EntityManager.Inst.EnemyEntityAttack());
                Debug.Log("Attack");
                UIManager.Inst.IsUIUse = false;
                isLoding = false;
            }
            CardManager.Inst.SetECardState();

        }
        else
        {
            CardManager.Inst.e_CardStats = CardManager.E_CardStats.CanMouseOver;
            EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
            UIManager.Inst.IsUIUse = true;
            isLoding = false;
            Debug.Log("������ƴ� //TurnManager");
        }


    }

    // ������� ȣ��, �� �ٲٷ��� ��ȣ���ϸ� ������ ȣ����
    public void OnAddCard()
	{
        onAddCard?.Invoke();
    }

    public void TurnNotification_Bool(bool _myTurn)
    {
        notificationPanel.Show(_myTurn);
    }
}
