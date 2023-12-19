using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class RoomEventListScript : MonoBehaviour
{
	[Header("�̺�Ʈ â")]
	[SerializeField] protected GameObject eventWindow;

	[Header("�⺻ ����")]
	[SerializeField] protected Sprite StatueSprite;
	[SerializeField] protected SpriteRenderer changedSpriteRenderer;
	[SerializeField] protected GameObject changeObject;


	[Header("�̺�Ʈ ����")]
	[SerializeField, TextArea] protected string explainEvent;
	[SerializeField, TextArea] protected string eventReward;
	[SerializeField, TextArea] protected string eventPay;

	//[Header("�̺�Ʈ TMP")]

	protected TMP_Text explainTMP;
	protected TMP_Text rewardTMP;
	protected TMP_Text payTMP;
	protected Button acceptButton;
	protected Button refuseButton;

	public abstract GameObject Event();
	public virtual void ExitRoom()
	{
		changeObject.SetActive(false);
	}

	public void SetObejects()
	{
		explainTMP = eventWindow.transform.GetChild(0).GetComponent<TMP_Text>();
		rewardTMP = eventWindow.transform.GetChild(1).GetComponent<TMP_Text>();
		payTMP = eventWindow.transform.GetChild(2).GetComponent<TMP_Text>();
		acceptButton = eventWindow.transform.GetChild(3).GetComponent<Button>();
		refuseButton = eventWindow.transform.GetChild(4).GetComponent<Button>();		
	}

	public void setEventWindow()
	{
		eventWindow.SetActive(true);
		rewardTMP.text = eventReward;
		explainTMP.text = explainEvent;
		payTMP.text = eventPay;
	}
	public void SpawnStatue()
	{
		changedSpriteRenderer.sprite = StatueSprite;
		changeObject.SetActive(true);
	}
}
