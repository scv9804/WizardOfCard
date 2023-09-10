using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Interfaces;

using Sirenix.OdinInspector;

using System;

namespace BETA.Data
{
    // ==================================================================================================== DataBase

    public abstract class DataBase<TRuntimeData, TScriptableData> : SerializedMonoBehaviour, IRuntimeDataBase<TRuntimeData>, IScriptableDataBase<TScriptableData> where TRuntimeData : RuntimeData where TScriptableData : ScriptableData
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [ShowInInspector]
        private ScriptableDataSet<TScriptableData> _scriptableData;

        [ShowInInspector] [HideReferenceObjectPicker]
        private DataBaseData<TRuntimeData> _data = new DataBaseData<TRuntimeData>();

        // ==================================================================================================== Property

        // =========================================================================== Data

        protected ScriptableDataSet<TScriptableData> ScriptableData
        {
            get
            {
                return _scriptableData;
            }

            set
            {
                _scriptableData = value;
            }
        }

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

        // ==================================================================================================== Method

        // =========================================================================== Event

        private void Awake()
        {
            Initialize();
        }

        // =========================================================================== Data

        public abstract void Initialize();

        public void Add(TRuntimeData data)
        {
            RuntimeData.Add(data.InstanceID, data);
        }

        public void Remove(TRuntimeData data)
        {
            RuntimeData.Remove(data.InstanceID);
        }

        public void Subscribe(IDataObserver<TRuntimeData> observer)
        {
            observer.DataBinding += Bind;
        }

        public void Unsubscribe(IDataObserver<TRuntimeData> observer)
        {
            observer.DataBinding -= Bind;
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
