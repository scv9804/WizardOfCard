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

using Spine.Unity;

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

        [FoldoutGroup("Base Cake Instance")]
        public Cake Cake = new Cake();

        Action Action;

        // ==================================================================================================== Method

        void Start()
        {
            Effect<CakeRuntimeData>.AddEffect(Cake, 0);
        }
    }
    
    public class GameComponent<TData> where TData : RuntimeData
    {
        public TData Data;

        public Library<string, Effect<TData>> Effect = new Library<string, Effect<TData>>();

        static GameComponent()
        {

        }

        public void ApplyRuntimeDataEffect()
        {

        }
    }

    [Serializable]
    public class Cake : GameComponent<CakeRuntimeData>
    {

    }

    [Serializable]
    public class RuntimeData
    {
        public string InstanceID;
        public int SerialID;

        public string Name;
    }

    [Serializable]
    public class CakeRuntimeData : RuntimeData
    {
        public int BaseAttack;
        public int BaseShield;

        public int Attack;
        public int Shield;

        public string Description;

        public event Action OnAttackHit;
    }

    [Serializable]
    public class Effect<TData> where TData : RuntimeData
    {
        public int SerialID;

        public static void AddEffect<TComponent>(TComponent component, int index) where TComponent : GameComponent<TData>
        {

        }
    }

    [Serializable]
    public class EffectRuntimeData<TData> where TData : RuntimeData
    {

    }

    [Serializable]
    public abstract class EffectData<TData> where TData : RuntimeData
    {


        public abstract void Apply(TData data);
    }

    [Serializable]
    public class CakeAttackEffect : EffectData<CakeRuntimeData>
    {

    }

    [Serializable]
    public class CakeShieldEffect : EffectData<CakeRuntimeData>
    {

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
