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

	[Header("ī�� �������̽�")] 
	[SerializeField] TMP_Text nameTMP;
	[SerializeField] TMP_Text manaCostTMP;
	[SerializeField] protected TMP_Text explainTMP;

	[Header("ī�� ����Ʈ ��������Ʈ")]
	[Tooltip("�ǰ� ����Ʈ")] public Sprite enemyDamageSprite;
	[Tooltip("���� ����Ʈ")] public Sprite playerAttackSprite;

	[Header("ī�� �⺻ ������")]
	[Tooltip("ī�� �̸�")] public string st_cardName;
	[Tooltip("ī�� ��ȣ")] public int i_itemNum;
	[Tooltip("ī�� �з�(���� ������)")] public CardType type;
	[Tooltip("ī�� ��͵�")] public float f_percentage;

	[Header("ī�� ��ȭ Ƚ��")]
	[Range(0, 2), SerializeField] int upgraded;

	[Header("ī�� ���� ������")]
	[Tooltip("ī�� ���"), SerializeField] int[] cost = new int[3];
	[Tooltip("ī�� ������ �� ȿ�� ��ġ"), SerializeField] int[] attack = new int[3];
	[Tooltip("ī�� ���� ����"), SerializeField] bool[] isExile = new bool[3];
	[Tooltip("ī�� ��� ����"), SerializeField] AttackRange[] AR_attackRange = new AttackRange[3];
	[Tooltip("ī�� ����"), TextArea(3, 5)]public string[] explainCard = new string[3]; // ��� �ʿ��ؼ� �ۺ����� �ٲ� �޼����߰��ұ� �ߴµ� �ϴ� �״�ε�

	//SpriteRenderer sp_card; // ���� �̻��

	[HideInInspector] public Pos_Rot_Scale originPRS;

	[HideInInspector] public bool is_Useable_Card = true;

	protected StringBuilder sb = new StringBuilder();

    //[HideInInspector] public int bonus; // ���� �̻��, ������ ���� ����

    // <<22-10-27 ������ :: �߰�>>
    protected int i_enhanceValue_inst = 0;
    // <<22-11-09 ������ :: ����>>
    //protected int i_magicAffinity_turn_inst = 0;
    //protected int i_magicAffinity_battle_inst = 0;
    //protected int i_magicAffinity_permanent_inst = 0;
    //protected int T_magicResistance_inst = 0;

    #region Job System

    MagicAffinityJob myMagicAffinityJob = new MagicAffinityJob();
    MagicResistanceJob myMagicResistanceJob = new MagicResistanceJob();
    EnhanceValueJob myEnhanceValueJob = new EnhanceValueJob();

	#endregion

	// <<22-11-12 ������ :: ���� ���� ������Ʈ�� �÷ȴ��� ���� ������ �ӽ� �߰�>>
	bool isUsing = false;

	// <<22-11-04 ������ :: ���� card_info�� �ִ� ���� �̸��� �����ϴ� ��� ������Ƽ�� ȣȯ�ǰ� �ص�>>
	// ���� �����ϸ� �ϳ��ϳ� �ٲ��ִ°� �±� �ѵ� �ʹ� ������... Ư�� i_damage...
	#region Properties

	public int i_upgraded
    {
		get
		{
			return upgraded;
		}

		set
		{
			upgraded = value;
		}
	}

	public AttackRange attackRange
	{
		get
		{
			return AR_attackRange[i_upgraded];
		}

		set
		{
			AR_attackRange[i_upgraded] = value;
		}
	}

	public int i_manaCost
	{
		get
		{
			return cost[i_upgraded];
		}

		set
		{
			cost[i_upgraded] = value;
		}
	}

	protected string st_explain
	{
		get
		{
			return explainCard[i_upgraded];
		}

		//set
		//{
		//	explainCard[i_upgraded] = value;
		//}
	}

	public bool b_isExile
	{
		get
		{
			return isExile[i_upgraded];
		}

		set
		{
			isExile[i_upgraded] = value;
		}
	}

	protected int i_damage
	{
		get
		{
			return attack[i_upgraded];
		}

		// �����
		set
		{
            attack[i_upgraded] = value;
        }
    }

    #endregion

    protected virtual void Awake()
	{
		//sp_card = GetComponent<SpriteRenderer>(); // �̰� ��ũ��Ʈ �󿡼� �� �� �ٲ�µ�?

		Setup();
		RefreshCardUI();
	}

	protected virtual void Start() { }

	protected virtual void OnDisable() { }

	protected void Update() // �ǽð����� ���� ��ȭ���Ѽ� �׽�Ʈ�ϱ� ���� �ӽ÷� �ٽ� �߰�
	{
		// <<22-11-12 ������ :: ���� ���� ������Ʈ�� �÷ȴ��� ���� ������ �ӽ� �߰�>>
		if (!isUsing)
        {
			RefreshCardUI();
		}
	}

	// <<22-11-09 ������ :: �߰�>>
	public void Setup()
	{
		i_manaCost = itemSO.items[i_itemNum].card.i_manaCost;
		b_isExile = itemSO.items[i_itemNum].card.b_isExile;
		AR_attackRange = itemSO.items[i_itemNum].card.AR_attackRange;

		i_enhanceValue_inst = 0;
	}

    #region ī�� UI ����

	// <<22-11-09 ������ :: �߰�>>
	public void RefreshCardUI()
	{
		manaCostTMP.text = i_manaCost.ToString();

		ExplainRefresh();
		NameRefresh();
	}

    public virtual void ExplainRefresh()
	{
		i_enhanceValue_inst = PlayerEntity.Inst.Buff_EnchaneValue;

		sb.Clear();
		if (b_isExile)
		{
			sb.Append("����\n");
		}
		sb.Append(st_explain);

		#region �� ���� �� �ؽ�Ʈ ���� ó��

		sb.Replace("{0}", i_damage.ToString());                       // 0��: ���� ��ġ, ���� ������ ����

		sb.Replace("{1}", "<color=#ff0000>{1}</color>");              // 1��: ��ȭ ��ġ�� ���� ģȭ���� ����Ǵ� ��ġ
		sb.Replace("{1}", ApplyMagicAffinity(i_damage).ToString());

		sb.Replace("{2}", "<color=#0000ff>{2}</color>");              // 2��: ��ȭ ��ġ�� ���� ���׷��� ����Ǵ� ��ġ
		sb.Replace("{2}", ApplyMagicResistance(i_damage).ToString());

																	  // 3��: ī�� �� ������ ����ϴ� ��ġ, �� ���� override�ؼ� �ٷ�

																	  // 4��: ��ȭ ��ġ�� ����Ǵ� ī�� �� ������ ����ϴ� ��ġ, �� ��쵵 override�ؼ� �ٷ�

		sb.Replace("{5}", "<color=#ff00ff>{5}</color>");              // 5��: ��ȭ��ġ�� ����Ǵ� ��ġ
		sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());

		sb.Replace("����", "<color=#ff00ff>����</color>");
		sb.Replace("��ȣ", "<color=#0000ff>��ȣ</color>");
		sb.Replace("��ο� �Ұ�", "<color=#ff0000>��ο� �Ұ�</color>");

		#endregion

		explainTMP.text = sb.ToString();
	}

	// <<22-11-04 ������ :: �߰�>>
	public void NameRefresh()
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
		myMagicAffinityJob.magicAffinity = PlayerEntity.Inst.Buff_MagicAffinity;

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
		myMagicResistanceJob.magicResistance = PlayerEntity.Inst.Buff_MagicResistance;

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

	// <<22-11-09 ������ :: ����>>
	//protected int ApplyManaAffinity_Instance(int _value)
	//protected int ApplyEnhanceValue_Instance(int _value)
	//protected int ApplyMagicResistance_Instance(int _value)

	#endregion

	#region ī�� ���

	// <<22-10-28 ������ :: ����>>
	public virtual IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		PlayerEntity.Inst.Status_Aether -= i_manaCost;

		CardManager.i_usingCardCount++;

		Utility.onCardUsed?.Invoke(this);

		i_enhanceValue_inst = PlayerEntity.Inst.Buff_EnchaneValue;

		// <<22-11-12 ������ :: ���� ���� ������Ʈ�� �÷ȴ��� ���� ������ �ӽ� �߰�>>
		isUsing = true;

		yield return null;
	}

	// <<22-11-09 ������ :: ����>>
	//protected void SetStatusInstance()

	// <<22-10-26 ������ :: �߰�>>
	protected IEnumerator Repeat(Action myMethodName, int _count) // Ƚ����ŭ �޼ҵ带 �ݺ�
	{
		for (int i = 0; i < _count; i++)
		{
			myMethodName(); // �ƴ� ī�� ���� ���� ����ϴµ�?
			yield return new WaitForSeconds( FindMin( 1.0f / (float)_count)); // DoTween ���ð�(1.05f) �����ϸ鼭 �۾��� ��
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

	// <<22-10-26 ������ :: �߰�>>
	protected void TargetAll(Action myMethodName, ref Entity _target) // �� ��ü���� �޼ҵ� �ݺ�
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

    #region ī�� ȿ��

    #region ����

    protected void Attack(Entity _target, int _value)
	{
		if(!_target.is_die)
        {
			_target?.Damaged(_value, enemyDamageSprite, this);

			StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackSprite));
			MusicManager.inst.PlayerDefultSoundEffect();
			//StartCoroutine(_target?.DamagedEffectCorutin(enemyDamageSprite));
		}
    }

	protected void Attack(PlayerEntity _target, int _value)
	{
		_target?.Damaged(_value, this);

		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackSprite));
		_target.SetDamagedSprite(enemyDamageSprite);
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
		_target.i_burning += _value;
	}

	protected void Add_Burning(PlayerEntity _target, int _value)
	{
		_target.Debuff_Burning += _value;
	}

	#endregion

	#region ���� ģȭ��

	protected void Add_MagicAffinity_Turn(int _value)
	{
		PlayerEntity.Inst.Buff_MagicAffinity_Turn += _value;

		CardManager.Inst.RefreshMyHands();
	}

	protected void Add_MagicAffinity_Battle(int _value)
	{
		PlayerEntity.Inst.Buff_MagicAffinity_Battle += _value;

		CardManager.Inst.RefreshMyHands();
	}

	#endregion

	protected void Shield(int _value)
	{
		PlayerEntity.Inst.Status_Shiled += _value;
	}

	protected void EnhanceValue()
	{
		PlayerEntity.Inst.Buff_EnchaneValue *= i_damage;

		CardManager.Inst.RefreshMyHands();
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
		}
	}

	protected void Protection(int _value)
    {
		PlayerEntity.Inst.Buff_Protection += _value;
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