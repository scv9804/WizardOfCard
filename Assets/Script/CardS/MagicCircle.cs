using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : Card, IEnhance
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ȿ�� ��ȭ ��ġ"), SerializeField] int[] enhance = new int[3];

    public int EnhanceValue
	{
        get { return ApplyEnhanceValue(enhance[i_upgraded]); }
    }

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
		sb.Replace("{0}", EnhanceValue.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		Enahnce();

		yield return StartCoroutine(EndUsingCard());
	}

	public void Enahnce()
    {
		Player.Buff_EnchaneValue = EnhanceValue;
	}
}
