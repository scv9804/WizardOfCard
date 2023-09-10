using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;

using System;

namespace BETA
{
    // ==================================================================================================== CardScriptableData

    [Serializable]
    public sealed class Card : Model<CardRuntimeData>
    {
        //

        //

        //

        // ==================================================================================================== Property

        // =========================================================================== General

        public CardKeyword Keyword
        {
            get
            {
                return Data.Keyword;
            }

            private set
            {
                Data.Keyword = value;
            }
        }

        public int Level
        {
            get
            {
                return Data.Level;
            }

            private set
            {
                Data.Level = value;
            }
        }

        public int Cost
        {
            get
            {
                return Data.Cost;
            }

            private set
            {
                Data.Cost = value;
            }
        }

        public string Description
        {
            get
            {
                return Data.Description;
            }

            private set
            {
                Data.Description = value;
            }
        }

        // =========================================================================== Ability

        //public int Damage
        //{
        //    get; private set;
        //}

        //public int Shield
        //{
        //    get; private set;
        //}

        //public int Heal
        //{
        //    get; private set;
        //}

        //public int Draw
        //{
        //    get; private set;
        //}

        //public int Count
        //{
        //    get; private set;
        //}
    }
}
