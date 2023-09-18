using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Enums;
using BETA.Interfaces;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== DataBase

    public abstract class DataBase<TRuntimeData, TDataSet> : SerializedMonoBehaviour, IRuntimeDataBase<TRuntimeData>, IScriptableDataBase<TDataSet> where TRuntimeData : RuntimeData where TDataSet : DataSet
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [ShowInInspector] [HideReferenceObjectPicker] [FoldoutGroup("런타임 데이터")]
        private DataBaseData<TRuntimeData> _data = new DataBaseData<TRuntimeData>();

        // =========================================================================== Data

        [SerializeField] [FoldoutGroup("데이터 셋")]
        private TDataSet _dataSet;

        // ==================================================================================================== Property

        // =========================================================================== Data

        protected Dictionary<string, TRuntimeData> RuntimeData
        {
            get
            {
                return _data.RuntimeData;
            }

            set
            {
                _data.RuntimeData = value;
            }
        }

        // =========================================================================== DataSet

        public TDataSet DataSet
        {
            get
            {
                return _dataSet;
            }

            protected set
            {
                _dataSet = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            Initialize();
        }

        // =========================================================================== Instance

        public abstract void Initialize();

        // =========================================================================== Data

        public void Add(TRuntimeData data)
        {
            RuntimeData.Add(data.InstanceID, data);
        }

        public void Remove(TRuntimeData data)
        {
            RuntimeData.Remove(data.InstanceID);
        }

        public void Subscribe(IUnit<TRuntimeData> unit)
        {
            unit.DataBinding += Bind;
        }

        public void Unsubscribe(IUnit<TRuntimeData> unit)
        {
            unit.DataBinding -= Bind;
        }

        private TRuntimeData Bind(string instanceID)
        {
            return RuntimeData[instanceID];
        }
    }

    // ==================================================================================================== DataBaseData

    [Serializable]
    public sealed class DataBaseData<TRuntimeData> where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        public Dictionary<string, TRuntimeData> RuntimeData = new Dictionary<string, TRuntimeData>();
    }
}
