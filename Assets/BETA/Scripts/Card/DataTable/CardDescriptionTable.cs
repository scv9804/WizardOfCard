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
            // ==================================================================================================== CardDescriptionTable

            [CreateAssetMenu(menuName = "Card/Data/DescriptionTable", fileName = "DescriptionTable")]
            public class CardDescriptionTable : SerializedListObject<string[], CardDescriptionData>
            {

            }

            // ==================================================================================================== CardDescriptionData

            [Serializable] public sealed class CardDescriptionData : SerializedListData<string[]>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("설명")]
                [SerializeField, TextArea(5, 9)] private string[] _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

                protected override string[] Value
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
