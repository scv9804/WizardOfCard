using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Enums;
using BETA.Interfaces;
using BETA.Porting;
using BETA.Singleton;

using Newtonsoft.Json;

using System;

using Sirenix.OdinInspector;

namespace BETA
{
    // ==================================================================================================== DataManager

    public class DataManager : SingletonMonoBehaviour<DataManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Instance

        [ShowInInspector]
        private Dictionary<string, int> _reference = new Dictionary<string, int>();

        // ==================================================================================================== Property

        // =========================================================================== Instance

        public Dictionary<string, int> Reference
        {
            get
            {
                return _reference;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Data Manager";

                DontDestroyOnLoad(gameObject);
            }

            return isEmpty;
        }

        // =========================================================================== Instance

        public string Allocate(string instanceID = null)
        {
            if (instanceID == null)
            {
                Create();
            }

            if (!Reference.ContainsKey(instanceID))
            {
                Reference.Add(instanceID, 0);
            }

            return instanceID;

            #region void Create();

            void Create()
            {
                int ID;

                do
                {
                    ID = UnityEngine.Random.Range(0, GameManager.Instance.Configs.MaxInstanceCount);

                    instanceID = ID.ToString(GameManager.Instance.Configs.InstanceIDFormat);
                }
                while (Reference.ContainsKey(instanceID));
            }

            #endregion
        }

        // =========================================================================== Data

        public void Add<TRuntimeData>(TRuntimeData data) where TRuntimeData : RuntimeData
        {
            GetRuntimeDataBase<TRuntimeData>().Add(data);
        }

        public void Remove<TRuntimeData>(TRuntimeData data) where TRuntimeData : RuntimeData
        {
            GetRuntimeDataBase<TRuntimeData>().Remove(data);
        }

        public void Subscribe<TRuntimeData>(IUnit<TRuntimeData> unit) where TRuntimeData : RuntimeData
        {
            Reference[unit.InstanceID] += 1;

            GetRuntimeDataBase<TRuntimeData>().Subscribe(unit);
        }

        public void Unsubscribe<TRuntimeData>(IUnit<TRuntimeData> unit) where TRuntimeData : RuntimeData
        {
            Reference[unit.InstanceID] -= 1;

            if (Reference[unit.InstanceID] == 0)
            {
                Reference.Remove(unit.InstanceID);
            }

            GetRuntimeDataBase<TRuntimeData>().Unsubscribe(unit);
        }

        // =========================================================================== DataSet

        public TDataSet GetDataSet<TDataSet>() where TDataSet : DataSet
        {
            return GetScriptableDataBase<TDataSet>().DataSet;
        }

        // =========================================================================== DataBase

        private IRuntimeDataBase<TRuntimeData> GetRuntimeDataBase<TRuntimeData>() where TRuntimeData : RuntimeData
        {
            return GetComponent<IRuntimeDataBase<TRuntimeData>>();
        }

        private IScriptableDataBase<TDataSet> GetScriptableDataBase<TDataSet>() where TDataSet : DataSet
        {
            return GetComponent<IScriptableDataBase<TDataSet>>();
        }
    }
}