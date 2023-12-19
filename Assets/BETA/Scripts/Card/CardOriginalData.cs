using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    // ==================================================================================================== Card.Original.Data

    public sealed partial class Card
    {
        public static partial class Original
        {
            [Serializable] public sealed class Data
            {
                // ==================================================================================================== Field

                // =========================================================================== Identifier

                [Header("원본 ID")]
                public int SerialID;

                // =========================================================================== Status

                // ================================================== Level

                [Header("강화 횟수")]
                public int Level;

                // ==================================================================================================== Property

                // =========================================================================== Status

                // ================================================== Base

                public string Name
                {
                    get
                    {
                        return s_name[SerialID];
                    }
                }

                public int Cost
                {
                    get
                    {
                        return s_cost[SerialID][Level];
                    }
                }

                public Type Type
                {
                    get
                    {
                        return s_type[SerialID];
                    }
                }

                public string Description
                {
                    get
                    {
                        return s_description[SerialID][Level];
                    }
                }

                // =========================================================================== Resource

                // ================================================== Sprite

                public Sprite ArtworkSprite
                {
                    get
                    {
                        return s_artworkSprite[SerialID][Level];
                    }
                }

                public Sprite FrameSprite
                {
                    get
                    {
                        return s_frameSprite[Type][Level];
                    }
                }

                // ==================================================================================================== Method

                // =========================================================================== Constructor

                private Data()
                {

                }

                // =========================================================================== Instance

                public static Data Create(int serialID, int level = 0)
                {
                    var data = new Data();

                    data.SerialID = serialID;
                    data.Level = level;

                    return data;
                }

                // =========================================================================== BETA

                public string ReadData()
                {
                    string serialID = "Serial ID".Color("#5F9EA0").Bold();

                    string name = "Name".Color("#5F9EA0").Bold();
                    string cost = "Cost".Color("#5F9EA0").Bold();
                    string type = "Type".Color("#5F9EA0").Bold();
                    string description = "Description".Color("#5F9EA0").Bold();

                    string level = "Level".Color("#5F9EA0").Bold();

                    return $"{serialID}: {SerialID}, {name}: {Name}, {cost}: {Cost}, {type}: {Type}, {description}: {Description}, {level}: {Level}";
                }
            }
        }
    }
}