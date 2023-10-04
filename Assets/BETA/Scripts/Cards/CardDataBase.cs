using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

namespace BETA
{
    // ==================================================================================================== CardDataBase

    public sealed class CardDataBase : DataBase<CardRuntimeData, Data.CardDataSet>
    {
        // ==================================================================================================== Method

        // =========================================================================== Instance

        public override void Initialize()
        {
            DataSet = Resources.Load<Data.CardDataSet>("Data/CardDataSet");
        }
    }
}
