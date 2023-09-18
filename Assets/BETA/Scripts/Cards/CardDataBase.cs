using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

namespace BETA
{
    // ==================================================================================================== CardDataBase

    public sealed class CardDataBase : DataBase<CardRuntimeData, CardDataSet>
    {
        // ==================================================================================================== Method

        // =========================================================================== Instance

        public override void Initialize()
        {
            DataSet = Resources.Load<CardDataSet>("Data/CardDataSet");
        }
    }
}
