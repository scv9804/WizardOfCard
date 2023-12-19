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
	// <<22-11-04 장형용 :: 코드 정리 겸 레벨 시스템 추가>>
	[Header("카드 원본 데이터")]
	[Tooltip("카드 원본 데이터베이스"), SerializeField] ItemSO itemSO;

	[Header("카드 이펙트 스프라이트")]
	[Tooltip("피격 이펙트")] public Sprite enemyDamageSprite;
	[Tooltip("공격 이펙트")] public Sprite playerAttackSprite;

	[Header("카드 기본 고정 데이터")]
	[Tooltip("카드 이름")] public string st_cardName;
	[Tooltip("카드 번호")] public int i_itemNum;
	[Tooltip("카드 희귀도")] public float f_percentage;

	[Header("카드 강화 횟수")]
	[Range(0, 2), SerializeField] int upgraded;

    [Header("카드 기본 가변 데이터")] 
	[Tooltip("카드 프레임 이미지")] public Sprite[] cardImage;
	[Tooltip("카드 아이콘 이미지")] public Sprite[] cardIconImage;

	[Space(10)]
	[Tooltip("카드 비용"), SerializeField] int[] cost = new int[3];
	//int[] attack; // <<22-11-24 장형용 :: 삭제>>
	[Tooltip("카드 망각 여부"), SerializeField] bool[] isExile = new bool[3];
	[Tooltip("카드 분류")] public CardType[] cardType = new CardType[3];
	[Tooltip("카드 대상 범위"), SerializeField] AttackRange[] AR_attackRange = new AttackRange[3];
	[Tooltip("카드 설명"), TextArea(3, 5)] public string[] explainCard = new string[3]; // 잠시 필요해서 퍼블릭으로 바꿈 메서드 추가할까 했는데 일단 그대로둠

	// 카드 인터페이스
	// <<22-12-01 장형용 :: 동적 할당 추가 후 일괄적으로 보호 레벨 private로 변경>>
	TMP_Text nameTMP;
	TMP_Text manaCostTMP;
	TMP_Text explainTMP;

	SpriteRenderer sr_card;
	SpriteRenderer sr_cardIcon;

	[HideInInspector] public Pos_Rot_Scale originPRS;

	[HideInInspector] public bool is_Useable_Card = true;
	[HideInInspector] public bool is_UI_Card = false;

	protected StringBuilder sb = new StringBuilder();

    //[HideInInspector] public int bonus; // 현재 미사용, 아이템 구현 대비용

    // <<22-10-27 장형용 :: 추가>>
    protected int i_enhanceValue_inst = 0;
	// <<22-11-09 장형용 :: 제거>>
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

	// <<22-11-04 장형용 :: 기존 card_info에 있던 값들 이름을 수정하는 대신 프로퍼티로 호환되게 해둠>>
	// 성능 생각하면 하나하나 바꿔주는게 맞긴 한데 너무 귀찮다... 특히 i_damage...
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

	//// <<22-11-24 장형용 :: 삭제>>
	//protected int i_damage	

	// <<22-12-01 장형용 :: 추가>>
	public Sprite CardImage
	{
		get { return cardImage[i_upgraded]; }
	}

	public Sprite CardIconImage
	{
		get { return cardIconImage[i_upgraded]; }
	}

	#endregion

	// 코드 치기 귀찮아서 만든 코드 단축용 프로퍼티
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
			// <<22-12-01 장형용 :: 에라 모르겠다 동적할당 ON>>
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
			Debug.Log("카드 셋업 할 수 없어용");
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

	// <<22-11-09 장형용 :: 추가>>
	public virtual void Setup()
	{
		i_manaCost = itemSO.items[i_itemNum].card.i_manaCost;
		b_isExile = itemSO.items[i_itemNum].card.b_isExile;
		AR_attackRange = itemSO.items[i_itemNum].card.AR_attackRange;

		i_enhanceValue_inst = 1;
	}

    #region 카드 UI 갱신

	// <<22-11-09 장형용 :: 추가>>
	public void RefreshCardUI()
	{
		manaCostTMP.text = i_manaCost.ToString();
		CardExplainRefresh();
		CardNameRefresh();
		CardImageRefresh();
	}

	// <<22-11-24 장형용 :: 함수 분리로 삭제>>
	//public virtual string ExplainRefresh()

	// <<22-11-24 장형용 :: 변경>>
	public void CardExplainRefresh()
    {
	//		explainTMP.text = GetCardExplain();
	}

	#region  // <<22-11-24 장형용 :: 추가, 수치 별 텍스트 갱신 처리는 통합적으로 처리하지 않고 각 카드에서 개별적으로 수행함, 수치 별 명명 규칙 변경>>
	// ㄴ> 고정 값,						 색 코드 적용하지 않음
	// ㄴ> 마나 친화성과 강화 수치 적용, 색 #ff0000(Red) 적용
	// ㄴ> 마나 저항력과 강화 수치 적용, 색 #0000ff(Blue) 적용
	// ㄴ> 강화 수치 적용,			 	 색 #ff00ff(Magenta) 적용
	#endregion
	public virtual string GetCardExplain()
    {
		i_enhanceValue_inst = Player.Buff_EnchaneValue;

		sb.Clear();

		if (b_isExile)
			sb.Append("망각\n");

		sb.Append(st_explain);

		sb.Replace("망각", "<color=#ff00ff>망각</color>");
		sb.Replace("보호", "<color=#0000ff>보호</color>");
		sb.Replace("드로우 불가", "<color=#ff0000>드로우 불가</color>");

		return sb.ToString();
    }

	// <<22-11-04 장형용 :: 추가>>
	// <<22-11-24 장형용 :: 함수 분리 후 삭제>>
	//public void NameRefresh()

	// <<22-11-24 장형용 :: 변경>>
	public void CardNameRefresh()
    {
		nameTMP.text = GetCardName();
	}

	// <<22-11-24 장형용 :: 추가>>
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

	#region 카드 이동

	//카드 드로우 움직임 추가 해야할 요소들 많음.
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

		//람다식(시퀀스) 사용해서 모션 끝나면 사라지게함.
		Sequence sequence1 = DOTween.Sequence()
		.Append(transform.DORotate(new Vector3(0, 0, -120), 0.45f).SetEase(Ease.OutCirc))
		.Append(transform.DOPath(_prs, 0.6f, PathType.CubicBezier, PathMode.Sidescroller2D, 5).SetLookAt(new Vector3(0,0,-120), new Vector3(0, 0 ,-120)).SetEase(Ease.InQuad))
        .AppendCallback(() => { this.gameObject.SetActive(false); }); // 롤백함미다~
        //.AppendCallback(() => { Destroy( this.gameObject); }); // 굳이 제거할 이유는 없지만 그냥 제거
    }

	#endregion

	// <<22-10-21 장형용 :: 추가>>
	#region 카드 수치 계산

	// <<22-11-09 장형용 :: 수정>>
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

	// <<22-11-09 장형용 :: 수정>>
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

    // <<22-11-09 장형용 :: 수정>>
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

	// <<22-12-03 장형용 :: 수정>>
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

	#region 카드 사용

	// <<22-10-28 장형용 :: 수정>>
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

		// <<22-11-24 장형용 :: 강화 수치를 사용하지 않더라도 초기화되도록 변경>>
		Player.ResetEnhanceValue();

		yield return null;
	}

	// 횟수만큼 메소드를 반복
	// <<22-10-26 장형용 :: 추가>>
	protected IEnumerator Repeat(Action myMethodName, int _count)
	{
		for (int i = 0; i < _count; i++)
		{
			// 아니 카드 루프 돌다 퇴근하는데?
			myMethodName();

			// DoTween 대기시간(1.05f) 인지하면서 작업할 것
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

	// 적 전체에게 메소드 반복
	// <<22-10-26 장형용 :: 추가>>
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

	#region 이벤트트리거

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
