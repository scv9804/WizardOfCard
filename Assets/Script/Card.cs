using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;
using System.Text;
using System;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;

public class Card : MonoBehaviour
{   
	// <<22-11-04 ������ :: �ڵ� ���� �� ���� �ý��� �߰�>>
	[Header("ī�� ���� ������")]
	[Tooltip("ī�� ���� �����ͺ��̽�"), SerializeField] ItemSO itemSO;

	[Header("ī�� ����Ʈ ��������Ʈ")]
	[Tooltip("�ǰ� ����Ʈ")] public Sprite enemyDamageSprite;
	[Tooltip("���� ����Ʈ")] public Sprite playerAttackSprite;

	[Header("ī�� �⺻ ���� ������")]
	[Tooltip("ī�� �̸�")] public string st_cardName;
	[Tooltip("ī�� ��ȣ")] public int i_itemNum;
	[Tooltip("ī�� ��͵�")] public float f_percentage;

	[Header("ī�� ��ȭ Ƚ��")]
	[Range(0, 2), SerializeField] int upgraded;

    [Header("ī�� �⺻ ���� ������")] 
	[Tooltip("ī�� ������ �̹���")] public Sprite[] cardImage;
	[Tooltip("ī�� ������ �̹���")] public Sprite[] cardIconImage;

	[Space(10)]
	[Tooltip("ī�� ���"), SerializeField] int[] cost = new int[3];
	//int[] attack; // <<22-11-24 ������ :: ����>>
	[Tooltip("ī�� ���� ����"), SerializeField] bool[] isExile = new bool[3];
	[Tooltip("ī�� �з�")] public CardType[] cardType = new CardType[3];
	[Tooltip("ī�� ��� ����"), SerializeField] AttackRange[] AR_attackRange = new AttackRange[3];
	[Tooltip("ī�� ����"), TextArea(3, 5)] public string[] explainCard = new string[3]; // ��� �ʿ��ؼ� �ۺ����� �ٲ� �޼��� �߰��ұ� �ߴµ� �ϴ� �״�ε�

	// ī�� �������̽�
	// <<22-12-01 ������ :: ���� �Ҵ� �߰� �� �ϰ������� ��ȣ ���� private�� ����>>
	TMP_Text nameTMP;
	TMP_Text manaCostTMP;
	TMP_Text explainTMP;

	SpriteRenderer sr_card;
	SpriteRenderer sr_cardIcon;

	[HideInInspector] public Pos_Rot_Scale originPRS;

	[HideInInspector] public bool is_Useable_Card = true;
	[HideInInspector] public bool is_UI_Card = false;

	protected StringBuilder sb = new StringBuilder();

    //[HideInInspector] public int bonus; // ���� �̻��, ������ ���� ����

    // <<22-10-27 ������ :: �߰�>>
    protected int i_enhanceValue_inst = 0;
	// <<22-11-09 ������ :: ����>>
	//protected int i_magicAffinity_turn_inst = 0;
	//protected int i_magicAffinity_battle_inst = 0;
	//protected int i_magicAffinity_permanent_inst = 0;
	//protected int T_magicResistance_inst = 0;

	#region OnlyUnityEditor[protected bool isUsing]
#if UNITY_EDITOR
	protected bool isUsing = false;
#endif
	#endregion

	#region Job System

	MagicAffinityJob myMagicAffinityJob = new MagicAffinityJob();
    MagicResistanceJob myMagicResistanceJob = new MagicResistanceJob();
    EnhanceValueJob myEnhanceValueJob = new EnhanceValueJob();
	HealValueJob myHealValueJob = new HealValueJob();

	#endregion

	// <<22-11-04 ������ :: ���� card_info�� �ִ� ���� �̸��� �����ϴ� ��� ������Ƽ�� ȣȯ�ǰ� �ص�>>
	// ���� �����ϸ� �ϳ��ϳ� �ٲ��ִ°� �±� �ѵ� �ʹ� ������... Ư�� i_damage...
	#region Properties

	public int i_upgraded
    {
		get { return upgraded; }

		set { if(value < -1 || value > 3) upgraded = value; }
	}

	public CardType CardType
	{
		get { return cardType[i_upgraded]; }
	}

	public AttackRange attackRange
	{
		get { return AR_attackRange[i_upgraded]; }

		set { AR_attackRange[i_upgraded] = value; }
	}

	public int i_manaCost
	{
		get { return cost[i_upgraded]; }

		set { cost[i_upgraded] = value; }
	}

