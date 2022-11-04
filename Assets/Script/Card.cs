using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;
using System.Text;
using System;
using System.Linq;

using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{   
	//<<22-11-04 장형용 :: 카드 정리 겸 레벨 시스템 추가>>
	[Header("카드 데이터")]
	[Tooltip("카드 원본 데이터베이스"), SerializeField] ItemSO itemSO;
	[Tooltip("카드 데이터")] public Card_Info card_info;

	[Header("카드 정보 인터페이스")] 
	[SerializeField] TMP_Text nameTMP;
	[SerializeField] TMP_Text manaCostTMP;
	[SerializeField] protected TMP_Text explainTMP;

	/*[SerializeField] */SpriteRenderer sp_card;

	[HideInInspector] public Sprite enemyDamagedEffectSprite;
	[HideInInspector] public Sprite playerAttackEffectSprite;

	[HideInInspector] public Pos_Rot_Scale originPRS;

	[HideInInspector] public bool is_Useable_Card = true;

	protected StringBuilder sb = new StringBuilder();

	[HideInInspector] public int bonus; // 현재는 안씀, 아이템 구현 대비용

    // <<22-10-27 장형용 :: 추가, 이게 진짜 맞나>>
    #region 강화 수치 인스턴스 값

    protected int i_enhenceValue_inst = 0;
	protected int i_magicAffinity_turn_inst = 0;
	protected int i_magicAffinity_battle_inst = 0;
	protected int i_magicAffinity_permanent_inst = 0;
	protected int T_magicResistance_inst = 0;

    #endregion

    //<<22-11-04 장형용 :: 추가>>
    //card_info로 풀어쓰기 보다는 성능 문제 희생하더라도 가시성 챙김 (사실 성능 문제 있을지 없을지는 몰?루...)
    //아무튼 card_info에 있는 값들을 굳이 card_info 안 붙이고 기존처럼 쓸 수 있도록 변경
    #region 프로퍼티

    public int i_CardNum
	{
		get
		{
			return card_info.i_itemNum;
		}

		//set
		//{
		//	card_info.i_itemNum = value;
		//}
	}

	public int i_manaCost
	{
		get
		{
			return card_info.i_cost;
		}

		set
		{
			card_info.i_cost = value;
		}
	}

	public string st_explain
	{
		get
		{
			return card_info.st_explainCard;
		}

		//set
		//{
		//	card_info.st_explainCard = value;
		//}
	}

	public bool b_isExile
	{
		get
		{
			return card_info.b_isExile;
		}

		set
		{
			card_info.b_isExile = value;
		}
	}

	public int i_damage
	{
		get
		{
			return card_info.i_attack;
		}

		//set
		//{
		//	card_info.i_attack = value;
		//}
	}

	public int i_upgraded
	{
		get
		{
			return card_info.i_upgraded;
		}

		set
		{
			card_info.i_upgraded = value;
		}
	}

	#endregion

	private void Awake()
	{
		Setup();
	}

	protected virtual void Start()
    {

    }

    private void Update() // 실시간으로 레벨 변화시켜서 테스트하기 위해 임시로 다시 추가
    {
		ExplainRefresh();
		ManaCostRefresh();
		NameRefresh();
	}

	//   public void SetItemSO(Card_Info _card_info) <<22-11-04 장형용 :: 이거때매 원본까지 변경되서 잠시 제거함>>
	//{
	//	card_info = _card_info;
	//}

	public void Setup()
	{
		enemyDamagedEffectSprite = card_info.enemyDamageSprite;
		playerAttackEffectSprite = card_info.playerAttackSprite;

		ExplainRefresh();
		ManaCostRefresh(); // <<22-10-21 장형용 :: 함수로 변경>>
		NameRefresh(); // <<22-11-04 장형용 :: 함수로 변경>>
	}

    #region 카드 UI 갱신

    public virtual void ExplainRefresh()
	{
		sb.Clear();
		if (b_isExile)
		{
			sb.Append("망각\n");
		}
		sb.Append(st_explain);

		#region 각 변수 별 텍스트 갱신 처리

		sb.Replace("{0}", i_damage.ToString());                       // 0번: 고정 수치, 색은 변하지 않음

		sb.Replace("{1}", "<color=#ff0000>{1}</color>");              // 1번: 강화 수치와 마나 친화성이 적용되는 수치
		sb.Replace("{1}", ApplyManaAffinity(i_damage).ToString());

		sb.Replace("{2}", "<color=#0000ff>{2}</color>");              // 2번: 강화 수치와 마나 저항력이 적용되는 수치
		sb.Replace("{2}", ApplyMagicResistance(i_damage).ToString());

																	  // 3번: 카드 별 별도로 사용하는 수치, 이 경우는 override해서 다룸

																	  // 4번: 강화 수치가 적용되는 카드 별 별도로 사용하는 수치, 이 경우도 override해서 다룸

		sb.Replace("{5}", "<color=#ff00ff>{5}</color>");              // 5번: 강화수치만 적용되는 수치
		sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());

		sb.Replace("망각", "<color=#ff00ff>망각</color>");

		#endregion

		explainTMP.text = sb.ToString();
	}

	public virtual void ManaCostRefresh() // <<22-10-21 장형용 :: 추가>>
	{
		manaCostTMP.text = i_manaCost.ToString();
	}

	public void NameRefresh() // <<22-11-04 장형용 :: 추가>>
	{
		sb.Clear();
		sb.Append(card_info.st_cardName);

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

		nameTMP.text = sb.ToString();
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
		.AppendCallback(() => { this.gameObject.SetActive(false); });
    }

    #endregion

    // <<22-10-21 장형용 :: 추가>>
    #region 수치 계산 (원본값)

    public int ApplyManaAffinity(int _value)
	{
		int value = _value
			+ PlayerEntity.Inst.Status_MagicAffinity_Permanent
			+ PlayerEntity.Inst.Status_MagicAffinity_Battle
			+ PlayerEntity.Inst.Status_MagicAffinity_Turn;

		return ApplyEnhanceValue(value);
	}

	public int ApplyMagicResistance(int _value)
	{
		return ApplyEnhanceValue(_value + PlayerEntity.Inst.Status_MagicResistance);
	}

	public int ApplyEnhanceValue(int _value)
	{
		return _value * PlayerEntity.Inst.Status_EnchaneValue;
	}

	#endregion

	#region 수치 계산 (인스턴스)

	protected int ApplyManaAffinity_Instance(int _value)
	{
		int total = _value
			+ i_magicAffinity_permanent_inst
			+ i_magicAffinity_battle_inst
			+ i_magicAffinity_turn_inst;

		return ApplyEnhanceValue_Instance(total);

	}

	protected int ApplyEnhanceValue_Instance(int _value)
	{
		return _value *= i_enhenceValue_inst;
	}

	protected int ApplyMagicResistance_Instance(int _value)
	{
		int total = _value += T_magicResistance_inst;

		return _value *= i_enhenceValue_inst;
	}

	#endregion

	#region 카드 사용

	// <<22-10-28 장형용 :: 수정>>
	public virtual IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		PlayerEntity.Inst.Status_Aether -= i_manaCost;

		SetStatusInstance();

		CardManager.i_usingCardCount++;

		Utility.onCardUsed?.Invoke(this);

		yield return null;
	}

	protected void SetStatusInstance()
	{
		i_enhenceValue_inst = PlayerEntity.Inst.Status_EnchaneValue;

		i_magicAffinity_turn_inst = PlayerEntity.Inst.Status_MagicAffinity_Turn;
		i_magicAffinity_battle_inst = PlayerEntity.Inst.Status_MagicAffinity_Battle;
		i_magicAffinity_permanent_inst = PlayerEntity.Inst.Status_MagicAffinity_Permanent;

		T_magicResistance_inst = PlayerEntity.Inst.Status_MagicResistance;
	}

	protected IEnumerator Repeat(Action myMethodName, int _count) // <<22-10-26 장형용 :: 횟수만큼 메소드를 반복, 논리 상으론 문제가 없는데 비용이 비싼 듯>>
	{
		for (int i = 0; i < _count; i++)
		{
			myMethodName(); // 아니 카드 루프 돌다 퇴근하는데?
			yield return new WaitForSeconds( FindMin( 1.0f / (float)_count)); // DoTween 대기시간 인지하면서 작업할 것
		}

		yield return null;
	}

	float FindMin(float _time)
    {
		if(_time > 0.15f)
        {
			return 0.15f;

		}
        else
        {
			return _time;

		}
    }

	protected void TargetAll(Action myMethodName, ref Entity _target) // <<22-10-26 장형용 :: 적 전체에게 메소드 반복>>
	{
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		{
			_target = EntityManager.Inst.enemyEntities[i];

            if (!_target.is_die)
            {
				myMethodName();
			}
        }
	}

	protected IEnumerator EndUsingCard()
	{
		CardManager.i_usingCardCount--;

		yield return null;
	}

    #region 카드 모듈

    #region 공격

    protected void Attack(Entity _target, int _value)
	{
		if(!_target.is_die)
        {
			_target?.Damaged(_value, this);

			StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSprite));
			StartCoroutine(_target?.DamagedEffectCorutin(enemyDamagedEffectSprite));
		}
    }

	protected void Attack(PlayerEntity _target, int _value)
	{
		_target?.Damaged(_value, this);

		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSprite));
		_target.SetDamagedSprite(enemyDamagedEffectSprite);
	}

	protected void Attack_RandomEnemy(Entity _target, int _value)
	{
		_target = EntityManager.Inst.SelectRandomTarget();

		if (_target != null)
		{
			Attack(_target, _value);
		}
	}

	#endregion

	#region 화상

	protected void Add_Burning(Entity _target, int _value)
	{
		_target.i_burning += ApplyEnhanceValue_Instance(_value);
	}

	protected void Add_Burning(PlayerEntity _target, int _value)
	{
		_target.i_burning += ApplyEnhanceValue_Instance(_value);
	}

	#endregion

	#region 마나 친화성

	protected void Add_MagicAffinity_Turn(int _value)
	{
		PlayerEntity.Inst.Status_MagicAffinity_Turn += ApplyEnhanceValue_Instance(_value);

		BattleCalculater.Inst.RefreshMyHands();
	}

	protected void Add_MagicAffinity_Battle(int _value)
	{
		PlayerEntity.Inst.Status_MagicAffinity_Battle += ApplyEnhanceValue_Instance(_value);

		BattleCalculater.Inst.RefreshMyHands();
	}

	#endregion

	protected void Shield(int _value)
	{
		PlayerEntity.Inst.Status_Shiled += _value;
	}

	protected void EnhanceValue()
	{
		PlayerEntity.Inst.Status_EnchaneValue *= i_damage;

		BattleCalculater.Inst.RefreshMyHands();
	}

	protected void RestoreAether(int _value)
	{
		PlayerEntity.Inst.Status_Aether += _value;
	}

	protected void RestoreHealth(int _value)
	{
		PlayerEntity.Inst.Status_Health += _value;
	}

	protected void AddCardAndCostDescrease()
	{
		if (CardManager.Inst.myDeck.Count > 0)
		{
			CardManager.Inst.AddCard();

			CardManager.Inst.myCards.Last().i_manaCost = 0;
			CardManager.Inst.myCards.Last().b_isExile = true;
			CardManager.Inst.myCards.Last().card_info.b_isExile = true;

			CardManager.Inst.myCards.Last().ManaCostRefresh();
			CardManager.Inst.myCards.Last().ExplainRefresh();
		}
	}

    #endregion

    #endregion

    #region 이벤트트리거

    private void OnMouseOver()
    {
		if (is_Useable_Card)
		{
			CardManager.Inst.CardMouseOver(this);
		}
    }

    private void OnMouseExit()
	{
		if (is_Useable_Card)
		{
			CardManager.Inst.CardMouseExit(this);
		}
    }

	private void OnMouseDown()
	{
		if (is_Useable_Card)
		{
			CardManager.Inst.CardMouseDown();
		}
	}

	private void OnMouseUp()
	{
		if (is_Useable_Card)
		{
			CardManager.Inst.CardMouseUp(this);
		}
	}

	#endregion
}