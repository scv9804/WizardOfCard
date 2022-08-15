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
    [SerializeField]int i_StartCardsCount;

    [SerializeField]
    NotificationPanel notificationPanel;

    [Header("Properties")]
    [HideInInspector]
    //Ư�����϶��� false�� �� ������. �׻� ������ ���� ����.
    public bool myTurn = true;
    // ������ ������ True�� �ٲ㼭 Entity Ŭ�� ����.
    public bool isLoding;

    WaitForSeconds delay_07 = new WaitForSeconds(0.7f);
    WaitForSeconds delay_01 = new WaitForSeconds(0.1f);


    //event �Ⱥ���, GameManager���� �����ϱ� ���ϰ� �ϱ� ����
    public static Action onAddCard;
    public static Action <bool> onStartTurn;

    public IEnumerator Co_StartGame()
    {
        isLoding = true;

        for (int i = 0; i < i_StartCardsCount; i ++)
        {
            yield return delay_01;
            onAddCard?.Invoke();
        }
        StartCoroutine(Co_StartTurn());
    }

    // �Ͻ���
    public IEnumerator Co_StartTurn()
    {
        isLoding = true;
        onStartTurn?.Invoke(myTurn);


        if (myTurn == true)
        {
            //GameManager.Inst.Notification("�� ��");
            TurnNotification_Bool(true);
            CardManager.Inst.SetCardStateCannot();
            yield return delay_07;
            onAddCard?.Invoke();
            yield return delay_07;
            EntityManager.Inst.playerEntity.Status_Aether = EntityManager.Inst.playerEntity.Status_MaxAether;
            isLoding = false;
        }
        else
        {
            // GameManager.Inst.Notification("��� ��");
            TurnNotification_Bool(false);
            yield return delay_07;
            yield return delay_07;
            StartCoroutine(EntityManager.Inst.EnemyEntityAttack());
            Debug.Log("Attack");
            isLoding = false;
        }
        CardManager.Inst.SetECardState();
    }

    // ������� ȣ��, �� �ٲٷ��� ��ȣ���ϸ� ������ ȣ����
    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(Co_StartTurn());
    }


    public void SetMyTurn()
	{
        myTurn = true;
        StartCoroutine(Co_StartTurn());
	}

    public void TurnNotification_Bool(bool _myTurn)
    {
        notificationPanel.Show(_myTurn);
    }

}
