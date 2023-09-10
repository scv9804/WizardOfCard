using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Interfaces;
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

        // =========================================================================== ????

        // ==================================================================================================== Property

        // =========================================================================== ????

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

        // =========================================================================== Data

        public void Add<TRuntimeData>(TRuntimeData data) where TRuntimeData : RuntimeData
        {
            Reiceve<TRuntimeData>((database) =>
            {
                database.Add(data);
            });
        }

        public void Remove<TRuntimeData>(TRuntimeData data) where TRuntimeData : RuntimeData
        {
            Reiceve<TRuntimeData>((database) =>
            {
                database.Remove(data);
            });
        }

        public void Subscribe<TRuntimeData>(IDataObserver<TRuntimeData> observer) where TRuntimeData : RuntimeData
        {
            Reiceve<TRuntimeData>((database) =>
            {
                database.Subscribe(observer);
            });
        }

        public void Unsubscribe<TRuntimeData>(IDataObserver<TRuntimeData> observer) where TRuntimeData : RuntimeData
        {
            Reiceve<TRuntimeData>((database) =>
            {
                database.Unsubscribe(observer);
            });
        }

        // =========================================================================== DataBase

        public void Reiceve<TRuntimeData>(Action<IRuntimeDataBase<TRuntimeData>> callback) where TRuntimeData : RuntimeData
        {
            var database = GetComponent<IRuntimeDataBase<TRuntimeData>>();

            callback.Invoke(database);
        }

        public void Reiceve<TScriptableData>(Action<IScriptableDataBase<TScriptableData>> callback) where TScriptableData : ScriptableData
        {
            var database = GetComponent<IScriptableDataBase<TScriptableData>>();

            callback.Invoke(database);
        }
    }
}