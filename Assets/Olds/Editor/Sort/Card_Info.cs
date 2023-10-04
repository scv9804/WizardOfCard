using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Info : MonoBehaviour
{
	//<<22-11-04 ������ :: ī�� ���� �� ���� �ý��� �߰�>>
	[Header("ī�� �⺻ ������")]
	[Tooltip("ī�� �̸�")] public string st_cardName;
	[Tooltip("ī�� ��ȣ")] public int i_itemNum;
	//[Tooltip("ī�� �з�(���� ������)")] public CardType type; // <<22-11-04 ������ :: Enum ������ Utility�� ����>>
	[Tooltip("ī�� ��͵�")] public float f_percentage;

	[Header("ī�� ��ȭ Ƚ��")]
	[Range(0, 2)] public int i_upgraded;

	[Header("ī�� ���� ������")]
	[Tooltip("ī�� ���"), SerializeField] int[] cost = new int[3];
	[Tooltip("ī�� ������ �� ȿ�� ��ġ"), SerializeField] int[] attack = new int[3];
	[Tooltip("ī�� ���� ����"), SerializeField] bool[] isExile = new bool[3];
	[Tooltip("ī�� ��� ����"), SerializeField] AttackRange[] AR_attackRange = new AttackRange[3];
	[Tooltip("ī�� ����"), TextArea(3, 5), SerializeField] string[] explainCard = new string[3];

	[Header("����Ʈ ��������Ʈ")]
	[Tooltip("�ǰ� ����Ʈ")] public Sprite enemyDamageSprite;
	[Tooltip("���� ����Ʈ")] public Sprite playerAttackSprite;

	//public Sprite sp_CardSprite; <<22-11-04 ������ :: ����>>

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
