using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Serialized.Card;

namespace BETA
{
    // ==================================================================================================== Card.Original

    public sealed partial class Card
    {
        public static partial class Original
        {
            // ==================================================================================================== Field

            // =========================================================================== Status

            // ================================================== Base

            private static List<string> s_name = new List<string>();

            private static List<int[]> s_cost = new List<int[]>();

            private static List<Type> s_type = new List<Type>();

            private static List<string[]> s_description = new List<string[]>();

            // =========================================================================== Resource

            // ================================================== Sprite

            private static Dictionary<Type, Sprite[]> s_frameSprite = new Dictionary<Type, Sprite[]>();

            private static List<Sprite[]> s_artworkSprite = new List<Sprite[]>();

            // ================================================== Prefab

            private static GameObject s_prefab;

            // ==================================================================================================== Property

            // =========================================================================== Resource

            // ================================================== Prefab

            public static GameObject Prefab
            {
                get
                {
                    return s_prefab;
                }

                set
                {
                    s_prefab = value;
                }
            }

            // ==================================================================================================== Method

            // =========================================================================== Resource

            public static void Initialize()
            {
                Prefab = Resources.Load<GameObject>("Prefabs/Card (BETA)");

                s_name = Resources.Load<CardNameTable>("Data/Card/NameTable").Deserialize();
                s_cost = Resources.Load<CardCostTable>("Data/Card/CostTable").Deserialize();
                s_type = Resources.Load<CardTypeTable>("Data/Card/TypeTable").Deserialize();
                s_description = Resources.Load<CardDescriptionTable>("Data/Card/DescriptionTable").Deserialize();

                s_frameSprite = Resources.Load<CardFrameSpriteTable>("Data/Card/FrameSpriteTable").Deserialize();
                s_artworkSprite = Resources.Load<CardArtworkSpriteTable>("Data/Card/ArtworkSpriteTable").Deserialize();
            }

            public static void Clear()
            {
                Prefab = null;

                s_name = null;
                s_cost = null;
                s_type = null;
                s_description = null;

                s_frameSprite = null;
                s_artworkSprite = null;
            }
        }
    }
}