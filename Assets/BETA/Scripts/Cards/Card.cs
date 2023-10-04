using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;
using BETA.Interfaces;

using System;

namespace BETA
{
    // ==================================================================================================== Card

    [Serializable]
    public sealed class Card : Data.Unit<CardRuntimeData>, ICard
    {
        // ==================================================================================================== Constance

        // =========================================================================== General

        public const int MAX_LEVEL = 2;

        // ==================================================================================================== Property

        // =========================================================================== General

        public Enums.CardType Type
        {
            get
            {
                return Data.Type;
            }
        }

        public CardKeyword Keyword
        {
            get
            {
                return Data.Keyword;
            }
        }

        public int Level
        {
            get
            {
                return Data.Level;
            }
        }

        public int Cost
        {
            get
            {
                return Data.Cost;
            }
        }

        public string Description
        {
            get
            {
                return Data.Description;
            }
        }

        // =========================================================================== Ability

        //public int Damage
        //{
        //    get;
        //}

        //public int Shield
        //{
        //    get;
        //}

        //public int Heal
        //{
        //    get;
        //}

        //public int Draw
        //{
        //    get;
        //}

        //public int Count
        //{
        //    get;
        //}

        // =========================================================================== Data

        public static Data.CardDataSet DataSet
        {
            get
            {
                return DataManager.Instance.GetDataSet<Data.CardDataSet>();
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        private Card() : base() { }

        public Card(string instanceID, int serialID) : base(instanceID, serialID) { }

        // =========================================================================== Data

        protected override CardRuntimeData Create(string instanceID, int serialID)
        {
            return new CardRuntimeData(instanceID, serialID);
        }

        public override void Refresh()
        {
            var scriptableData = DataSet.Data[SerialID];

            Data.Type = scriptableData.Type;

            Data.Keyword = scriptableData.Keyword[Level];
            Data.Cost = scriptableData.Cost[Level];
 
            //Data.ApplyStateModifier();
 
            SetName();
            SetDescription();

            base.Refresh();

            #region void SetName();

            void SetName()
            {
                var name = $"{scriptableData.Name} I";

                for (var i = 0; i < Level; i++)
                {
                    name = $"{name}I";
                }

                Data.Name = name;
            }

            #endregion

            #region void SetDescription();

            void SetDescription()
            {
                Data.Description = scriptableData.Description[Level];
            } 

            #endregion
        }

        // =========================================================================== General

        public void Upgrade(int upgrade = 1)
        {
            var level = Level + upgrade;

            Data.Level = level > MAX_LEVEL ? MAX_LEVEL : level;

            Refresh();
        }
    }
}