	protected string st_explain
	{
		get { return explainCard[i_upgraded];}
	}

	public bool b_isExile
	{
		get { return isExile[i_upgraded]; }

		set { isExile[i_upgraded] = value; }
	}

	//// <<22-11-24 ������ :: ����>>
	//protected int i_damage	

	// <<22-12-01 ������ :: �߰�>>
	public Sprite CardImage
	{
		get { return cardImage[i_upgraded]; }
	}

	public Sprite CardIconImage
	{
		get { return cardIconImage[i_upgraded]; }
	}

	#endregion

	// �ڵ� ġ�� �����Ƽ� ���� �ڵ� ����� ������Ƽ
	#region Code Shortening

	//Card

	protected IEnumerator PlayAttackSprite
    {
		get { return Player.AttackSprite(Player.playerChar.MagicBoltSprite, playerAttackSprite); }
    }

	// EntityManager

	protected PlayerEntity Player
    {
		get { return EntityManager.Inst.playerEntity; }
    }

	protected Entity RandomTarget
    {
		get { return EntityManager.Inst.SelectRandomTarget(); }
    }

	protected List<Entity> Enemies
    {
		get { return EntityManager.Inst.enemyEntities; }
    }

	// CardManager

	protected List<Card> MyHandCards
    {
		get { return CardManager.Inst.myCards; }
    }

	protected List<Card> MyCemeteryCards
	{
		get { return CardManager.Inst.myCemetery; }
	}

	#endregion

	protected virtual void Awake()
	{
		try
		{
			// <<22-12-01 ������ :: ���� �𸣰ڴ� �����Ҵ� ON>>
			nameTMP = transform.GetChild(0).GetComponent<TMP_Text>();
			manaCostTMP = transform.GetChild(2).GetComponent<TMP_Text>();
			explainTMP = transform.GetChild(3).GetComponent<TMP_Text>();

			sr_card = GetComponent<SpriteRenderer>();
			sr_cardIcon = transform.GetChild(4).GetComponent<SpriteRenderer>();

			Setup();
			RefreshCardUI();

			is_UI_Card = false;
		}
		catch
		{
			Debug.Log("ī�� �¾� �� �� �����");
		}
	}

	protected virtual void Start() { }

	protected virtual void OnDisable() { }

//	#region OnlyUnityEditor[void Update()]
//#if UNITY_EDITOR
//	void Update()
//	{
//		if (!isUsing)
//			RefreshCardUI();
//	}
//#endif
	//#endregion

	// <<22-11-09 ������ :: �߰�>>
	public virtual void Setup()
	{
		i_manaCost = itemSO.items[i_itemNum].card.i_manaCost;
		b_isExile = itemSO.items[i_itemNum].card.b_isExile;
		AR_attackRange = itemSO.items[i_itemNum].card.AR_attackRange;

		i_enhanceValue_inst = 1;
	}

    #region ī�� UI ����

	// <<22-11-09 ������ :: �߰�>>
	public void RefreshCardUI()
	{
		manaCostTMP.text = i_manaCost.ToString();
		CardExplainRefresh();
		CardNameRefresh();
		CardImageRefresh();
	}

	// <<22-11-24 ������ :: �Լ� �и��� ����>>
	//public virtual string ExplainRefresh()

	// <<22-11-24 ������ :: ����>>
	public void CardExplainRefresh()
    {
	//		explainTMP.text = GetCardExplain();
	}

	#region  // <<22-11-24 ������ :: �߰�, ��ġ �� �ؽ�Ʈ ���� ó���� ���������� ó������ �ʰ� �� ī�忡�� ���������� ������, ��ġ �� ��� ��Ģ ����>>
	// ��> ���� ��,						 �� �ڵ� �������� ����
	// ��> ���� ģȭ���� ��ȭ ��ġ ����, �� #ff0000(Red) ����
	// ��> ���� ���׷°� ��ȭ ��ġ ����, �� #0000ff(Blue) ����
	// ��> ��ȭ ��ġ ����,			 	 �� #ff00ff(Magenta) ����
	#endregion
	public virtual string GetCardExplain()
    {
		i_enhanceValue_inst = Player.Buff_EnchaneValue;

		sb.Clear();

		if (b_isExile)
			sb.Append("����\n");

		sb.Append(st_explain);

		sb.Replace("����", "<color=#ff00ff>����</color>");
		sb.Replace("��ȣ", "<color=#0000ff>��ȣ</color>");
		sb.Replace("��ο� �Ұ�", "<color=#ff0000>��ο� �Ұ�</color>");

		return sb.ToString();
    }

