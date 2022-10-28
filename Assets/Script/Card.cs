using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;
using System.Text;
using System;
using System.Linq;

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

	// ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
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

		i_damage = card_info.i_attack; // <<22-10-21 장형용 :: 추가>>
		b_isExile = card_info.b_isExile;

		ExplainRefresh();

		ManaCostRefresh(); // <<22-10-21 장형용 :: 함수로 변경>>
		nameTMP.text = card_info.st_cardName;
	}

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

	public virtual void UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		PlayerEntity.Inst.Status_Aether -= i_manaCost;

		StartCoroutine(T_UseCard(_target_enemy, _target_player));
	}

	// <<22-10-21 장형용 :: 추가>>
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

	#region 카드 능력 모듈

	// <<22-10-21 장형용 :: 추가>>
	protected void Attack_SingleEnemy(Entity _target, int _value) // 적 공격
    {
        StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
        StartCoroutine(_target?.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));

        _target?.Damaged(ApplyManaAffinity(_value));
    }

	protected void Attack_PlayerSelf(PlayerEntity _target, int _value) // 자해
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		_target.SetDamagedSprite(enemyDamagedEffectSpriteRenderer.sprite);

		_target?.Damaged(ApplyManaAffinity(_value));
	}

	protected void Attack_AllEnemy(int _value)
    {
		for(int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
        {
			if(!EntityManager.Inst.enemyEntities[i].is_die)
            {
				Attack_SingleEnemy(EntityManager.Inst.enemyEntities[i], _value);
			}
        }
    }

	protected void Attack_RandomEnemy(int _value)
    {
		Entity _target = EntityManager.Inst.TargetRandomEnemy();

		Attack_SingleEnemy(_target, _value);
	}

	protected IEnumerator Repeat(Action myMethodName, int _count) // <<22-10-26 장형용 :: 메소드를 반복 시켜주는 메소드>>
	{
		for (int i = 0; i < _count; i++)
		{
			myMethodName(); // 아니 카드 루프 돌다 퇴근하는데?
			yield return new WaitForSeconds(0.1f);
		}

		yield return null;
	}

	public virtual IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		//PlayerEntity.Inst.Status_Aether -= i_manaCost;

		T_SetStatusInstance();

		UIManager.i_UsingCardCount++;

		Utility.onCardUsed?.Invoke();

		yield return null;
    }

	protected void T_SetStatusInstance() // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		i_enhenceValue_inst = PlayerEntity.Inst.Status_EnchaneValue;
		i_magicAffinity_turn_inst = PlayerEntity.Inst.Status_MagicAffinity_Turn;
		i_magicAffinity_battle_inst = PlayerEntity.Inst.Status_MagicAffinity_Battle;
		i_magicAffinity_permanent_inst = PlayerEntity.Inst.Status_MagicAffinity_Permanent;
		T_magicResistance_inst = PlayerEntity.Inst.Status_MagicResistance;
	}

	protected void T_Attack(Entity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		StartCoroutine(_target?.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));

		_target?.Damaged(T_ApplyManaAffinity(_value), this);
    }

	protected void T_Attack(PlayerEntity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		_target.SetDamagedSprite(enemyDamagedEffectSpriteRenderer.sprite);

		_target?.Damaged(T_ApplyManaAffinity(_value), this);
	}

	protected void T_Attack_AllEnemy(Entity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		{
			_target = EntityManager.Inst.enemyEntities[i];

			if (!_target.is_die)
            {
				T_Attack(EntityManager.Inst.enemyEntities[i], i_damage);
			}
		}
	}

	protected IEnumerator T_EndUsingCard() // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		UIManager.i_UsingCardCount--;

		yield return null;
	}

	protected void T_Attack_RandomEnemy(Entity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		_target = EntityManager.Inst.TargetRandomEnemy();

		if (_target != null)
		{
			T_Attack(_target, _value);
		}
	}

	protected void T_Shield(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_Shiled += T_ApplyMagicResistance(i_damage);
	}

	protected void T_Apply_Burning(Entity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		_target.i_burning += T_ApplyEnhanceValue(_value);
	}

	protected void T_Apply_Burning(PlayerEntity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		_target.i_burning += T_ApplyEnhanceValue(_value);
	}

	protected void T_Apply_Burning_AllEnemy(Entity _target, int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		{
			_target = EntityManager.Inst.enemyEntities[i];

			T_Apply_Burning(_target, _value);
		}
	}

	protected void T_Apply_MagicAffinity_Turn(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_MagicAffinity_Turn += T_ApplyEnhanceValue(_value);

		CardManager.Inst.RefreshMyHand();
	}

	protected void T_Apply_MagicAffinity_Battle(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_MagicAffinity_Battle += T_ApplyEnhanceValue(_value);

		CardManager.Inst.RefreshMyHand();
	}

	protected void T_EnhanceValue() // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_EnchaneValue *= i_damage;

		CardManager.Inst.RefreshMyHand();
	}

	protected void T_ResotreAether(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_Aether += _value;
	}

	protected void T_ResotreHealth(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		PlayerEntity.Inst.Status_Health += T_ApplyEnhanceValue(_value);
	}

	protected void T_AddCardAndCostDescrease() // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
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



	protected int T_ApplyManaAffinity(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		int total = _value 
			+ i_magicAffinity_permanent_inst						
			+ i_magicAffinity_battle_inst					
			+ i_magicAffinity_turn_inst;

		return T_ApplyEnhanceValue(total);

	}

	protected int T_ApplyEnhanceValue(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		return _value *= i_enhenceValue_inst;
	}

	protected int T_ApplyMagicResistance(int _value) // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		int total = _value += T_magicResistance_inst;

		return _value *= i_enhenceValue_inst;
	}

	#endregion

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

}