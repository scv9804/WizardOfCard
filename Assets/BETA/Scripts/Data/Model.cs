using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Delegates;
using BETA.Interfaces;

using Sirenix.OdinInspector;

namespace BETA.Data
{
    // ==================================================================================================== Model

    public abstract class Model<TRuntimeData> : IDataObserver<TRuntimeData> where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [SerializeField]
        private string _instanceID;

        // =========================================================================== Data

        public event ModelDataBindEvent<TRuntimeData> DataBinding;

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

            protected set
            {
                Data.SerialID = value;
            }
        }

        // =========================================================================== General

        public string Name
        {
            get
            {
                return Data.Name;
            }

            protected set
            {
                Data.Name = value;
            }
        }

        // =========================================================================== Data

        protected TRuntimeData Data
        {
            get
            {
                return DataBinding?.Invoke(InstanceID);
            }
        }
    }
}
