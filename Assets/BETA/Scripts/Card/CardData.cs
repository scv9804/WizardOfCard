using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace BETA
{
    // ==================================================================================================== Card.Data

    public sealed partial class Card
    {
        [Serializable] public sealed class Data
        {
            // ==================================================================================================== Field

            // =========================================================================== Identifier

            [Header("원본 ID")]
            public int SerialID;

            [Header("개체 ID")]
            public string InstanceID;

            // =========================================================================== Status

            // ================================================== Base

            [Header("이름")]
            public string Name;

            [Header("비용")]
            public int Cost;

            [Header("타입")]
            public Type Type;

            [Header("설명")]
            public string Description;

            // ================================================== Level

            [Header("강화 횟수")]
            [Range(0, MAX_LEVEL)] public int Level;

            // ==================================================================================================== Method

            // =========================================================================== Constructor

            private Data()
            {

            }

            // =========================================================================== Destructor

            ~Data()
            {

            }

            // =========================================================================== Instance

            public static Data Create(int serialID, string instanceID)
            {
                try
                {
                    var data = new Data();

                    data.SerialID = serialID;
                    data.InstanceID = instanceID;

                    Instance.Data.Add(instanceID, data);

                    data.Refresh();

                    return data;
                }
                catch (Exception e)
                {
                    EditorDebug.EditorLogError($"! CARD DATA CREATE ERROR ! {e}");

                    return null;
                }
            }

            public void Delete()
            {
                try
                {
                    Instance.Data.Remove(InstanceID);
                }
                catch (Exception e)
                {
                    EditorDebug.EditorLogError($"! CARD DATA DELETE ERROR ! {e}");
                }
            }

            // =========================================================================== Data

            public void Refresh()
            {
                var data = Original.Data.Create(SerialID, Level);

                Name = data.Name;
                Cost = data.Cost;
                Type = data.Type;
                Description = data.Description;
            }

            // =========================================================================== BETA

            public string ReadData()
            {
                string serialID = "Serial ID".Color("#7FFFD4").Bold();

                string name = "Name".Color("#7FFFD4").Bold();
                string cost = "Cost".Color("#7FFFD4").Bold();
                string type = "Type".Color("#7FFFD4").Bold();
                string description = "Description".Color("#7FFFD4").Bold();

                string level = "Level".Color("#7FFFD4").Bold();

                return $"{serialID}: {SerialID}, {name}: {Name}, {cost}: {Cost}, {type}: {Type}, {description}: {Description}, {level}: {Level}";
            }
        }
    }
}

// Card.Name
// Card.Cost
// Card.Oblivion
// Card.Description

// Card.Type
// Card.Distance
// Card.Range

// Card.Attack
// Card.Shield
// Card.Heal
// Card.Draw
// ...

// ... 어우 많아 ...

// Attack 이후로는 능력 노드에서 받아오겠습니다^^ 그게 빠르고 편해^^