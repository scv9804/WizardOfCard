using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHasteBarrier : Card, IShield
{
    [Header("ī�� �߰� ���� ������")]
    [Tooltip("ī�� ����"), SerializeField] int[] shield = new int[3];
    [Tooltip("ī�� ��ο� Ƚ��"), SerializeField] int[] drawCount = new int[3];

    public int Shield
    {
        get { return ApplyMagicResistance(shield[i_upgraded]); }
    }

    int DrawCount
    {
        get { return drawCount[i_upgraded]; }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", "<color=#0000ff>{0}</color>");
        sb.Replace("{0}", Shield.ToString());

        sb.Replace("{1}", DrawCount.ToString());

        return sb.ToString();
    }

    // <<22-10-28 ������ :: ����>>
    // <<22-11-24 ������ :: ����>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        GainShield();

        for (int i = 0; i < DrawCount; i++)
        {
            CardManager.Inst.AddCard();

            yield return new WaitForSeconds(0.15f);
        }

        #region EndUsingCard

        CardManager.i_usingCardCount--;

        RefreshMyHandsExplain();

        yield return null;

        #endregion
    }

    public void GainShield()
    {
        Player.Status_Shiled += Shield;
    }
}
