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
	//<<22-11-04 ������ :: ī�� ���� �� ���� �ý��� �߰�>>
	[Header("ī�� ������")]
	[Tooltip("ī�� ���� �����ͺ��̽�"), SerializeField] ItemSO itemSO;
	[Tooltip("ī�� ������")] public Card_Info card_info;

	[Header("ī�� ���� �������̽�")] 
	[SerializeField] TMP_Text nameTMP;
	[SerializeField] TMP_Text manaCostTMP;
	[SerializeField] protected TMP_Text explainTMP;

	/*[SerializeField] */SpriteRenderer sp_card;

	[HideInInspector] public Sprite enemyDamagedEffectSprite;
	[HideInInspector] public Sprite playerAttackEffectSprite;

	[HideInInspector] public Pos_Rot_Scale originPRS;

	[HideInInspector] public bool is_Useable_Card = true;

	protected StringBuilder sb = new StringBuilder();

	[HideInInspector] public int bonus; // ����� �Ⱦ�, ������ ���� ����

    // <<22-10-27 ������ :: �߰�, �̰� ��¥ �³�>>
    #region ��ȭ ��ġ �ν��Ͻ� ��

    protected int i_enhenceValue_inst = 0;
	protected int i_magicAffinity_turn_inst = 0;
	protected int i_magicAffinity_battle_inst = 0;
	protected int i_magicAffinity_permanent_inst = 0;
	protected int T_magicResistance_inst = 0;

    #endregion

    //<<22-11-04 ������ :: �߰�>>
    //card_info�� Ǯ��� ���ٴ� ���� ���� ����ϴ��� ���ü� ì�� (��� ���� ���� ������ �������� ��?��...)
    //�ƹ�ư card_info�� �ִ� ������ ���� card_info �� ���̰� ����ó�� �� �� �ֵ��� ����
    #region ������Ƽ

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

    private void Update() // �ǽð����� ���� ��ȭ���Ѽ� �׽�Ʈ�ϱ� ���� �ӽ÷� �ٽ� �߰�
    {
		ExplainRefresh();
		ManaCostRefresh();
		NameRefresh();
	}

	//   public void SetItemSO(Card_Info _card_info) <<22-11-04 ������ :: �̰Ŷ��� �������� ����Ǽ� ��� ������>>
	//{
	//	card_info = _card_info;
	//}

	public void Setup()
	{
		enemyDamagedEffectSprite = card_info.enemyDamageSprite;
		playerAttackEffectSprite = card_info.playerAttackSprite;

		ExplainRefresh();
		ManaCostRefresh(); // <<22-10-21 ������ :: �Լ��� ����>>
		NameRefresh(); // <<22-11-04 ������ :: �Լ��� ����>>
	}

    #region ī�� UI ����

    public virtual void ExplainRefresh()
	{
		sb.Clear();
		if (b_isExile)
		{
			sb.Append("����\n");
		}
		sb.Append(st_explain);

		#region �� ���� �� �ؽ�Ʈ ���� ó��

		sb.Replace("{0}", i_damage.ToString());                       // 0��: ���� ��ġ, ���� ������ ����

		sb.Replace("{1}", "<color=#ff0000>{1}</color>");              // 1��: ��ȭ ��ġ�� ���� ģȭ���� ����Ǵ� ��ġ
		sb.Replace("{1}", ApplyManaAffinity(i_damage).ToString());

		sb.Replace("{2}", "<color=#0000ff>{2}</color>");              // 2��: ��ȭ ��ġ�� ���� ���׷��� ����Ǵ� ��ġ
		sb.Replace("{2}", ApplyMagicResistance(i_damage).ToString());

																	  // 3��: ī�� �� ������ ����ϴ� ��ġ, �� ���� override�ؼ� �ٷ�

																	  // 4��: ��ȭ ��ġ�� ����Ǵ� ī�� �� ������ ����ϴ� ��ġ, �� ��쵵 override�ؼ� �ٷ�

		sb.Replace("{5}", "<color=#ff00ff>{5}</color>");              // 5��: ��ȭ��ġ�� ����Ǵ� ��ġ
		sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());

		sb.Replace("����", "<color=#ff00ff>����</color>");

		#endregion

		explainTMP.text = sb.ToString();
	}

	public virtual void ManaCostRefresh() // <<22-10-21 ������ :: �߰�>>
	{
		manaCostTMP.text = i_manaCost.ToString();
	}

	public void NameRefresh() // <<22-11-04 ������ :: �߰�>>
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
		.AppendCallback(() => { this.gameObject.SetActive(false); });
    }

    #endregion

    // <<22-10-21 ������ :: �߰�>>
    #region ��ġ ��� (������)

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

	#region ��ġ ��� (�ν��Ͻ�)

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

	#region ī�� ���

	// <<22-10-28 ������ :: ����>>
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

	protected IEnumerator Repeat(Action myMethodName, int _count) // <<22-10-26 ������ :: Ƚ����ŭ �޼ҵ带 �ݺ�, �� ������ ������ ���µ� ����� ��� ��>>
	{
		for (int i = 0; i < _count; i++)
		{
			myMethodName(); // �ƴ� ī�� ���� ���� ����ϴµ�?
			yield return new WaitForSeconds( FindMin( 1.0f / (float)_count)); // DoTween ���ð� �����ϸ鼭 �۾��� ��
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

	protected void TargetAll(Action myMethodName, ref Entity _target) // <<22-10-26 ������ :: �� ��ü���� �޼ҵ� �ݺ�>>
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

    #region ī�� ���

    #region ����

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

	#region ȭ��

	protected void Add_Burning(Entity _target, int _value)
	{
		_target.i_burning += ApplyEnhanceValue_Instance(_value);
	}

	protected void Add_Burning(PlayerEntity _target, int _value)
	{
		_target.i_burning += ApplyEnhanceValue_Instance(_value);
	}

	#endregion

	#region ���� ģȭ��

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

    #region �̺�ƮƮ����

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