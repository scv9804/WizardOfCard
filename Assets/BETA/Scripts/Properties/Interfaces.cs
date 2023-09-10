using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Delegates;

namespace BETA.Interfaces
{
    // ==================================================================================================== IRuntimeDataBase

    public interface IRuntimeDataBase<TRuntimeData> where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Method

        // =========================================================================== Data

        public void Add(TRuntimeData data);

        public void Remove(TRuntimeData data);

        public void Subscribe(IDataObserver<TRuntimeData> observer);

        public void Unsubscribe(IDataObserver<TRuntimeData> observer);
    }

    // ==================================================================================================== IScriptableDataBase

    public interface IScriptableDataBase<TScriptableData> where TScriptableData : ScriptableData
    {

    }

    // ==================================================================================================== IDataObserver

    public interface IDataObserver<TRuntimeData> where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        public event ModelDataBindEvent<TRuntimeData> DataBinding;

        // ==================================================================================================== Property

        // =========================================================================== Instance

        public string InstanceID
        {
            get;
        }

        // =========================================================================== General

        public string Name
        {
            get;
        }
    }
}
