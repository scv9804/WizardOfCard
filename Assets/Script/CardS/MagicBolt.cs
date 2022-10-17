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
            Debug.Log("������ ���");

            targetEntity?.Damaged(CulcEnchaneValue());
            SpellEnchaneReset();
            targetEntity.RefreshEntity();

            StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackEffectSpriteRenderer.sprite));
            StartCoroutine(targetEntity.DamagedEffectCorutin(enemyDamagedEffectSpriteRenderer.sprite));
        }
        else
        {
            Debug.Log("������ ���");

            EntityManager.Inst.playerEntity?.Damaged(i_damage);
            SpellEnchaneReset();
            EntityManager.Inst.playerEntity.RefreshPlayer();
        }

        Debug.Log("������ ��� �Լ�");

        targetEntity = null;
    }
}