	// <<22-11-04 ������ :: �߰�>>
	// <<22-11-24 ������ :: �Լ� �и� �� ����>>
	//public void NameRefresh()

	// <<22-11-24 ������ :: ����>>
	public void CardNameRefresh()
    {
		nameTMP.text = GetCardName();
	}

	// <<22-11-24 ������ :: �߰�>>
	public string GetCardName()
    {
		sb.Clear();

		sb.Append(st_cardName);

		switch(i_upgraded)
        {
			case 0:
				sb.Append(" I");
				break;
			case 1:
				sb.Append(" II");
				break;
			case 2:
				sb.Append(" III");
				break;
		}

		return sb.ToString();
	}
	public void CardImageRefresh()
	{
		sr_card.sprite = CardImage;
		sr_cardIcon.sprite = CardIconImage;
	}

	#endregion

	#region ī�� �̵�

	//ī�� ��ο� ������ �߰� �ؾ��� ��ҵ� ����.
	public void MoveTransform(Pos_Rot_Scale _prs, bool _isUseDotween, float _DotweenTime = 0)
    {
		if (_isUseDotween)
		{
			transform.DOMove(_prs.pos, _DotweenTime);
			transform.DORotateQuaternion(_prs.rot, _DotweenTime);
			transform.DOScale(_prs.scale , _DotweenTime);
		}
        else
        {
			transform.position = _prs.pos;
			transform.rotation = _prs.rot;
			transform.localScale = _prs.scale;
        }
    }

	public void MoveTransformGarbage(Vector3[] _prs, float _Scale, float _DotweenTime = 0)
	{
		this.transform.DOScale(new Vector3(1f, 1f, 1f) * _Scale, _DotweenTime).SetEase(Ease.InBack);

		//���ٽ�(������) ����ؼ� ��� ������ ���������.
		Sequence sequence1 = DOTween.Sequence()
		.Append(transform.DORotate(new Vector3(0, 0, -120), 0.45f).SetEase(Ease.OutCirc))
		.Append(transform.DOPath(_prs, 0.6f, PathType.CubicBezier, PathMode.Sidescroller2D, 5).SetLookAt(new Vector3(0,0,-120), new Vector3(0, 0 ,-120)).SetEase(Ease.InQuad))
        .AppendCallback(() => { this.gameObject.SetActive(false); }); // �ѹ��Թ̴�~
        //.AppendCallback(() => { Destroy( this.gameObject); }); // ���� ������ ������ ������ �׳� ����
    }

	#endregion

	// <<22-10-21 ������ :: �߰�>>
	#region ī�� ��ġ ���

	// <<22-11-09 ������ :: ����>>
	protected int ApplyMagicAffinity(int _value)
	{
		#region _value += PlayerEntity.Inst.Status_MagicAffinity;

		NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
        result[0] = _value;

		myMagicAffinityJob.value = result;
		myMagicAffinityJob.magicAffinity = Player.Buff_MagicAffinity;

		JobHandle firstJob = myMagicAffinityJob.Schedule();

		firstJob.Complete();

        _value = result[0];

        result.Dispose();

        #endregion

        return ApplyEnhanceValue(_value);
	}

	// <<22-11-09 ������ :: ����>>
	protected int ApplyMagicResistance(int _value)
	{
		#region _value += PlayerEntity.Inst.Status_MagicResistance;

		NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
        result[0] = _value;

		myMagicResistanceJob.value = result;
		myMagicResistanceJob.magicResistance = Player.Buff_MagicResistance;

		JobHandle firstJob = myMagicResistanceJob.Schedule();

		firstJob.Complete();

        _value = result[0];

        result.Dispose();

        #endregion

        return ApplyEnhanceValue(_value);
	}

    // <<22-11-09 ������ :: ����>>
    protected int ApplyEnhanceValue(int _value)
    {
		#region _value *= i_enhanceValue_inst;

		NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
        result[0] = _value;

		myEnhanceValueJob.value = result;
		myEnhanceValueJob.enhanceValue = i_enhanceValue_inst;

		JobHandle firstJob = myEnhanceValueJob.Schedule();

		firstJob.Complete();

		_value = result[0];

        result.Dispose();

        #endregion

        return _value;
	}

