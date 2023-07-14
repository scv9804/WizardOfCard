using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    // ==================================================================================================== Card

    [Serializable] public sealed partial class Card
    {
        // ==================================================================================================== Constant

        // =========================================================================== Level

        public const int MAX_LEVEL = 2;

        // ==================================================================================================== Field

        // =========================================================================== Identifier

        [Header("개체 ID")]
        [SerializeField] private string _instanceID;

        // ==================================================================================================== Property

        // =========================================================================== Identifier

        public int SerialID
        {
            get
            {
                return Instance.Data[InstanceID].SerialID;
            }

            private set
            {
                Instance.Data[InstanceID].SerialID = value;
            }
        }

        public string InstanceID
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

        // =========================================================================== Status

        // ================================================== Base

        public string Name
        {
            get
            {
                return Instance.Data[InstanceID].Name;
            }

            private set
            {
                Instance.Data[InstanceID].Name = value;
            }
        }

        public int Cost
        {
            get
            {
                return Instance.Data[InstanceID].Cost;
            }

            private set
            {
                Instance.Data[InstanceID].Cost = value;
            }
        }

        public string Description
        {
            get
            {
                return Instance.Data[InstanceID].Description;
            }

            private set
            {
                Instance.Data[InstanceID].Description = value;
            }
        }

        // ================================================== Level

        public int Level
        {
            get
            {
                return Instance.Data[InstanceID].Level;
            }

            private set
            {
                Instance.Data[InstanceID].Level = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        private Card()
        {

        }

        // =========================================================================== Destructor

        ~Card()
        {
            if (InstanceID is null)
            {
                return;
            }

            Delete();
        }

        // =========================================================================== Instance

        public static Card Create(int serialID, string instanceID = Game.Instance.NEW_INSTANCE)
        {
            try
            {
                instanceID = Game.Instance.Allocate(instanceID);

                var card = new Card();

                card.InstanceID = instanceID;

                if (!Instance.Data.ContainsKey(instanceID))
                {
                    Data.Create(serialID, instanceID);
                }

                return card;
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! CARD CREATE ERROR ! {e}");

                return null;
            }
        }

        public void Delete()
        {
            try
            {
                Game.Instance.Deallocate(InstanceID);

                if (!Game.Instance.IsContains(InstanceID))
                {
                    Instance.Data[InstanceID].Delete();
                }
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! CARD DELETE ERROR ! {e}");
            }
        }

        // =========================================================================== Status

        // ================================================== Level

        public void Upgrade()
        {
            if (Level < MAX_LEVEL)
            {
                Level += 1;
            }

            Instance.Data[InstanceID].Refresh();
        }

        // =========================================================================== BETA

        public static void ReadAllData()
        {
            Instance.ReadAllData();
        }
    }
}
