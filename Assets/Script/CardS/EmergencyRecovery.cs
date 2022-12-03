using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyRecovery : Card, IRestoreHealth
{
    [Header("카드 추가 가변 데이터")]
    [Tooltip("카드 체력 회복 수치"), SerializeField] int[] health = new int[3];

    public int Health
    {
        get { return ApplyHealValue(health[i_upgraded]); }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
        sb.Replace("{0}", Health.ToString());

        return sb.ToString();
    }

    // <<22-10-28 장형용 :: 수정>>
    // <<22-11-24 장형용 :: 수정>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        RestoreHealth();

        if(i_upgraded == 2)
            CardManager.Inst.AddCard();

        yield return StartCoroutine(EndUsingCard());
    }

    public void RestoreHealth()
    {
        Player.Status_Health += Health;
    }
}
