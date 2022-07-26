using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using DG.Tweening;

public class Card : MonoBehaviour
{

	[SerializeField] GameObject card;
	[SerializeField] ItemSO itemSO;
//	[SerializeField] SpriteRenderer charater;
	[SerializeField] SpriteRenderer sp_card;
	[SerializeField] TMP_Text nameTMP;
	[SerializeField] TMP_Text healthTMP;
	[SerializeField] TMP_Text manaCostTMP;
	[SerializeField] TMP_Text explainTMP;
	[SerializeField] Vector3 v_cardSize;

	public Card_Info card_info;

	[HideInInspector] public Pos_Rot_Scale originPRS;
	[HideInInspector] public int i_CardNum;
	[HideInInspector] public int i_manaCost;
	[HideInInspector] public int i_damage;
	[HideInInspector] public int i_cardType;

	[HideInInspector] public int i_explainDamage;
	[HideInInspector] public string st_explain;
	[HideInInspector] public string[] st_splitExplain;
	[HideInInspector] public int i_explainDamageOrigin;

	 public bool is_Useable_Card = true;



	private void FixedUpdate()
	{	
		ExplainRefresh();
	}

	private void Start()
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

		splitString();
		ExplainRefresh();
	
		manaCostTMP.text = i_manaCost.ToString() ;
		nameTMP.text = card_info.st_cardName;
	}

	//OutPut Srting 문자열 
    void splitString()
    {
		string tempt = Regex.Replace(st_explain, @"[^0-9]", "");
		Regex regex = new Regex(tempt);
		i_damage = int.Parse(tempt);
		i_explainDamageOrigin = int.Parse(tempt);
		st_splitExplain = regex.Split(st_explain);
	}


	public void ExplainRefresh()
	{
		if (st_splitExplain[1] !=  null)
		{
			string dumy = st_splitExplain[0] + (i_damage * PlayerEntity.Inst.Status_EnchaneValue) + st_splitExplain[1];
			explainTMP.text = dumy;
		}
		
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
