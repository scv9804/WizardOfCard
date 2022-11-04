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

	public static int i_usingCardCount = 0; // <<22-10-30 장형용 :: 실행 중인 카드 사용 수>>

	public static int i_usedCardCount; // 비전 광선꺼 옮겨옴

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

		// <<22-10-21 장형용 :: 추가>>		
		Utility.onBattleStart += ShuffleExiledCard;
		// 22-10-24 이동화 :: 이건 장비로 넣자
		//Utility.onBattleStart += ShuffleCemetery;
		//Utility.onBattleStart += RefreshMyHand;

		// 비전 광선꺼 옮겨옴
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

		// <<22-10-21 장형용 :: 추가>>
		Utility.onBattleStart -= ShuffleExiledCard;

		// 22-10-24 이동화 :: 얘들도 장비에 넣자 (장착 시)
		//	Utility.onBattleStart -= ShuffleCemetery;
		//	Utility.onBattleStart -= RefreshMyHand;
	}

    private void OnDisable()
    {
		Utility.onCardUsed -= IncreaseCardCount;

		TurnManager.onStartTurn -= ResetCardCount;
	}

    // 비전 광선꺼 옮겨옴
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


	//그냥만듦. 나중에수정 Update에 임시로 박아넣음 ㅋㅋㅋ;;;
	public void GameTick_CardManager()
	{
		UIManager.Inst.SetDeckCountUI(myDeck.Count);
		UIManager.Inst.SetCemeteryCountUI(myCemetery.Count);
		UIManager.Inst.SetManaUI();
	}



	// 덱에서 카드뽑기
	public Card PopItem()
	{
		//총 카드 0장이면 다시 뽑기
		if (myDeck.Count == 0)
		{
			SetupMyDeck();
		}
		//카드 뽑기 
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

	//need retouch // 22.5.25 수정
	void SetupMyDeck()
	{
		myDeck = new List<Card>(100);
		// 아이템 버퍼에 추가
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


	// 손패추가 (추가) 강화카드 사용 시 damage 0을 보내서 데미지 수치를 기본 값으로 변경 <<왜이렇게 만듦? 나 병신인가봄;;
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

	public void InstantinateCard(Card tempt) // <<22-10-30 장형용 :: 좋은 기능 있길래 쓰려고 분리했읍니다 ㅎㅎ;;;>>
    {
		var cardObject = Instantiate(itemSO.items[tempt.i_itemNum].card_object, cardSpawnPos.position, Quaternion.identity);
		var card = cardObject.GetComponent<Card>();
		//card.SetItemSO(tempt.card_info); // <<22-11-04 장형용 :: 현재 카드 정보를 대부분 Card_Info로 옮기기 위해 분리>>
		card.Setup();
		myCards.Add(card);
		setOriginOrder();
		CardAlignment();
	}


	// 기본 레이어 배치
	void setOriginOrder()
	{
		int count = myCards.Count;
		for (int i = 0; i < count; i++)
		{
			var targetCard = myCards[i];
			targetCard?.GetComponent<OrderLayer>().SetOriginOrder(i);
		}
	}


	//카드 배치 크기조정 포함. Set Card Pos,Pos
	void CardAlignment()
	{
		List<Pos_Rot_Scale> originCardPRSs = new List<Pos_Rot_Scale>();
		//카드 오리지날 크기와 위치 조정.
		originCardPRSs = RoundAlignment(LeftCard_Tf, RightCard_Tf, myCards.Count, 0.5f, Vector3.one / 4);


		for (int i = 0; i < myCards.Count; i++)
		{
			var targetCard = myCards[i];

			targetCard.originPRS = originCardPRSs[i];
			targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
		}
	}
	//원형 배치 //안좋은 함수긴함....
	List<Pos_Rot_Scale> RoundAlignment(Transform _Left_tf, Transform _Right_tf, int _objCount, float _height, Vector3 _scale)
	{
		float[] objLerps = new float[_objCount];
		List<Pos_Rot_Scale> results = new List<Pos_Rot_Scale>(_objCount);

		switch (_objCount)
		{
			// 1~3 까지는 회전 없이
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

		//원형 배치 함수
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
	/// 카드 드래그 엔 드롭, 사용하기 // Card Drag and Drop, Using Cards Scripts
	/// </summary>
	/// <param name="_card"></param>
	#region MyCard

	public void CardMouseOver(Card _card)
	{
		if (e_CardStats == E_CardStats.Cannot || is_cardUsing || is_myCardDrag) // <<22-10-21 장형용 :: 카드 드래그 여부 확인 추가>>
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

	//카드 놓기 (카드사용)
	public void CardMouseUp(Card _card)
	{
		is_myCardDrag = false;
		if (e_CardStats != E_CardStats.CanAll)
		{	
			return;
		}



		//카드사용안하면 원위치.
		if (is_canUseCard)
		{
			Debug.Log("카드사용");
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
			// 카드 사용
			else if (AttackRange_Single(_card))
			{
				is_cardUsing = true;
				UseCardInArea(_card);
				EntityManager.Inst.SetUseCard(_card);
				Debug.Log("카드사용");
			}
			
		}
		else
		{
			// 카드 안사용
			UseCardSetDefult();
			SetCardDisable();
			Debug.Log("카드 미사용");
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

    //카드 배치 컬라이더 확인
    void DetectCardArea()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(Utility.MousePos, Vector3.forward);
		int layer = LayerMask.NameToLayer("UseCardArea");
		//배열에 일치하는 요소가 없는지 검사 bool 타입 반환.
		is_useCardArea = Array.Exists(hits, _hit => _hit.collider.gameObject.layer == layer);
	}


	// 카드 배치
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


	//카드 사용 가능하게 만들기
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

	// 카드 미 사용시
	void SetCardDisable()
    {
		is_cardUsing = false;
		is_canUseCard = false;
	}

	// 카드 사용시 호출 (확대 및 위치 조정, 카드 사용 했으면 안건드리게 하기.)
	void UseCardInArea(Card _card)
    {
		selectCard.MoveTransform(new Pos_Rot_Scale(UseCard_Tf.position, Quaternion.identity, Vector3.one * f_useCardSize), false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(true);
	
		_card.is_Useable_Card = false;
	}
  
	//카드 사용 취소.
	public void CancelUseCard()
    {
        if (is_cardUsing)
        {
			is_cardUsing = !is_cardUsing;
			UseCardSetDefult();
			EntityManager.Inst.DelectUseCard();
		}
	}

	//원위치
	public void UseCardSetDefult()
    {
		selectCard.MoveTransform(selectCard.originPRS, false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(false);
		selectCard.is_Useable_Card = true;
	}

	public void UseCardSetmyCemetery() 
    {
		//따로 만들려다가 말았음	
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
			Debug.Log("묘지가 비었습니다!");
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
		UIManager.Inst.canHandRefresh = false; // <<22-10-26 장형용 :: 추가>>

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

		UIManager.Inst.canHandRefresh = true; // <<22-10-26 장형용 :: 추가>>
	}

	public void ShuffleExiledCard() // <<22-10-21 장형용 :: 추가>>
	{
		int tempt = myExiledCards.Count;

		if (tempt == 0)
		{
			Debug.Log("제외된 카드가 없습니다!");
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


	//public void RefreshMyHand() // <<22-10-21 장형용 :: 추가, 초기화하는 과정에서 카드가 잠시 줄어들었다 확대되는 버그?가 생기는데 뭔가 그럴듯하니 걍 두기로>>
	//{
	//	int tempt = myCards.Count;

	//	if (tempt == 0)
	//	{
	//		Debug.Log("패에 카드가 없습니다!");
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