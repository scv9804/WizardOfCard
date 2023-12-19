using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.ObjectModel;

namespace Reworked
{
    // ==================================================================================================== Card.OriginData

    public partial class Card
    {
        [Serializable] public class OriginData
        {
            // ==================================================================================================== Field

            // =========================================================================== Status

            // ================================================== Base

            [Header("이름")]
            [SerializeField] private string _name;

            [Header("비용")]
            [SerializeField] private int[] _cost = new int[MAX_LEVEL + 1];

            [Header("설명")]
            [SerializeField, TextArea(4, 9)] private string[] _description = new string[MAX_LEVEL + 1];

            // ==================================================================================================== Field

            // =========================================================================== Status

            // ================================================== Base

            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public ReadOnlyCollection<int> Cost
            {
                get
                {
                    return Array.AsReadOnly(_cost);
                }
            }

            public ReadOnlyCollection<string> Description
            {
                get
                {
                    return Array.AsReadOnly(_description);
                }
            }
        }
    } 
}
