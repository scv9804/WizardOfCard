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
            // ==================================================================================================== CardTypeTable

            [CreateAssetMenu(menuName = "Card/Data/TypeTable", fileName = "TypeTable")]
            public class CardTypeTable : SerializedListObject<BETA.Card.Type, CardCardTypeData>
            {

            }

            // ==================================================================================================== CardCardTypeData

            [Serializable] public sealed class CardCardTypeData : SerializedListData<BETA.Card.Type>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("분류")]
                [SerializeField] private BETA.Card.Type _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

                protected override BETA.Card.Type Value
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