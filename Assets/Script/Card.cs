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
	[SerializeField] GameObject card;
	[SerializeField] ItemSO itemSO;
	[SerializeField] SpriteRenderer charater;
	[SerializeField] SpriteRenderer sp_card;
	[SerializeField] TMP_Text nameTMP;
	[SerializeField] TMP_Text manaCostTMP;
	[SerializeField] protected TMP_Text explainTMP;
	[SerializeField] Vector3 v_cardSize;

	public Card_Info card_info;
	public int i_itemCode;
	public int i_damage;

	[HideInInspector] public SpriteRenderer enemyDamagedEffectSpriteRenderer;
	[HideInInspector] public SpriteRenderer playerAttackEffectSpriteRenderer;
	[HideInInspector] public Pos_Rot_Scale originPRS;
	[HideInInspector] public int i_CardNum;
	[HideInInspector] public int i_manaCost;
	[HideInInspector] public int i_cardType;
	[HideInInspector] public int i_explainDamage;
	[HideInInspector] public string st_explain;
	[HideInInspector] public string[] st_splitExplain;
	[HideInInspector] public int i_explainDamageOrigin;
	[HideInInspector] public bool b_isExile = false;

	public Utility_enum.AttackRange attackRange;

	public bool is_Useable_Card = true;

	protected StringBuilder sb = new StringBuilder();

	// <<22-10-27 장형용 :: 추가, 이게 진짜 맞나>>
	protected int i_enhenceValue_inst = 0;
	protected int i_magicAffinity_turn_inst = 0;
	protected int i_magicAffinity_battle_inst = 0;
	protected int i_magicAffinity_permanent_inst = 0;
	protected int T_magicResistance_inst = 0;

	private void Awake()
	{
		Setup();
	}

	protected virtual void Start()
    {

    }

	public void SetItemSO(Card_Info _card_info)
	{
		card_info = _card_info;
	}

	public void Setup()
	{
		i_manaCost = card_info.i_Cost;
		i_CardNum = card_info.i_itemNum;
		i_cardType = ((int)card_info.type);
		st_explain = card_info.st_explainCard;
		sp_card.sprite = card_info.sp_CardSprite;
		card.transform.localScale = v_cardSize;
		attackRange = card_info.attackRange;
		enemyDamagedEffectSpriteRenderer = card_info.enemyDamageSprite;
		playerAttackEffectSpriteRenderer = card_info.playerAttackSprite;

		i_damage = card_info.i_attack;
		b_isExile = card_info.b_isExile;

		SetStatusInstance();
		ExplainRefresh();

		ManaCostRefresh(); // <<22-10-21 장형용 :: 함수로 변경>>
		nameTMP.text = card_info.st_cardName;
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

    #endregion

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
		.Append(transform.DORotate(new Vector3(0, 0, -120), 0.3f).SetEase(Ease.OutCirc))
		.Append(transform.DOPath(_prs , 0.4f , PathType.CubicBezier, PathMode.Sidescroller2D, 5).SetLookAt(new Vector3(0,0,-120), new Vector3(0, 0 ,-120)).SetEase(Ease.InQuad))
		.AppendCallback(() => { this.gameObject.SetActive(false); });
    }

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

	protected IEnumerator Repeat(Action myMethodName, int _count) // <<22-10-26 장형용 :: 메소드를 반복 시켜주는 메소드>>
	{
		for (int i = 0; i < _count; i++)
		{
			myMethodName(); // 아니 카드 루프 돌다 퇴근하는데?
			yield return new WaitForSeconds(0.15f);
		}

		yield return null;
	}

	// <<22-10-28 장형용 :: 수정>>
	public virtual IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		PlayerEntity.Inst.Status_Aether -= i_manaCost;

		SetStatusInstance();

		UIManager.i_UsingCardCount++;

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

	protected IEnumerator EndUsingCard()
	{
		UIManager.i_UsingCardCount--;

		yield return null;
	}

    #region 카드 모듈

    #region 공격

    protected void Attack(Entity _target, int _value)
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));

		_target?.Damaged(ApplyManaAffinity_Instance(_value), this);

		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		StartCoroutine(_target?.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));
    }

	protected void Attack(PlayerEntity _target, int _value)
	{
		_target?.Damaged(ApplyManaAffinity_Instance(_value), this);

		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		_target.SetDamagedSprite(enemyDamagedEffectSpriteRenderer.sprite);
	}

	protected void Attack_AllEnemy(Entity _target, int _value)
	{
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		{
			_target = EntityManager.Inst.enemyEntities[i];

			if (!_target.is_die)
			{
				Attack(EntityManager.Inst.enemyEntities[i], i_damage);
			}
		}
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

	protected void Add_Burning_AllEnemy(Entity _target, int _value)
	{
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		{
			_target = EntityManager.Inst.enemyEntities[i];

			Add_Burning(_target, _value);
		}
	}

	#endregion

	#region 마나 친화성
	protected void Add_MagicAffinity_Turn(int _value)
	{
		PlayerEntity.Inst.Status_MagicAffinity_Turn += ApplyEnhanceValue_Instance(_value);

		CardManager.Inst.RefreshMyHand();
	}

	protected void Add_MagicAffinity_Battle(int _value)
	{
		PlayerEntity.Inst.Status_MagicAffinity_Battle += ApplyEnhanceValue_Instance(_value);

		CardManager.Inst.RefreshMyHand();
	}

	#endregion

	protected void Shield(int _value)
	{
		PlayerEntity.Inst.Status_Shiled += ApplyMagicResistance_Instance(i_damage);
	}

	protected void EnhanceValue()
	{
		PlayerEntity.Inst.Status_EnchaneValue *= i_damage;

		CardManager.Inst.RefreshMyHand();
	}

	protected void RestoreAether(int _value)
	{
		PlayerEntity.Inst.Status_Aether += _value;
	}

	protected void RestoreHealth(int _value)
	{
		PlayerEntity.Inst.Status_Health += ApplyEnhanceValue_Instance(_value);
	}

	protected void AddCardAndCostDescrease()
	{
		if (CardManager.Inst.myDeck.Count > 0)
		{
			CardManager.Inst.AddCard();

			CardManager.Inst.myCards.Last().i_manaCost = 0;
			CardManager.Inst.myCards.Last().b_isExile = true;

			CardManager.Inst.myCards.Last().ManaCostRefresh();
			CardManager.Inst.myCards.Last().ExplainRefresh();
		}
	}

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