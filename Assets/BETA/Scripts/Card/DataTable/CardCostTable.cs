using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    namespace Serialized
    {
        namespace Card
        {
            // ==================================================================================================== CardCostTable

            [CreateAssetMenu(menuName = "Card/Data/CostTable", fileName = "CostTable")]
            public sealed class CardCostTable : SerializedListObject<int[], CardCostData>
            {

            }

            // ==================================================================================================== CardCostData

            [Serializable] public sealed class CardCostData : SerializedListData<int[]>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("비용")]
                [SerializeField] private int[] _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

                protected override int[] Value
                {
                    get
                    {
                        return _value;
                    }
                }
            }  
        }
    }
}
