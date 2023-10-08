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

    [Header("ī��Ŵ��� ���� ������")]
    [Tooltip("ī��Ŵ��� ���� �����ͺ��̽�")] public ItemSO itemSO;
    //[SerializeField] GameObject cardPrefab; // �̻��

    // ī�� ����Ʈ
    [Header("ī��Ŵ��� ī�� ����Ʈ")]
    /*[HideInInspector] */
    public List<Card> myCards;
    /*[HideInInspector] */
    public List<Card> myCemetery;
    /*[HideInInspector] */
    public List<Card> myDeck;
    /*[HideInInspector] */
    public List<Card> myExiledCards;
    List<Card> itemBuffer; // �̻��

    [Header("ī��Ŵ��� Ʈ������ ��ġ ����")]
    [Tooltip("��ο� �̵� ���� ��ġ"), SerializeField] Transform cardSpawnPos;
    [Tooltip("���� ���� ī�� �ִ� ��ġ"), SerializeField] Transform LeftCard_Tf;
    [Tooltip("���� ���� ī�� �ִ� ��ġ"), SerializeField] Transform RightCard_Tf;
    [Tooltip("��� �� ī�� ��ġ"), SerializeField] Transform UseCard_Tf;
    [Tooltip("���� �̵� ��ǥ ��ġ"), SerializeField] Transform cardGarbage_Tf;

    [Tooltip("ī�� �̵� ��� ����"), SerializeField] Transform[] useCardPath_OnHand_Tf;
    [Tooltip("ī�� �̵� ��� ��ġ")] public Vector3[] v3_cardPaths_onHand;

    [Header("ī��Ŵ��� ī�� ������ ����")]
    [Tooltip("��� �� ī�� ũ��"), SerializeField] float f_useCardSize;
    [Tooltip("���� �̵� �� ī�� ũ��"), SerializeField] float f_garbageCardSize;
    [Tooltip("���� ��ġ �� ī�� ũ��"), SerializeField] float f_arrangementSize;

    [Header("��Ÿ")]
    [Tooltip("ī�� ��� ���� ����")] public E_CardStats e_CardStats = E_CardStats.Cannot;

    /*[HideInInspector]*/
    public Card selectCard;

    //[HideInInspector] public int i_ManaCost; // �̻��
    [HideInInspector] public bool is_myCardDrag;
    [HideInInspector] public bool is_mouseOnCard;
    [HideInInspector] public bool is_useCardArea; // ����� �ϴµ� �뵵�� ������ ������
    [HideInInspector] public bool is_canUseCard = true;
    [HideInInspector] public bool is_cardUsing;
    //[HideInInspector] public bool is_useEnhance; // �̻��

    // <<22-10-30 ������ :: ���� ���� ī�� ��� ��>>
    public static int i_usingCardCount = 0;

    public static int i_usedCardCount; // ���� ������ �Űܿ�
    public static int i_attackCardCount; // ���� ���ݲ� �Űܿ�

    private void Start()
    {
        SetupMyDeck();

        #region �׼� ���

        TurnManager.onAddCard += AddCard;

        // <<22-10-21 ������ :: �߰�>>
        Utility.onBattleStart += ShuffleExiledCard;
        // 22-10-24 �̵�ȭ :: �̰� ���� ����
        //Utility.onBattleStart += ShuffleCemetery;
        //Utility.onBattleStart += RefreshMyHand;

        #region ArcaneRay

        //Utility.onCardUsed += IncreaseCardCount;

        TurnManager.onStartTurn += ResetCardCount;

        #endregion

        #region ContinuousAttack

        //Utility.onCardUsed += IncreaseAttackCount;

        Utility.onBattleStart += ResetAttackCount;

        #endregion

        #endregion
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
        #region �׼� ��� ����

        TurnManager.onAddCard -= AddCard;

        //// <<22-10-21 ������ :: �߰�>>
        Utility.onBattleStart -= ShuffleExiledCard;

        // 22-10-24 �̵�ȭ :: ��鵵 ��� ���� (���� ��)
        //	Utility.onBattleStart -= ShuffleCemetery;
        //	Utility.onBattleStart -= RefreshMyHand;

        #region ArcaneRay

        //Utility.onCardUsed -= IncreaseCardCount;

        TurnManager.onStartTurn -= ResetCardCount;

        #endregion

        #region ContinuousAttack

        //Utility.onCardUsed -= IncreaseAttackCount;

        Utility.onBattleStart -= ResetAttackCount;

        #endregion

        #endregion
    }

    //�׳ɸ���. ���߿����� Update�� �ӽ÷� �ھƳ��� ������;;;
    public void GameTick_CardManager()
    {
        UIManager.Inst.SetDeckCountUI(myDeck.Count);
        UIManager.Inst.SetCemeteryCountUI(myCemetery.Count);
        UIManager.Inst.SetManaUI();
    }

    E_CardStats tempt;

    public void SetCardStateCannot()
    {
        tempt = e_CardStats;
        e_CardStats = E_CardStats.Cannot;
    }

    public void SetCardSpawnPos()
    {
        cardSpawnPos = GameObject.Find("CardSapwnPos").transform;
        LeftCard_Tf = GameObject.Find("MyCardRight").transform;
        RightCard_Tf = GameObject.Find("MyCardLeft").transform;
        UseCard_Tf = GameObject.Find("UseCardArea").transform;
        cardGarbage_Tf = GameObject.Find("CardGarbagePos").transform;
    }

    public void SetCardStateBack()
    {
        e_CardStats = tempt;
        if (TurnManager.Inst.myTurn == true)
        {
            e_CardStats = E_CardStats.CanAll;
        }
    }

    // <<22-11-07 ������ :: ����>>
    //public bool BoolCradCanall()

    // MonoBehaviour ���� ������ ���� CardManager�� ���� ���� ȿ�� ����
    #region ArcaneRay

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

    #endregion

    // MonoBehaviour ���� ������ ���� CardManager�� ���� ���� ȿ�� ����
    #region ContinuousAttack

    void IncreaseAttackCount(Card _card)
    {
        if (_card.GetType() == typeof(ContinuousAttack))
        {
            i_attackCardCount++;
        }

        RefreshMyHands();
    }

    void ResetAttackCount()
    {
        i_attackCardCount = -1;

        RefreshMyHands();
    }

    #endregion

    // Ex) setOriginOrder(), CardAlignment(), RoundAlignment()
    #region SetCardPRS

    // �⺻ ���̾� ��ġ
    // <<22-11-12 ������ :: ��ȣ ���� ����>>
    public void setOriginOrder()
    {
        int count = myCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<OrderLayer>().SetOriginOrder(i);
        }
    }

    //ī�� ��ġ ũ������ ����. Set Card Pos,Pos
    // <<22-11-12 ������ :: ��ȣ ���� ����>>
    public void CardAlignment()
    {
        List<Pos_Rot_Scale> originCardPRSs = new List<Pos_Rot_Scale>();
        //ī�� �������� ũ��� ��ġ ����.
        originCardPRSs = RoundAlignment(LeftCard_Tf, RightCard_Tf, myCards.Count, 0.5f, Vector3.one / 4);

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f); // �̰� ������ �巡�� ��� �� �����µ� ��ü�Ҹ��� �ڵ� �ֳ�?
        }
    }

    //���� ��ġ, ������ �Լ�����....
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

    #endregion

    // Ex) AddDeck(), DeckShuffle(), SetupMyDeck(), ...
    #region ī��Ŵ��� �� ����

    public void AddDeck()
    {
        myDeck.Add(PopItem());
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

    // need retouch 
    // 22.5.25 ����
    void SetupMyDeck()
    {
        myDeck = new List<Card>(100);
        // ������ ���ۿ� �߰�
        //for (int i = 0; i < itemSO.items.Length; i++)
        //{
        //	Card card = itemSO.items[i].card;
        //	for (int j = 0; j < card.f_percentage; j++)
        //	{
        //		myDeck.Add(card);
        //	}
        //}

        // <<22-11-28 ������ :: ����>>
        for (int i = 0; i < 5; i++)
        {
            myDeck.Add(itemSO.items[0].card);
            myDeck.Add(itemSO.items[1].card);
        }

        //for (int i = 0; i < itemSO.items.Length; i++)
        //	myDeck.Add(itemSO.items[i].card);

        DeckShuffle();
    }

    #endregion

    // Ex) UseCardSetmyCemetery(), CemeteryRefesh(), ...
    #region ī��Ŵ��� ���� ����

    public void UseCardSetmyCemetery()
    {
        //���� ������ٰ� ������	
        int ran = UnityEngine.Random.Range(0, useCardPath_OnHand_Tf.Length);
        v3_cardPaths_onHand[0] = cardGarbage_Tf.position;
        v3_cardPaths_onHand[1] = useCardPath_OnHand_Tf[ran].position;

        selectCard.MoveTransformGarbage(v3_cardPaths_onHand, f_garbageCardSize, 0.5f);

        if (selectCard.b_isExile) // �ѹ��Թ̴�~
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

    public void CemeteryRefesh()
    {
        if (e_CardStats == E_CardStats.CanAll && EntityManager.Inst.playerEntity.Status_Aether > 0)
        {
            EntityManager.Inst.playerEntity.Status_Aether -= 1;

            if (myCemetery.Count == 0)
            {
                Debug.Log("������ ������ϴ�!");
                return;
            }

            for (int i = myCemetery.Count - 1; 0 <= i; i--)
            {
                //myDeck.Add(myCemetery[i]);
                //myCemetery.RemoveAt(i);

                // <<22-11-12 ������ :: �ش� ����� ����ϱ� ���� �и�>>
                ReplaceCardFromCemeteryToDeck(i);
            }

            DeckShuffle();
        }
    }

    // <<22-11-12 ������ :: �ش� ����� ����ϱ� ���� �и�>>
    public void ReplaceCardFromCemeteryToDeck(int _index)
    {
        myDeck.Insert(0, myCemetery[_index]);
        myCemetery.RemoveAt(_index);
    }

    // <<22-11-07 ������ :: �ӽ� ���� ����>>
    // <<22-11-12 ������ :: �Լ� �ϳ��� ����>>
    //public void ShuffleCemetery()

    #endregion

    // Ex) AddCard(), HandRefresh(), ...
    #region ī��Ŵ��� ���� / ��ο� ����

    // �����߰� (�߰�) ��ȭī�� ��� �� damage 0�� ������ ������ ��ġ�� �⺻ ������ ���� <<���̷��� ����? �� �����ΰ���;;
    public void AddCard()
    {
        if (PlayerEntity.Inst != null)
        {
            if (PlayerEntity.Inst.Debuff_CannotDrawCard)
                return;

            var tempt = PopDeck();

            if (tempt == null)
            {
                Debug.Log("Please Refresh Deck");
            }
            else
            {
                InstantinateCard(tempt);
            }
        }
        else
        {
            Debug.Log("!__�÷��̾� ��ƼƼ�� ����ֽ��ϴ�__!");
        }
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

    // <<22-10-30 ������ :: ���� ��� �ֱ淡 ������ �и������ϴ� ����;;;>>
    public void InstantinateCard(Card tempt)
    {
        var cardObject = Instantiate(itemSO.items[tempt.i_itemNum].card_object, cardSpawnPos.position, Quaternion.identity);
        var card = cardObject.GetComponent<Card>();
        card.Setup();
        myCards.Add(card);
        setOriginOrder();
        CardAlignment();
    }

    public void AddSelectCard_Deck(Card tempt)
    {
        myDeck.Add(tempt);
    }

    public IEnumerator HandRefresh()
    {
        if (e_CardStats == E_CardStats.CanAll && EntityManager.Inst.playerEntity.Status_Aether > 0)
        {
            EntityManager.Inst.playerEntity.Status_Aether -= 1;

            // <<22-10-26 ������ :: �߰�>>
            UIManager.Inst.canHandRefresh = false;

            for (int i = myCards.Count - 1; 0 <= i; i--)
            {
                myCemetery.Add(myCards[i]);
                myCards[i].gameObject.SetActive(false);
                myCards.RemoveAt(i);
            }

            for (int i = 0; 6 > i; i++)
            {
                AddCard();

                yield return new WaitForSeconds(0.1f);
            }

            // <<22-10-26 ������ :: �߰�>>
            UIManager.Inst.canHandRefresh = true;
        }

        yield return null;
    }

    // <<22-11-12 ������ :: �ռ� �ϳ��� ����>>
    //public void HandRefresh()
    //IEnumerator Shuffle()

    #endregion

    // CardMouseOver(), CardMouseExit(), CardMouseDown(), CardMouseUp(), AttackRangeBools
    #region ī��Ŵ��� �̺�Ʈ Ʈ���� ����

    public void CardMouseOver(Card _card)
    {
        // <<22-10-21 ������ :: ī�� �巡�� ���� Ȯ�� �߰�>>
        if (e_CardStats == E_CardStats.Cannot || is_cardUsing || is_myCardDrag)
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

        MusicManager.inst?.PlayCardClickSound();
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
            // ī�� ���
            Debug.Log("ī�� ���");

            if (AttackRange_Self(_card))
            {
                UseCardInArea(_card);
                // <<22-11-07 ������ :: ���� �����ͱ淡 ����>>
                //EntityManager.Inst.SetUseCard(_card);
                EntityManager.Inst.UseCard_Self();
            }
            else if (AttackRange_AllEnemy(_card))
            {
                UseCardInArea(_card);
                // <<22-11-07 ������ :: ���� �����ͱ淡 ����>>
                //EntityManager.Inst.SetUseCard(_card);
                EntityManager.Inst.UseCard_AllEnemy();

            }
            else if (AttackRange_Single(_card))
            {
                is_cardUsing = true;
                UseCardInArea(_card);
                // <<22-11-07 ������ :: ���� �����ͱ淡 ����>>
                //EntityManager.Inst.SetUseCard(_card);
            }
            // <<22-11-09 ������ :: �߰�>>
            else
            {
                // ī�� �Ȼ��
                UseCardSetDefult();
                SetCardDisable();
                Debug.Log("ī�� ��� �Ұ�");
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
            // <<22-11-07 ������ :: ���� �����ͱ淡 ����>>
            //EntityManager.Inst.DelectUseCard();
        }
    }

    //����ġ
    public void UseCardSetDefult()
    {
        selectCard.MoveTransform(selectCard.originPRS, false);
        selectCard.GetComponent<OrderLayer>().SetMostFrontOrder(false);
        selectCard.is_Useable_Card = true;
    }

    // <<22-11-07 ������::�ӽ� ���� ����>>
    public IEnumerator ShuffleHand()
    {
        // <<22-10-26 ������ :: �߰�>>
        UIManager.Inst.canHandRefresh = false;

        //int tempt = myCards.Count; 
        //for(int i =0;tempt > i ; i++ )
        //{
        //	myCemetery.Add(myCards[0]);
        //	myCards[0].gameObject.SetActive(false);
        //	myCards.RemoveAt(0);			
        //}

        for (int i = myCards.Count - 1; 0 <= i; i--)
        {
            myCemetery.Add(myCards[i]);
            myCards[i].gameObject.SetActive(false);
            myCards.RemoveAt(i);
        }

        for (int i = 0; 6 > i; i++)
        {
            AddCard();

            yield return new WaitForSeconds(0.1f);
        }

        // <<22-10-26 ������ :: �߰�>>
        UIManager.Inst.canHandRefresh = true;
    }

    // <<22-10-21 ������ :: �߰�>>
    // <<22-11-07 ������ :: �ӽ� ���� ����>>
    public void ShuffleExiledCard()
    {
        //int tempt = myExiledCards.Count;

        //if (tempt == 0)
        //{
        //	Debug.Log("���ܵ� ī�尡 �����ϴ�!");
        //	return;
        //}

        //for (int i = 0; tempt > i; i++)
        //{
        //	myDeck.Add(myExiledCards[0]);
        //	myExiledCards.RemoveAt(0);
        //}

        if (myExiledCards.Count == 0)
        {
            Debug.Log("���ܵ� ī�尡 �����ϴ�!");
            return;
        }

        for (int i = myExiledCards.Count - 1; 0 <= i; i--)
        {
            myDeck.Add(myExiledCards[i]);
            myExiledCards.RemoveAt(i);
        }

        DeckShuffle();
    }

    public void RefreshMyHands()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i]?.CardExplainRefresh();
        }
    }

    // <<22-10-21 ������ :: �߰�, �ʱ�ȭ�ϴ� �������� ī�尡 ��� �پ����� Ȯ��Ǵ� ����?�� ����µ� ���� �׷����ϴ� �� �α��>>
    // <<22-11-09 ������ :: RefreshMyHands�� ȥ�� ���ܼ� �� ������>>
    //public void RefreshMyHand()

    #endregion
}