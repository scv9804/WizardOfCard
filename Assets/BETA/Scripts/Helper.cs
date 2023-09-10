using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Data;
using BETA.Delegates;
using BETA.Enums;
using BETA.Graphics;
using BETA.Interfaces;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

using System;
using System.IO;
using System.Linq;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BETA.Editor
{
    public class Helper : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        public CardScriptableData ScriptableData;
        public CardRuntimeData RuntimeData = new CardRuntimeData();

        public CardObject CardObject;

        // ==================================================================================================== Method

        void Start()
        {
            RuntimeData.InstanceID = "5603";
            RuntimeData.SerialID = 0;

            RuntimeData.Name = ScriptableData.Name;
            RuntimeData.Type = ScriptableData.Type;

            RuntimeData.Level = 2;

            RuntimeData.Cost = ScriptableData.Cost[RuntimeData.Level];
            RuntimeData.Keyword = ScriptableData.Keyword[RuntimeData.Level];
            RuntimeData.Description = ScriptableData.Description[RuntimeData.Level].Replace("{0}", "6");

            //

            var components = CardObject.GetComponent<CardObjectComponents>();

            components.FrameImage.sprite = ScriptableData.Frame.Sprite[RuntimeData.Level];
            components.ArtworkImage.sprite = ScriptableData.Artwork.Sprite[RuntimeData.Level];

            components.NameTMP.text = RuntimeData.Name;
            components.CostTMP.text = RuntimeData.Cost.ToString();
            components.DescriptionTMP.text = RuntimeData.Description;

            DataManager.Instance.Add(RuntimeData);
        }
    }

    [Serializable]
    public class Cake
    {
        public string Name;

        public int ID;

        public string Description;
    }
}

// Entities
// 00 : Players
// 01 : Enemies
// 02 : Boss
// 03 : Special

// Item
// 10 : Equipments
// 11 : Consumables
// 12 : Event

// Card
// 20 : Card

// Map
// 30 : Empty
// 31 : Enemy
// 32 : Boss
// 33 : Shop
// 34 : Event

// ==================================================================================================== Map Create Test

//public CakeRuntimeData Data = new CakeRuntimeData();

//[TextArea] 
//public string Json;

//public CakeRuntimeData Result;

//public Tilemap Tilemap;

//[TableMatrix(SquareCells = true, HideColumnIndices = true, HideRowIndices = true)]
//public Tile[,] Map = new Tile[4, 4];

// 이거 근데 이렇게 넣으면 거울모드 생성되네 ㅋㅋㅋㅋㅋㅋㅋ 아 ㅋㅋㅋㅋㅋㅋㅋㅋㅋ

//Tilemap.ClearAllTiles();

//for (var i = 0; i < Map.GetLength(0); i++)
//{
//    for (var j = 0; j < Map.GetLength(1); j++)
//    {
//        Tilemap.SetTile(new Vector3Int(i, j, 0), Map[i, j]);
//    }
//}

// ==================================================================================================== Dictionary Drawer Test

//[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
//public Dictionary<string, CakeAuthoringData> OneLine = new Dictionary<string, CakeAuthoringData>();

//[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
//public Dictionary<string, CakeAuthoringData> Foldout = new Dictionary<string, CakeAuthoringData>();

//[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
//public Dictionary<string, CakeAuthoringData> CollapsedFoldout = new Dictionary<string, CakeAuthoringData>();

//[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
//public Dictionary<string, CakeAuthoringData> ExpandedFoldout = new Dictionary<string, CakeAuthoringData>();
