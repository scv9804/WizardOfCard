using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardAttackNode

    [CreateAssetMenu(menuName = "WIP/Card/Action/Shield", fileName = "_ShieldNode")]
    public class CardShieldNode : CardActionNode
    {
        // ==================================================================================================== Field

        // =========================================================================== Action

        // ================================================== Shield

        [Header("½¯µå")]
        [SerializeField] private int[] _shield = new int[Card.MAX_UPGRADE_LEVEL + 1];

        // ==================================================================================================== Property

        // =========================================================================== Action

        // ================================================== Shield

        public int[] Shield
        {
            get
            {
                return _shield;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Status

        public override string GetDescription(string description, int upgraded)
        {
            StringBuilder.Clear();
            StringBuilder.Append(description);

            EntityShieldCommand command = CreateCommand(upgraded);

            StringBuilder.Replace(Format, $"<color=#4444ff>{Format}</color>");
            StringBuilder.Replace(Format, command.GetShield().ToString());

            return StringBuilder.ToString();
        }

        // =========================================================================== Effect

        // ================================================== Base

        public override void Execute(Card card, CardHandler handler)
        {
            EntityShieldCommand command = CreateCommand(card.Upgraded.Value);

            handler.Commands.Add(command);
        }

        // ================================================== Shield

        private EntityShieldCommand CreateCommand(int upgraded)
        {
            EntityShieldCommand command = new EntityShieldCommand();

            command.Shield = Shield[upgraded];

            // apply buff effects

            return command;
        }
    }
}
