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
            // ==================================================================================================== CardFrameSpriteTable

            [CreateAssetMenu(menuName = "Card/Data/FrameSpriteTable", fileName = "FrameSpriteTable")]
            public sealed class CardFrameSpriteTable : SerializedDictionaryObject<BETA.Card.Type, Sprite[], CardFrameSpriteData>
            {

            }

            // ==================================================================================================== CardFrameSpriteData

            [Serializable] public sealed class CardFrameSpriteData : SerializedDictionaryData<BETA.Card.Type, Sprite[]>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("키")]
                [SerializeField] private BETA.Card.Type _key;

                [Header("프레임 스프라이트")]
                [SerializeField] private Sprite[] _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

                protected override BETA.Card.Type Key
                {
                    get
                    {
                        return _key;
                    }
                }

                protected override Sprite[] Value
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
