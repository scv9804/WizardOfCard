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
            // ==================================================================================================== CardArtworkSpriteTable

            [CreateAssetMenu(menuName = "Card/Data/ArtworkSpriteTable", fileName = "ArtworkSpriteTable")]
            public sealed class CardArtworkSpriteTable : SerializedListObject<Sprite[], CardArtworkSpriteData>
            {

            }

            // ==================================================================================================== CardArtworkSpriteData

            [Serializable] public sealed class CardArtworkSpriteData : SerializedListData<Sprite[]>
            {
                // ==================================================================================================== Field

                // =========================================================================== Data

                [Header("이미지 스프라이트")]
                [SerializeField] private Sprite[] _value;

                // ==================================================================================================== Property

                // =========================================================================== Data

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
