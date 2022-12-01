using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Card, IShield
{
    [Header("카드 추가 가변 데이터")]
    [Tooltip("카드 쉴드"), SerializeField] int[] shield = new int[3];

    public int Shield
    {
        get { return ApplyMagicResistance(shield[i_upgraded]); }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", "<color=#0000ff>{0}</color>");
        sb.Replace("{0}", Shield.ToString());

        return sb.ToString();
    }

    // <<22-10-28 장형용 :: 수정>>
    // <<22-11-24 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        GainShield();

        yield return StartCoroutine(EndUsingCard());
    }

    public void GainShield()
    {
        Player.Status_Shiled += Shield;

        MusicManager.inst.PlayBarrierSound();
    }
}
