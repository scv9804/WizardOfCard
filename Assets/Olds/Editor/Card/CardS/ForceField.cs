using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Card, IProtection
{
    [Header("ī�� �߰� ���� ������")]
    [Tooltip("ī�� ��ȣ"), SerializeField] int[] protection = new int[3];

    public int Protection
    {
        get { return ApplyEnhanceValue(protection[i_upgraded]); }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
        sb.Replace("{0}", Protection.ToString());

        return sb.ToString();
    }

    // <<22-10-28 ������ :: ����>>
    // <<22-11-24 ������ :: ����>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        GainProtection();

        yield return StartCoroutine(EndUsingCard());
    }

    public void GainProtection()
    {
        Player.Buff_Protection += Protection;
    }
}
