using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardAttackNode

    // ���� ���ٰ� �����ϰ� ��ũ��Ʈ ©�Կ� ����;;; ����;;;
    [CreateAssetMenu(menuName = "WIP/Card/Action/Attack", fileName = "_AttackNode")]
    public class CardAttackNode : CardActionNode
    {
        // ==================================================================================================== Field

        // =========================================================================== Action

        // ================================================== Attack

        [Header("������")]
        [SerializeField] private int[] _damage = new int[Card.MAX_UPGRADE_LEVEL + 1];

        // =========================================================================== Effect

        [Header("���� ����Ʈ ��������Ʈ")]
        [SerializeField] private Sprite _playerAttackSprite;

        [Header("�ǰ� ����Ʈ ��������Ʈ")]
        [SerializeField] private Sprite _enemyDamagedSprite;

        // ==================================================================================================== Property

        // =========================================================================== Action

        // ================================================== Attack

        public int[] Damage
        {
            get
            {
                return _damage;
            }
        }

        // =========================================================================== Effect

        public Sprite PlayerAttackSprite
        {
            get
            {
                return _playerAttackSprite;
            }
        }

        public Sprite EnemyDamagedSprite
        {
            get
            {
                return _enemyDamagedSprite;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Status

        public override string GetDescription(string description, int upgraded)
        {
            StringBuilder.Clear();
            StringBuilder.Append(description);

            EntityAttackCommand command = CreateCommand(upgraded);

            StringBuilder.Replace(Format, $"<color=#ff4444>{Format}</color>");
            StringBuilder.Replace(Format, command.GetDamage().ToString());

            return StringBuilder.ToString();
        }

        // =========================================================================== Effect

        // ================================================== Base

        public override void Execute(Card card, CardHandler handler)
        {
            EntityAttackCommand command = CreateCommand(card.Upgraded.Value);

            handler.Commands.Add(command);
        }

        // ================================================== Attack

        private EntityAttackCommand CreateCommand(int upgraded)
        {
            EntityAttackCommand command = new EntityAttackCommand();

            command.Damage = Damage[upgraded];

            command.PlayerAttackSprite = PlayerAttackSprite;
            command.EnemyDamagedSprite = EnemyDamagedSprite;

            // apply buff effects

            return command;
        }
    } 
}
