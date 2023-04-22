using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardAttackNodeData

    [CreateAssetMenu(menuName = "WIP/Card/Behaviour/Attack", fileName = "_AttackBehaviour")]
    public class CardAttackNodeData : CardBehaviourNodeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Behaviour

        // ================================================== Base

        public override void Create()
        {
            throw new System.NotImplementedException();
        }
    }

    // ==================================================================================================== CardAttackNode

    public class CardAttackNode : CardBehaviourNode
    {
        // ==================================================================================================== Method

        // =========================================================================== Behaviour

        // ================================================== Base

        public override IEnumerator Invoke(CardHandler handler, int current)
        {
            throw new System.NotImplementedException();
        }

        // ================================================== Attack

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public int ApplyPower(int damage, CardDamageApplyOption option)
        {
            return damage;
        }

        // =========================================================================== Data

        // ================================================== Base

        public override string AssemblyDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}
