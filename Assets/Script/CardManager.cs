using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	[SerializeField] List<Card> myCards;
	[SerializeField] List<Card> myCemetery;
	[SerializeField] Transform cardSpawnPos;
	[SerializeField] Transform LeftCard_Tf;
	[SerializeField] Transform RightCard_Tf;
	[SerializeField] Transform UseCard_Tf;
	[SerializeField] Transform cardGarbage_Tf;
	[SerializeField] E_CardStats e_CardStats;
	[SerializeField] float f_useCardSize;
	[SerializeField] float f_garbageCardSize;
	[SerializeField] float f_arrangementSize;



	[SerializeField] Transform[] useCardPath_OnHand_Tf;
	public Vector3[] v3_cardPaths_onHand;


	[SerializeField] List<Item> itemBuffer;
	[SerializeField] List<Item> myDeck;
	

	enum E_CardStats { Cannot, CanMouseOver, CanAll };


	public Card selectCard;
	[HideInInspector] public int i_ManaCost;
	[HideInInspector] public bool is_myCardDrag;
	[HideInInspector] public bool is_useCardArea;
	[HideInInspector] public bool is_canUseCard = true;
	[HideInInspector] public bool is_cardUsing;
	[HideInInspector] public bool is_useEnhance;


	private void Start()
	{
		myDeck = new List<Item>(50);
		SetupMyDeck();
		v3_cardPaths_onHand = new Vector3[3];
		for (int i = 0; i < 6; i++)
		{
			AddDeck();
		}
		TurnManager.onAddCard += AddCard;
	}

	void Update()
	{
		if (is_myCardDrag)
		{
			CardDrag();
		}
		SetECardState();
		DetectCardArea();
		SetCardEnable();

		GameTick_CardManager();

				if (Input.GetKeyDown(KeyCode.R))
				{
					AddCard();
				}
				if (Input.GetKeyDown(KeyCode.T))
				{
					AddDeck();
				}
				if (Input.GetKeyDown(KeyCode.Y))
				{
					DeckShuffle();
				}
	}

	private void OnDestroy()
	{
		TurnManager.onAddCard -= AddCard;
	}


	//益撹幻糾. 蟹掻拭呪舛 Update拭 績獣稽 酵焼隔製 せせせ;;;
	public void GameTick_CardManager()
	{
		UIManager.Inst.SetDeckCountUI(myDeck.Count);
		UIManager.Inst.SetCemeteryCountUI(myCemetery.Count);
		UIManager.Inst.SetManaUI();
	}



	// 畿拭辞 朝球嗣奄
	public Item PopItem()
	{
		//恥 朝球 0舌戚檎 陥獣 嗣奄
		if (myDeck.Count == 0)
		{
			SetupMyDeck();
		}
		//朝球 嗣奄 
		Item item = myDeck[0];
		myDeck.RemoveAt(0);
		return item;
	}







	public void DeckShuffle()
	{
		for (int i = 0; i < myDeck.Count; i++)
		{
			int rand = UnityEngine.Random.Range(i, myDeck.Count);
			Item temp = myDeck[i];
			myDeck[i] = myDeck[rand];
			myDeck[rand] = temp;
		}
	}

	//need retouch // 22.5.25 呪舛
	void SetupMyDeck()
	{
		myDeck = new List<Item>(100);
		// 焼戚奴 獄遁拭 蓄亜
		for (int i = 0; i < itemSO.items.Length; i++)
		{
			Item item = itemSO.items[i];
			for (int j = 0; j < item.f_percentage; j++)
			{
				myDeck.Add(item);
			}
		}
		DeckShuffle();
	}

	// 謝鳶蓄亜 (蓄亜) 悪鉢朝球 紫遂 獣 damage 0聖 左鎧辞 汽耕走 呪帖研 奄沙 葵生稽 痕井 <<訊戚係惟 幻糾? 蟹 佐重昔亜砂 せせ 悦汽 呪舛亀 瑛諾焼辞 硝焼辞背推 呪遭松 せせせせせせ;;;;;
	public void AddCard()
	{
		var tempt = PopDeck();
		if (tempt == null)
		{
			Debug.Log("Please Refresh Deck");
		}
		else
		{
			var cardObject = Instantiate(cardPrefab, cardSpawnPos.position, Quaternion.identity);
			var card = cardObject.GetComponent<Card>();
			card.Setup(tempt);
			DrowCardExplainRefresh(card);
			myCards.Add(card);
			setOriginOrder();
			CardAlignment();
		}
	}

	void DrowCardExplainRefresh(Card _card)
    {
		if (!is_useEnhance)
		{
			_card.i_explainDamage = _card.i_explainDamageOrigin;
		}
		else
		{
			_card.i_explainDamage *= BattleCalculater.Inst.i_enhacneVal;
		}
	}



	// 奄沙 傾戚嬢 壕帖
	void setOriginOrder()
	{
		int count = myCards.Count;
		for (int i = 0; i < count; i++)
		{
			var targetCard = myCards[i];
			targetCard?.GetComponent<OrderLayer>().SetOriginOrder(i);
		}
	}


	//朝球 壕帖 滴奄繕舛 匂敗. Set Card Pos,Pos
	void CardAlignment()
	{
		List<Pos_Rot_Scale> originCardPRSs = new List<Pos_Rot_Scale>();
		//朝球 神軒走劾 滴奄人 是帖 繕舛.
		originCardPRSs = RoundAlignment(LeftCard_Tf, RightCard_Tf, myCards.Count, 0.5f, Vector3.one /10);

		for (int i = 0; i < myCards.Count; i++)
		{
			var targetCard = myCards[i];

			targetCard.originPRS = originCardPRSs[i];
			targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
		}
	}
	//据莫 壕帖
	List<Pos_Rot_Scale> RoundAlignment(Transform _Left_tf, Transform _Right_tf, int _objCount, float _height, Vector3 _scale)
	{
		float[] objLerps = new float[_objCount];
		List<Pos_Rot_Scale> results = new List<Pos_Rot_Scale>(_objCount);

		switch (_objCount)
		{
			// 1~3 猿走澗 噺穿 蒸戚
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

		//据莫 壕帖 敗呪
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

	void AddDeck()
	{
		myDeck.Add(PopItem());
	}

	public Item PopDeck()
	{
		if (myDeck.Count == 0)
		{
			Debug.Log("Deck is Empty");
		}
		else
		{
			Item item = myDeck[0];
			myDeck.RemoveAt(0);
			return item;
		}
		return null;
	}



	#endregion

	/// <summary>
	/// 朝球 球掘益 殖 球罫, 紫遂馬奄 // Card Drag and Drop, Using Cards Scripts
	/// </summary>
	/// <param name="_card"></param>
	#region MyCard

	public void CardMouseOver(Card _card)
	{
		if (e_CardStats == E_CardStats.Cannot || is_cardUsing)
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

	//朝球 兜奄 (朝球紫遂)
	public void CardMouseUp(Card _card)
	{
		is_myCardDrag = false;
		if (e_CardStats != E_CardStats.CanAll)
		{	
			return;
		}

		//朝球紫遂照馬檎 据是帖.
		if (is_canUseCard)
		{
			Debug.Log("朝球紫遂");
			// 朝球 紫遂
			if (_card.i_manaCost <= EntityManager.Inst.playerEntity.i_aether)
			{
				is_cardUsing = true;
				UseCardInArea(_card);
				EntityManager.Inst.SetUseCard(_card);
				Debug.Log("朝球紫遂");
			}
			
		}
		else
		{
			// 朝球 照紫遂
			UseCardSetDefult();
			SetCardDisable();
			Debug.Log("朝球 耕紫遂");
		}


	}

	void CardDrag()
	{
		selectCard.MoveTransform(new Pos_Rot_Scale(Utility.MousePos, Quaternion.identity, Vector3.one * 0.1f), false);
	}

	//朝球 壕帖 鎮虞戚希 溌昔
	void DetectCardArea()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(Utility.MousePos, Vector3.forward);
		int layer = LayerMask.NameToLayer("UseCardArea");
		//壕伸拭 析帖馬澗 推社亜 蒸澗走 伊紫 bool 展脊 鋼発.
		is_useCardArea = Array.Exists(hits, _hit => _hit.collider.gameObject.layer == layer);
	}


	// 朝球 壕帖
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

	void SetECardState()
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


	//朝球 紫遂 亜管馬惟 幻級奄
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

	// 朝球 耕 紫遂獣
	void SetCardDisable()
    {
		is_cardUsing = false;
		is_canUseCard = false;
	}

	// 朝球 紫遂獣 硲窒 (溌企 貢 是帖 繕舛, 朝球 紫遂 梅生檎 照闇球軒惟 馬奄.)
	void UseCardInArea(Card _card)
    {
		selectCard.MoveTransform(new Pos_Rot_Scale(UseCard_Tf.position, Quaternion.identity, Vector3.one * f_useCardSize), false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(true);
	
		_card.is_Useable_Card = false;
	}
  
	//朝球 紫遂 昼社.
	public void CancelUseCard()
    {
        if (is_cardUsing)
        {
			is_cardUsing = !is_cardUsing;
			UseCardSetDefult();
			EntityManager.Inst.DelectUseCard();
		}
	}

	//据是帖
	public void UseCardSetDefult()
    {
		selectCard.MoveTransform(selectCard.originPRS, false);
		selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(false);
		selectCard.is_Useable_Card = true;
	}


	public void UseCardSetmyCemetery()
    {
		//魚稽 幻級形陥亜 源紹製	
		int ran = UnityEngine.Random.Range(0, useCardPath_OnHand_Tf.Length);
		v3_cardPaths_onHand[0] = cardGarbage_Tf.position;
		v3_cardPaths_onHand[1] = useCardPath_OnHand_Tf[ran].position;


		selectCard.MoveTransformGarbage(v3_cardPaths_onHand, f_garbageCardSize, 0.5f);
		myCemetery.Add(selectCard);
		myCards.Remove(selectCard);
		CardAlignment();
	}



	public void ShuffleCemetery()
	{
		int tempt = myCemetery.Count;

		if (tempt == 0)
		{
			Debug.Log("孔走亜 搾醸柔艦陥!");
			return;
		}


		for (int i =0; tempt > i; i++)
		{
			myDeck.Add(myCemetery[0].item);
			myCemetery.RemoveAt(0);
		}
		DeckShuffle();
	}

	public void ShuffleHand()
	{
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
		}
	}


	// 悪鉢 朝球 紫遂獣 硲窒
	public void UseEnhanceRefresh(int _timesVal)
    {
		//悪鉢朝球澗 焼還
		foreach (Card _c in myCards)
		{
			switch (_c.i_cardType)
			{
				case 0: //Spell
					{
						_c.i_damage = _c.i_explainDamageOrigin * _timesVal;
						break;
					}
				case 1: //Spell_Enhance
					{
						break;
					}
				case 2: // Shiled
					{
						_c.i_damage = _c.i_explainDamageOrigin * _timesVal;
						break;
					}
				case 3: //Heal
					{
						_c.i_damage = _c.i_explainDamageOrigin * _timesVal;
						break;
					}
				case 4:  //Buff
					{
						break;
					}
				case 5: //Debuff
					{
						break;
					}
			}
		}

		myCards.ForEach(x => x.ExplainRefresh());
	}

	//悪鉢朝球 紫遂 板 陥献 朝球 紫遂獣 硲窒
	public void AfterUseEnhance()
    {
		myCards.ForEach(x => x.i_damage = x.i_explainDamageOrigin);

		foreach (Card _c in myCards)
		{
			if (_c.i_cardType == 1) // 1 == spellEnhance (enum item's Num) 
			{
				_c.i_damage = _c.i_explainDamageOrigin;
			}
		}

		myCards.ForEach(x => x.ExplainRefresh());
	}

	#endregion

}
