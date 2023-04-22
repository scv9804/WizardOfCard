using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WIP
{
    // ==================================================================================================== CardBlackBoardNodeData

    public abstract class CardBlackBoardNodeData : ScriptableObject
    {
        // ==================================================================================================== Method

        // =========================================================================== BlackBoard

        // ================================================== Base

        public abstract void Create();
    }

    // ==================================================================================================== CardBlackBoardNode

    public class CardBlackBoardNode
    {
        // ==================================================================================================== Field

        // =========================================================================== BlackBoard

        // ================================================== Base

        public CardBehaviourProcess Result = CardBehaviourProcess.None;

        public int InvokeCount = 0;
    }

    // ==================================================================================================== CardBehaviourProcess

    public enum CardBehaviourProcess
    {
        None,

        Running,

        Success,

        Failed
    }
}
