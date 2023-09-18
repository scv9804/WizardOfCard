using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Delegates;
using BETA.Enums;

namespace BETA.Interfaces
{
    // ==================================================================================================== IDataBase

    public interface IDataBase
    {

    }

    // ==================================================================================================== IRuntimeDataBase

    public interface IRuntimeDataBase<TRuntimeData> : IDataBase where TRuntimeData : RuntimeData
    {
        // ==================================================================================================== Method

        // =========================================================================== Data

        public void Add(TRuntimeData data);

        public void Remove(TRuntimeData data);

        public void Subscribe(IUnit<TRuntimeData> unit);

        public void Unsubscribe(IUnit<TRuntimeData> unit);
    }

    // ==================================================================================================== IScriptableDataBase

    public interface IScriptableDataBase<TDataSet> : IDataBase where TDataSet : DataSet
    {
        // ==================================================================================================== Property

        // =========================================================================== DataSet

        public TDataSet DataSet
        {
            get;
        }
    }

    // ==================================================================================================== IModel

    public interface IUnit<TRuntimeData> where TRuntimeData : RuntimeData
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

        public int SerialID
        {
            get;
        }

        // =========================================================================== General

        public string Name
        {
            get;
        }

        // ==================================================================================================== Method

        // =========================================================================== Instance

        public void Refresh();
    }

    // ==================================================================================================== ICard

    public interface ICard : IUnit<CardRuntimeData>
    {
        // ==================================================================================================== Property

        // =========================================================================== General

        public CardType Type
        {
            get;
        }

        public CardKeyword Keyword
        {
            get;
        }

        public int Level
        {
            get;
        }

        public int Cost
        {
            get;
        }

        public string Description
        {
            get;
        }
    }

    // ==================================================================================================== IUnitObject

    public interface IUnitObject
    {

    }

    // ==================================================================================================== ICardObject

    public interface ICardObject : IUnitObject
    {

    }
}
