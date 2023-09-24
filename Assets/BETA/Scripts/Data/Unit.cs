using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Delegates;
using BETA.Interfaces;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== Unit

    public abstract class Unit<TRuntimeData> : IUnit<TRuntimeData> where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [SerializeField]
        private string _instanceID;

        // =========================================================================== Unit

        public event Action OnDestroy;

        // =========================================================================== Data

        public event ModelDataBindEvent<TRuntimeData> DataBinding;

        public event Action OnDataChanged;

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

        // =========================================================================== Data

        protected TRuntimeData Data
        {
            get
            {
                return DataBinding?.Invoke(InstanceID);
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        protected Unit()
        {
            DataManager.Instance.Subscribe(this);
        }

        public Unit(string instanceID, int serialID)
        {
            instanceID = DataManager.Instance.Allocate(instanceID);

            InstanceID = instanceID;

            if (DataManager.Instance.Reference[instanceID] == 0)
            {
                var data = Create(instanceID, serialID);

                DataManager.Instance.Add(data);
            }

            DataManager.Instance.Subscribe(this);

            Refresh();
        }

        // =========================================================================== Destructor

        ~Unit()
        {
            if (InstanceID == null)
            {
                return;
            }

            DataManager.Instance.Unsubscribe(this);

            if (DataManager.Instance.Reference[InstanceID] == 0)
            {
                Delete();
            }

            OnDestroy?.Invoke();
        }

        // =========================================================================== Instance

        // =========================================================================== Datas

        protected abstract TRuntimeData Create(string instanceID, int serialID);

        private void Delete()
        {
            DataManager.Instance.Remove(Data);
        }

        public virtual void Refresh()
        {
            OnDataChanged?.Invoke();
        }
    }
}
