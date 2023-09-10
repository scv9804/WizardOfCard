using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Delegates;
using BETA.Enums;
using BETA.Interfaces;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== CardObject

    public class CardObject : SerializedMonoBehaviour, IDataObserver<CardRuntimeData>
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [ShowInInspector]
        private string _instanceID;

        // =========================================================================== Data

        public event ModelDataBindEvent<CardRuntimeData> DataBinding;

        // ==================================================================================================== Property

        // =========================================================================== Instance

        public string InstanceID
        {
            get
            {
                return _instanceID;
            }

            set
            {
                _instanceID = value;
            }
        }

        public int SerialID
        {
            get
            {
                return Data.SerialID;
            }
        }

        // =========================================================================== General

        public string Name
        {
            get
            {
                return Data.Name;
            }
        }

        public CardType Type
        {
            get
            {
                return Data.Type;
            }
        }

        public int Level
        {
            get
            {
                return Data.Level;
            }
        }

        public CardKeyword Keyword
        {
            get
            {
                return Data.Keyword;
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

        // =========================================================================== Component

        //public Sprite FrameSprite
        //{
        //    get
        //    {
        //        return DataManager.Instance.
        //    }
        //}

        // =========================================================================== Data

        protected CardRuntimeData Data
        {
            get
            {
                return DataBinding?.Invoke(InstanceID);
            }
        }
    }
}