	// <<22-12-03 ������ :: ����>>
	protected int ApplyHealValue(int _value)
	{
		#region _value += Player.Buff_Heal;

		NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
		result[0] = _value;

		myHealValueJob.value = result;
		myHealValueJob.heal = Player.Buff_Heal;

		JobHandle firstJob = myHealValueJob.Schedule();

		firstJob.Complete();

		_value = result[0];

		result.Dispose();

		#endregion

		return ApplyEnhanceValue(_value);
	}

	#endregion

	#region ī�� ���

	// <<22-10-28 ������ :: ����>>
	public virtual IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		Player.Status_Aether -= i_manaCost;

		#region OnlyUnityEditor[isUsing = true]
#if UNITY_EDITOR
		isUsing = true;
#endif
		#endregion

		CardManager.i_usingCardCount++;

		Utility.onCardUsed?.Invoke(this);

		i_enhanceValue_inst = Player.Buff_EnchaneValue;

		// <<22-11-24 ������ :: ��ȭ ��ġ�� ������� �ʴ��� �ʱ�ȭ�ǵ��� ����>>
		Player.ResetEnhanceValue();

		yield return null;
	}

	// Ƚ����ŭ �޼ҵ带 �ݺ�
	// <<22-10-26 ������ :: �߰�>>
	protected IEnumerator Repeat(Action myMethodName, int _count)
	{
		for (int i = 0; i < _count; i++)
		{
			// �ƴ� ī�� ���� ���� ����ϴµ�?
			myMethodName();

			// DoTween ���ð�(1.05f) �����ϸ鼭 �۾��� ��
			yield return new WaitForSeconds( FindMin( 1.0f / _count));
		}

		yield return null;
	}

	float FindMin(float _time)
    {
		if(_time > 0.15f)
			return 0.15f;
		else
			return _time;
	}

	// �� ��ü���� �޼ҵ� �ݺ�
	// <<22-10-26 ������ :: �߰�>>
	protected void TargetAll(Action myMethodName, ref Entity _target)
	{
		for (int i = 0; i < Enemies.Count; i++)
		{
			_target = Enemies[i];

            if (!_target.is_die)
				myMethodName();
		}
	}

	protected IEnumerator EndUsingCard()
	{
		CardManager.i_usingCardCount--;

		CardManager.Inst.RefreshMyHands();

		#region OnlyUnityEditor[isUsing = false]
#if UNITY_EDITOR
		isUsing = false;
#endif
		#endregion

		yield return null;
	}

	#endregion

	#region �̺�ƮƮ����

	private void OnMouseOver()
    {
        if (is_Useable_Card && !is_UI_Card)
		{
			CardManager.Inst.CardMouseOver(this);
			CardManager.Inst.is_mouseOnCard = true;
		}
			
	}

    private void OnMouseExit()
	{
		if (is_Useable_Card && !is_UI_Card)
		{
			CardManager.Inst.CardMouseExit(this);
			CardManager.Inst.is_mouseOnCard = false;
		}
			
	}

	private void OnMouseDown()
	{
		if (is_Useable_Card && !is_UI_Card)
		{
			CardManager.Inst.CardMouseDown();	
		}
	}

	private void OnMouseUp()
	{
		if (is_Useable_Card && !is_UI_Card)
		{
			CardManager.Inst.CardMouseUp(this);
			CardManager.Inst.is_mouseOnCard = false;
		}
			
	}

	#endregion
}

#region Card Effects Interfaces

public interface IAttack
{
	int Damage { get; }

	void Attack(Entity _target);
	void Attack(PlayerEntity _target);
}

public interface IShield
{
	int Shield { get; }

	void GainShield();
}

public interface IBurning
{
	int Burning { get; }

	void AddBurning(Entity _target);
	void AddBurning(PlayerEntity _target);
}

public interface IManaAffinity_Turn
{
	int ManaAffinity_Turn { get; }

	void GainManaAffinity_Turn();
}

public interface IManaAffinity_Battle
{
	int ManaAffinity_Battle { get; }

	void GainManaAffinity_Battle();
}

public interface IRestoreHealth
{
	int Health { get; }

	void RestoreHealth();
}

public interface IRestoreAether
{
	int Aether { get; }

	void RestoreAether();
}

public interface IEnhance
{
	int EnhanceValue { get; }

	void Enahnce();
}

public interface IProtection
{
	int Protection { get; }

	void GainProtection();
}

#endregion
