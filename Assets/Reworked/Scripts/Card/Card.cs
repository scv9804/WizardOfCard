using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Reworked
{
    // ==================================================================================================== Card

    [Serializable] public partial class Card : ICard
    {
        // ==================================================================================================== Fixed

        // =========================================================================== Status

        public const int MAX_LEVEL = 2;

        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("개체 ID")]
        [SerializeField, JsonProperty("InstanceID")] private string _instanceID;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        [JsonIgnore] public string InstanceID
        {
            get
            {
                return _instanceID;
            }

            private set
            {
                _instanceID = value;
            }
        }

        [JsonIgnore] public int SerialID
        {
            get
            {
                return Cache.Data[InstanceID].SerialID;
            }

            private set
            {
                Cache.Data[InstanceID].SerialID = value;
            }
        }

        // =========================================================================== Status

        // ================================================== Base

        [JsonIgnore] public string Name
        {
            get
            {
                return Cache.Data[InstanceID].Name;
            }

            private set
            {
                Cache.Data[InstanceID].Name = value;
            }
        }

        [JsonIgnore] public int Cost
        {
            get
            {
                return Cache.Data[InstanceID].Cost;
            }

            private set
            {
                Cache.Data[InstanceID].Cost = value;
            }
        }

        [JsonIgnore] public string Description
        {
            get
            {
                return Cache.Data[InstanceID].Description;
            }

            private set
            {
                Cache.Data[InstanceID].Description = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Instance

        public static Card Create(string instanceID)
        {
            var card = new Card();

            card.InstanceID = instanceID;

            return card;
        }
    }
}