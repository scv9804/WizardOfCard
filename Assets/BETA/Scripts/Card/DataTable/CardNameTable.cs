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
            // ==================================================================================================== CardNameTable

            [CreateAssetMenu(menuName = "Card/Data/NameTable", fileName = "NameTable")]
            public sealed class CardNameTable : SerializedListObject<string, CardNameData>
            {

            }

            // ==================================================================================================== CardNameData

            [Serializable] public sealed class CardNameData : SerializedListData<string>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("이름")]
                [SerializeField] private string _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

                protected override string Value
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
