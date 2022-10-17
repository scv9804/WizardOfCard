using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : Card
{
    public override void UseCard(int index)
    {
        base.UseCard(index);

        if (index < EntityManager.Inst.enemyEntities.Count)
        {
            Debug.Log("적에게 사용");

            targetEntity?.Damaged(CulcEnchaneValue());
            SpellEnchaneReset();
            targetEntity.RefreshEntity();

            StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
            StartCoroutine(targetEntity.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));
        }
        else
        {
            Debug.Log("나에게 사용");

            EntityManager.Inst.playerEntity?.Damaged(i_damage);
            SpellEnchaneReset();
            EntityManager.Inst.playerEntity.RefreshPlayer();
        }

        Debug.Log("마법구 사용 함수");

        targetEntity = null;
    }
}
