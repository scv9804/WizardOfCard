using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CardManager : MonoBehaviour
{
	public static CardManager Inst { get; private set; }
	private void Awake()
	{
		Inst = this;
		DontDestroyOnLoad(this.gameObject);
	}


	[SerializeField] ItemSO itemSO;
	[SerializeField] GameObject cardPrefab;


	public List<Card> myCards;
	/*[HideInInspector] */public List<Card> myCemetery;
	List<Card> itemBuffer;
	[HideInInspector] public List<Card> myDeck;

	/*[HideInInspector] */public List<Card> myExiledCards;

	[SerializeField] Transform cardSpawnPos;
	[SerializeField] Transform LeftCard_Tf;
	[SerializeField] Transform RightCard_Tf;
	[SerializeField] Transform UseCard_Tf;
	[SerializeField] Transform cardGarbage_Tf;
	[SerializeField] public E_CardStats e_CardStats = E_CardStats.Cannot;


	[SerializeField] float f_useCardSize;
	[SerializeField] float f_garbageCardSize;
	[SerializeField] float f_arrangementSize;



	[SerializeField] Transform[] useCardPath_OnHand_Tf;
	public Vector3[] v3_cardPaths_onHand;


	public enum E_CardStats { Cannot, CanMouseOver, CanAll };


	public Card selectCard;
	[HideInInspector] public int i_ManaCost;
	[HideInInspector] public bool is_myCardDrag;
	[HideInInspector] public bool is_useCardArea;
	[HideInInspector] public bool is_canUseCard = true;
	[HideInInspector] public bool is_cardUsing;
	[HideInInspector] public bool is_useEnhance;

	public static int i_usingCardCount = 0; // <<22-10-30 ������ :: ���� ���� ī�� ��� ��>>

	public static int i_usedCardCount; // ���� ������ �Űܿ�

	[SerializeField] int[] test = new int[5];


	private void Start()
	{
		SetupMyDeck();
		v3_cardPaths_onHand = new Vector3[3];
		for (int i = 0; i < 6; i++)
		{
			AddDeck();
		}
		TurnManager.onAddCard += AddCard;

		// <<22-10-21 ������ :: �߰�>>		
		Utility.onBattleStart += ShuffleExiledCard;
		// 22-10-24 �̵�ȭ :: �̰� ���� ����
		//Utility.onBattleStart += ShuffleCemetery;
		//Utility.onBattleStart += RefreshMyHand;

		// ���� ������ �Űܿ�
		Utility.onCardUsed += IncreaseCardCount;

		TurnManager.onStartTurn += ResetCardCount;
	}

	void Update()
	{
		if (is_myCardDrag)
		{
            CardDrag();
        }
		DetectCardArea();
		SetCardEnable();
	}

	private void FixedUpdate()
	{
		GameTick_CardManager();
	}

	private void OnDestroy()
	{
		TurnManager.onAddCard -= AddCard;

		// <<22-10-21 ������ :: �߰�>>
		Utility.onBattleStart -= ShuffleExiledCard;

		// 22-10-24 �̵�ȭ :: ��鵵 ��� ���� (���� ��)
		//	Utility.onBattleStart -= ShuffleCemetery;
		//	Utility.onBattleStart -= RefreshMyHand;
	}

    private void OnDisable()
    {
		Utility.onCardUsed -= IncreaseCardCount;

		TurnManager.onStartTurn -= ResetCardCount;
	}

    // ���� ������ �Űܿ�
    void IncreaseCardCount(Card _card)
	{
		i_usedCardCount++;

		RefreshMyHands();
	}

	void ResetCardCount(bool isMyTurn)
	{
		i_usedCardCount = 0;

		RefreshMyHands();
	}


	//�׳ɸ���. ���߿����� Update�� �ӽ÷� �ھƳ��� ������;;;
	public void GameTick_CardManager()
	{
		UIManager.Inst.SetDeckCountUI(myDeck.Count);
		UIManager.Inst.SetCemeteryCountUI(myCemetery.Count);
		UIManager.Inst.SetManaUI();
	}



	// ������ ī��̱�
	public Card PopItem()
	{
		//�� ī�� 0���̸� �ٽ� �̱�
		if (myDeck.Count == 0)
		{
			SetupMyDeck();
		}
		//ī�� �̱� 
		Card card = myDeck[0];
		myDeck.RemoveAt(0);
		return card;
	}


	public void DeckShuffle()
	{
		for (int i = 0; i < myDeck.Count; i++)
		{
			int rand = UnityEngine.Random.Range(i, myDeck.Count);
			Card temp = myDeck[i];
			myDeck[i] = myDeck[rand];
			myDeck[rand] = temp;
		}
	}

	//need retouch // 22.5.25 ����
	void SetupMyDeck()
	{
		myDeck = new List<Card>(100);
		// ������ ���ۿ� �߰�
		for (int i = 0; i < itemSO.items.Length; i++)
		{
			Card card = itemSO.items[i].card;
			for (int j = 0; j < card.f_percentage; j++)
			{
				myDeck.Add(card);
			}
		}
		DeckShuffle();
	}


	// �����߰� (�߰�) ��ȭī�� ��� �� damage 0�� ������ ������ ��ġ�� �⺻ ������ ���� <<���̷��� ����? �� �����ΰ���;;
	public void AddCard()
	{
		var tempt = PopDeck();
		if (tempt == null)
		{
			Debug.Log("Please Refresh Deck");
		}
		else
		{
			//var cardObject = Instantiate(cardPrefab, cardSpawnPos.position, Quaternion.identity);
			InstantinateCard(tempt);
		}
	}

	public void InstantinateCard(Card tempt) // <<22-10-30 ������ :: ���� ��� �ֱ淡 ������ �и������ϴ� ����;;;>>
    {
		var cardObject = Instantiate(itemSO.items[tempt.i_itemNum].card_object, cardSpawnPos.position, Quaternion.identity);
		var card = cardObject.GetComponent<Card>();
		//card.SetItemSO(tempt.card_info); // <<22-11-04 ������ :: ���� ī�� ������ ��κ� Card_Info�� �ű�� ���� �и�>>
		card.Setup();
		myCards.Add(card);
		setOriginOrder();
		CardAlignment();
	}


	// �⺻ ���̾� ��ġ
	void setOriginOrder()
	{
		int count = myCards.Count;
		for (int i = 0; i < count; i++)
		{
			var targetCard = myCards[i];
			targetCard?.GetComponent<OrderLayer>().SetOriginOrder(i);
		}
	}


	//ī�� ��ġ ũ������ ����. Set Card Pos,Pos
	void CardAlignment()
	{
		List<Pos_Rot_Scale> originCardPRSs = new List<Pos_Rot_Scale>();
		//ī�� �������� ũ��� ��ġ ����.
		originCardPRSs = RoundAlignment(LeftCard_Tf, RightCard_Tf, myCards.Count, 0.5f, Vector3.one / 4);


		for (int i = 0; i < myCards.Count; i++)
		{
			var targetCard = myCards[i];

			targetCard.originPRS = originCardPRSs[i];
			targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
		}
	}
	//���� ��ġ //������ �Լ�����....
	List<Pos_Rot_Scale> RoundAlignment(Transform _Left_tf, Transform _Right_tf, int _objCount, float _height, Vector3 _scale)
	{
		float[] objLerps = new float[_objCount];
		List<Pos_Rot_Scale> results = new List<Pos_Rot_Scale>(_objCount);

		switch (_objCount)
		{
			// 1~3 ������ ȸ�� ����
			case 1: objLerps = new float[] { 0.5f }; break;
			case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
			case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
			default:
				float interval = 1f / (_objCount - 1);
				for (int i = 0; i < _objCount; i++)
				{
					objLerps[i] = interval * i;
				}
				break;
		}

		//���� ��ġ �Լ�
		for (int i = 0; i < _objCount; i++)
		{
			var targetPos = Vector3.Lerp(LeftCard_Tf.position, RightCard_Tf.position, objLerps[i]);
			var targetRot = Quaternion.identity;
			if (_objCount >= 4)
			{
				float curve = Mathf.Sqrt(Mathf.Pow(_height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
				targetPos.y += curve;
				targetRot = Quaternion.Slerp(_Left_tf.rotation, _Right_tf.rotation, objLerps[i]);
			}
			results.Add(new Pos_Rot_Scale(targetPos, targetRot, _scale));
		}
		return results;
	}


	#region Deck

	public void AddDeck()
	{
		myDeck.Add(PopItem());
	}

	public Card PopDeck()
	{
		if (myDeck.Count == 0)
		{
			Debug.Log("Deck is Empty");
		}
		else
		{
			Card card = myDeck[0];
			myDeck.RemoveAt(0);
			return card;
		}
		return null;
	}



	#endregion

	/// <summary>
	/// ī�� �巡�� �� ���, ����ϱ� // Card Drag and Drop, Using Cards Scripts
	/// </summary>
	/// <param name="_card"></param>
	#region MyCard

	public void CardMouseOver(Card _card)
	{
		if (e_CardStats == E_CardStats.Cannot || is_cardUsing || is_myCardDrag) // <<22-10-21 ������ :: ī�� �巡�� ���� Ȯ�� �߰�>>
		{
			return;
		}
		else
		{
			selectCard = _card;
			EnlargementCard(true, _card);
		}
	}

	public void CardMouseExit(Card _card)
	{
		if (e_CardStats == E_CardStats.Cannot || is_cardUsing)
		{

		}
		else
		{
			EnlargementCard(false, _card);
		}
	}

	public void CardMouseDown()
	{
		
		if (e_CardStats != E_CardStats.CanAll || is_cardUsing)
		{
			return;
		}
		is_myCardDrag = true;
	}

	//ī�� ���� (ī����)
	public void CardMouseUp(Card _card)
	{
		is_myCardDrag = false;
		if (e_CardStats != E_CardStats.CanAll)
		{	
			return;
		}



		//ī������ϸ� ����ġ.
		if (is_canUseCard)
		{
			Debug.Log("ī����");
			if (AttackRange_Self(_card))
			{
				UseCardInArea(_card);
				EntityManager.Inst.SetUseCard(_card);
				EntityManager.Inst.UseCard_Self();
			}
			else if (AttackRange_AllEnemy(_card))
			{
				UseCardInArea(_card);
				EntityManager.Inst.SetUseCard(_card);
				EntityManager.Inst.UseCard_AllEnemy();

			}
			// ī�� ���
			else if (AttackRange_Single(_card))
			{
				is_cardUsing = true;
				UseCardInArea(_card);
				EntityManager.Inst.SetUseCard(_card);
				Debug.Log("ī����");
			}
			
		}
		else
		{
			// ī�� �Ȼ��
			UseCardSetDefult();
			SetCardDisable();
			Debug.Log("ī�� �̻��");
		}


	}

	#region AttackRange Bool
	bool AttackRange_Self(Card _card)
	{
		if (_card.i_manaCost <= EntityManager.Inst.playerEntity.Status_Aether && _card.attackRange == AttackRange.Target_Self)
		{
			return true;
		}
		return false; 
	}
	bool AttackRange_AllEnemy(Card _card)
	{
		if (_card.i_manaCost <= EntityManager.Inst.playerEntity.Status_Aether && _card.attackRange == AttackRange.Target_AllEnemy)
		{
			return true;
		}
		return false;
	}
	bool AttackRange_Single(Card _card)
	{
		if (_card.i_manaCost <= EntityManager.Inst.playerEntity.Status_Aether && _card.attackRange == AttackRange.Target_Single)
		{
			return true;
		}
		return false;
	}
    #endregion

    void CardDrag()
    {
        selectCard.MoveTransform(new Pos_Rot_Scale(Utility.MousePos, Quaternion.identity, Vector3.one * 0.1f), false);
    }

    //ī�� ��ġ �ö��̴� Ȯ��
    void DetectCardArea()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(Utility.MousePos, Vector3.forward);
		int layer = LayerMask.NameToLayer("UseCardArea");
		//�迭�� ��ġ�ϴ� ��Ұ� ������ �˻� bool Ÿ�� ��ȯ.
		is_useCardArea = Array.Exists(hits, _hit => _hit.collider.gameObject.layer == layer);
	}


	// ī�� ��ġ
	void EnlargementCard(bool _isEnlarge, Card _card)
	{
		if (_isEnlarge)
		{
			Vector3 enlargePos = new Vector3(_card.originPRS.pos.x, -3f, -10f);
			_card.MoveTransform(new Pos_Rot_Scale(enlargePos, Quaternion.identity, Vector3.one * f_arrangementSize), false);
		}
		else
		{
			_card.MoveTransform(_card.originPRS, false);
		}
		_card.GetComponent<OrderLayer>().SetMostFrontOrder(_isEnlarge);
	}

	public void SetECardState()
	{
		if (TurnManager.Inst.isLoding)
		{
			e_CardStats = E_CardStats.Cannot;
		}
		else if (!TurnManager.Inst.myTurn)
		{

				e_CardStats = E_CardStats.CanMouseOver;
			
		}
		else if (TurnManager.Inst.myTurn)
		{
			e_CardStats = E_CardStats.CanAll;
		}
	}


	//ī�� ��� �����ϰ� �����
	void SetCardEnable()
	{
        if (is_myCardDrag && is_useCardArea)
        {
			is_canUseCard = true;
        }
        else
        {
			is_canUseCard = false;
        }
	}

	// ī�� �� ����
	void SetCardDisable()
    {
		is_cardUsing = false;
		is_canUseCard = false;
	}

	// ī�� ���� ȣ�� (Ȯ�� �� ��ġ ����, ī�� ��� ������ �Ȱǵ帮�� �ϱ�.)
	void UseCardInArea(Card _card)
    {
		selectCard.MoveTransform(new Pos_Rot_Scale(UseCard_Tf.position, Quaternion.identity, Vector3.one * f_useCardSize), false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(true);
	
		_card.is_Useable_Card = false;
	}
  
	//ī�� ��� ���.
	public void CancelUseCard()
    {
        if (is_cardUsing)
        {
			is_cardUsing = !is_cardUsing;
			UseCardSetDefult();
			EntityManager.Inst.DelectUseCard();
		}
	}

	//����ġ
	public void UseCardSetDefult()
    {
		selectCard.MoveTransform(selectCard.originPRS, false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(false);
		selectCard.is_Useable_Card = true;
	}

	public void UseCardSetmyCemetery() 
    {
		//���� ������ٰ� ������	
		int ran = UnityEngine.Random.Range(0, useCardPath_OnHand_Tf.Length);
		v3_cardPaths_onHand[0] = cardGarbage_Tf.position;
		v3_cardPaths_onHand[1] = useCardPath_OnHand_Tf[ran].position;


		selectCard.MoveTransformGarbage(v3_cardPaths_onHand, f_garbageCardSize, 0.5f);

		if(selectCard.b_isExile)
		{
			myExiledCards.Add(selectCard);
		}
        else
        {
			myCemetery.Add(selectCard);
		}

		myCards.Remove(selectCard);
		CardAlignment();
	}



	public void ShuffleCemetery()
	{
		int tempt = myCemetery.Count;

		if (tempt == 0)
		{
			Debug.Log("������ ������ϴ�!");
			return;
		}


		for (int i =0; tempt > i; i++)
		{
			myDeck.Add(myCemetery[0]);
			myCemetery.RemoveAt(0);
		}
		DeckShuffle();
	}

	public IEnumerator ShuffleHand()
	{
		UIManager.Inst.canHandRefresh = false; // <<22-10-26 ������ :: �߰�>>

		int tempt = myCards.Count; 
		for(int i =0;tempt > i ; i++ )
		{
			myCemetery.Add(myCards[0]);
			myCards[0].gameObject.SetActive(false);
			myCards.RemoveAt(0);			
		}
		for (int i = 0; 6 > i; i++)
		{
			AddCard();
			yield return new WaitForSeconds(0.1f);
		}

		UIManager.Inst.canHandRefresh = true; // <<22-10-26 ������ :: �߰�>>
	}

	public void ShuffleExiledCard() // <<22-10-21 ������ :: �߰�>>
	{
		int tempt = myExiledCards.Count;

		if (tempt == 0)
		{
			Debug.Log("���ܵ� ī�尡 �����ϴ�!");
			return;
		}

		for (int i = 0; tempt > i; i++)
		{
			myDeck.Add(myExiledCards[0]);
			myExiledCards.RemoveAt(0);
		}

		DeckShuffle();
	}

	public void RefreshMyHands()
	{
		for (int i = 0; i < myCards.Count; i++)
		{
			myCards[i].ExplainRefresh();
		}
	}


	//public void RefreshMyHand() // <<22-10-21 ������ :: �߰�, �ʱ�ȭ�ϴ� �������� ī�尡 ��� �پ����� Ȯ��Ǵ� ����?�� ����µ� ���� �׷����ϴ� �� �α��>>
	//{
	//	int tempt = myCards.Count;

	//	if (tempt == 0)
	//	{
	//		Debug.Log("�п� ī�尡 �����ϴ�!");
	//		return;
	//	}

	//	for (int i = 0; tempt > i; i++)
	//	{
	//		myCards[i].Setup();
	//	}
	//}

	#endregion


	#region UIManager
	public void HandRefresh()
	{
		if (e_CardStats == E_CardStats.CanAll && EntityManager.Inst.playerEntity.Status_Aether > 0)
		{
			StartCoroutine(Shuffle());
		}
	}

	IEnumerator Shuffle()
	{
		StartCoroutine(ShuffleHand());
		EntityManager.Inst.playerEntity.Status_Aether -= 1;
		yield return new WaitForSeconds(0.3f);
	}

	public void CemeteryRefesh()
	{
		if (e_CardStats == E_CardStats.CanAll && EntityManager.Inst.playerEntity.Status_Aether > 0)
		{
			CardManager.Inst.ShuffleCemetery();
			EntityManager.Inst.playerEntity.Status_Aether -= 1;
		}
	}


	E_CardStats tempt;

	public void SetCardStateCannot()
	{
		tempt = e_CardStats;
		e_CardStats = E_CardStats.Cannot;
	}

	public void SetCardStateBack()
	{
		e_CardStats = tempt;
		if (TurnManager.Inst.myTurn == true)
		{
			e_CardStats = E_CardStats.CanAll;
		}
	}

	public bool BoolCradCanall()
	{
		if (e_CardStats == E_CardStats.CanAll)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	#endregion
}