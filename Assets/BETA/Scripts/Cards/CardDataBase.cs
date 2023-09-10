using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;

namespace BETA
{
    // ==================================================================================================== CardDataBase

    public sealed class CardDataBase : DataBase<CardRuntimeData, CardScriptableData>
    {
        //

        //

        public override void Initialize()
        {
            ScriptableData = Resources.Load<CardScriptableDataSet>("Data/Cards");
        }
    }
}
