using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Info : MonoBehaviour
{
	//<<22-11-04 장형용 :: 카드 정리 겸 레벨 시스템 추가>>
	[Header("카드 기본 데이터")]
	[Tooltip("카드 이름")] public string st_cardName;
	[Tooltip("카드 번호")] public int i_itemNum;
	//[Tooltip("카드 분류(더미 데이터)")] public CardType type; // <<22-11-04 장형용 :: Enum 선언은 Utility로 빼둠>>
	[Tooltip("카드 희귀도")] public float f_percentage;

	[Header("카드 강화 횟수")]
	[Range(0, 2)] public int i_upgraded;

	[Header("카드 가변 데이터")]
	[Tooltip("카드 비용"), SerializeField] int[] cost = new int[3];
	[Tooltip("카드 데미지 및 효과 수치"), SerializeField] int[] attack = new int[3];
	[Tooltip("카드 망각 여부"), SerializeField] bool[] isExile = new bool[3];
	[Tooltip("카드 대상 범위"), SerializeField] AttackRange[] AR_attackRange = new AttackRange[3];
	[Tooltip("카드 설명"), TextArea(3, 5), SerializeField] string[] explainCard = new string[3];

	[Header("이펙트 스프라이트")]
	[Tooltip("피격 이펙트")] public Sprite enemyDamageSprite;
	[Tooltip("공격 이펙트")] public Sprite playerAttackSprite;

	//public Sprite sp_CardSprite; <<22-11-04 장형용 :: 제거>>

	#region Properties

	public int i_cost
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

	public int i_attack
    {
        get
        {
			return attack[i_upgraded];
		}

        //set
        //{
		//	attack[i_upgraded] = value;
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

	public string st_explainCard
	{
		get
		{
			return explainCard[i_upgraded];
		}

        //set
        //{
        //    explainCard[i_upgraded] = value;
        //}
    }

	#endregion
}
