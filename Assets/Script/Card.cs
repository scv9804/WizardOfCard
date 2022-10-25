using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;
using System.Text;
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

	private void Awake()
	{
		Setup();
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

		i_damage = card_info.i_attack; // <<22-10-21 ������ :: �߰�>>
		this.b_isExile = card_info.b_isExile;

		ExplainRefresh();

		ManaCostRefresh(); // <<22-10-21 ������ :: �Լ��� ����>>
		nameTMP.text = card_info.st_cardName;
	}

	public virtual void ExplainRefresh()
	{
		sb.Clear();
		if(b_isExile)
        {
			sb.Append("<color=#ff00ff>����</color>\n");
		}
		sb.Append(st_explain);


		sb.Replace("{0}", i_damage.ToString());                       // 0��: ���� ��ġ
		sb.Replace("{1}", ApplyManaAffinity(i_damage).ToString());    // 1��: ��ȭ ��ġ�� ���� ģȭ���� ����Ǵ� ��ġ
		sb.Replace("{2}", ApplyMagicResistance(i_damage).ToString()); // 2��: ��ȭ ��ġ�� ���� ���׷��� ����Ǵ� ��ġ
																	  // 3��: ī�� �� ������ ����ϴ� ��ġ, �� ���� override�ؼ� �ٷ�
																	  // 4��: ��ȭ ��ġ�� ����Ǵ� ī�� �� ������ ����ϴ� ��ġ, �� ���� override�ؼ� �ٷ�
		sb.Replace("{5}", ApplyEnhanceValue(i_damage).ToString());    // 5��: ��ȭ��ġ�� ����Ǵ� ��ġ

		explainTMP.text = sb.ToString();
	}

	public virtual void ManaCostRefresh() // <<22-10-21 ������ :: �߰�>>
	{
		manaCostTMP.text = i_manaCost.ToString();
	}

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
		.Append(transform.DORotate(new Vector3(0, 0, -120), 0.3f).SetEase(Ease.OutCirc))
		.Append(transform.DOPath(_prs , 0.4f , PathType.CubicBezier, PathMode.Sidescroller2D, 5).SetLookAt(new Vector3(0,0,-120), new Vector3(0, 0 ,-120)).SetEase(Ease.InQuad))
		.AppendCallback(() => { this.gameObject.SetActive(false); });
    }

	public virtual void UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		PlayerEntity.Inst.Status_Aether -= i_manaCost;
	}

	// <<22-10-21 ������ :: �߰�>>
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

	#region ī�� �ɷ� ���

	// <<22-10-21 ������ :: �߰�>>
	public void Attack_Defalut(Entity _target, int _value) // �� ����
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		StartCoroutine(_target.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));

		_target?.Damaged(ApplyManaAffinity(_value));
	}

	public void Attack_PlayerSelf(PlayerEntity _target, int _value) // ����
	{
		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
		_target.SetDamagedSprite(enemyDamagedEffectSpriteRenderer.sprite);

		_target?.Damaged(ApplyManaAffinity(_value));
	}

	public void RepeatAttack_AllEnemy(int _count, int _value)
    {
		for (int i = 0; i < EntityManager.Inst.enemyEntities.Count * _count; i++)
		{
			Attack_Defalut(EntityManager.Inst.enemyEntities[i % EntityManager.Inst.enemyEntities.Count], _value);	
		}
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