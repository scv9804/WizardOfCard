using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBreath : Card, IRestoreHealth, IRestoreAether
{
    [Header("ī�� �߰� ���� ������")]
    [Tooltip("ī�� ü�� ȸ�� ��ġ"), SerializeField] int[] health = new int[3];
	[Tooltip("ī�� ���� ȸ�� ��ġ"), SerializeField] int[] aether = new int[3];

    public int Health
    {
        get { return ApplyEnhanceValue(health[i_upgraded]); }
    }

    public int Aether
    {
        get { return aether[i_upgraded]; }
    }

    public override string GetCardExplain()
    {
        base.GetCardExplain();

        sb.Replace("{0}", "<color=#ff00ff>{0}</color>");
        sb.Replace("{0}", Health.ToString());

        sb.Replace("{1}", Aether.ToString());

        return sb.ToString();
    }

    // <<22-10-28 ������ :: ����>>
    // <<22-11-24 ������ :: ����>>
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		RestoreHealth(); 
        RestoreAether();

        #region EndUsingCard

        CardManager.i_usingCardCount--;

        RefreshMyHandsExplain();

        yield return null;

        #endregion
    }

    public void RestoreHealth()
    {
        Player.Status_Health += Health;
    }

    public void RestoreAether()
    {
        Player.Status_Aether += Aether;
    }
}
