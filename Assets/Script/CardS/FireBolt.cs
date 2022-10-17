using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Card
{
    [SerializeField] int applyBurn = 0;

    public override void ExplainUpdate()
    {
        sb.Clear();

        sb.Append(st_explain);
        sb.Replace("{0}", applyBurn.ToString());
        sb.Replace("{1}", CulcEnchaneValue().ToString());

        explainTMP.text = sb.ToString();
    }

    public override void UseCard(int index)
    {
        base.UseCard(index);

        if (index < EntityManager.Inst.enemyEntities.Count)
        {
            Debug.Log("적에게 사용");

            targetEntity.i_burn += applyBurn;
            targetEntity?.Damaged(CulcEnchaneValue());
            SpellEnchaneReset();
            targetEntity.RefreshEntity();

            StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
            StartCoroutine(targetEntity.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));
        }
        else
        {
            Debug.Log("나에게 사용");

            EntityManager.Inst.playerEntity.i_burn += applyBurn;

            EntityManager.Inst.playerEntity?.Damaged(i_damage);
            SpellEnchaneReset();
            EntityManager.Inst.playerEntity.RefreshPlayer();
        }

        Debug.Log("화염구 사용 함수");

        targetEntity = null;
    }
}
