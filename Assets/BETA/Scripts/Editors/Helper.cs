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

using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TacticsToolkit;

namespace BETA.Editor
{
    public class Helper : SerializedMonoBehaviour
    {
        // ==================================================================================================== Field

        [FoldoutGroup("Base Cake Instance")]
        public Cake Cake = new Cake();

        public Option[] Options;

        public List<Option> Repacked;

        public GameObject Folder;

        public Dictionary<string, MonoData> MonoData = new Dictionary<string, MonoData>();
        public List<DataMonoBehaviour> Behaviours = new List<DataMonoBehaviour>();

        public DataMonoBehaviour Original;

        //public Library<string, string> SavedData = new Library<string, string>();
        //public Library<string, Status> LoadedData = new Library<string, Status>();

        // ==================================================================================================== Method

        void Start()
        {
            //Effect<CakeRuntimeData>.AddEffect(Cake, 0);

            //FindAbility(Cake);

            Create("0010");
            Create("0010");
            Create("4592");
            Create("4175");

            First(); 
            Second();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (var behaviour in Behaviours)
                {
                    Destroy(behaviour.gameObject);
                }

                Behaviours.Clear();
            }
        }

        public void First()
        {
            Card card1 = new Card("0010", 0);
            Card card2 = new Card("4592", 0);
            Card card3 = new Card("4175", 0);
        }
        
        public void Second()
        {
            GC.Collect();
        }

        public void FindAbility<TData>(GameComponent<TData> component) where TData : RuntimeData
        {
            Activate(component.Ability);
        }

        public void Activate<TData>(Ability<TData> ability) where TData : RuntimeData
        {
            ability.PrintInfo();
        }

        public DataMonoBehaviour Create(string instanceID)
        {
            MonoData monoData;

            if (MonoData.ContainsKey(instanceID))
            {
                monoData = MonoData[instanceID];
            }
            else
            {
                monoData = new MonoData();

                MonoData.Add(instanceID, monoData);
            }

            var behaviour = Instantiate(Original, Folder.transform);
            behaviour.Data = monoData;

            Behaviours.Add(behaviour);

            return behaviour;
        }

        //public Library<string, T> Load<T>(Library<string, string> saved) where T : Status, new()
        //{
        //    var result = new Library<string, T>();
        //    var memoization = new Dictionary<string, T>();

        //    foreach (var category in saved)
        //    {
        //        // 빠르긴 한테 동일 개체가 개별 개체가 되어버림
        //        //result.Add(category.Key, category.Value.ConvertAll((hash) =>
        //        //{
        //        //    return new T()
        //        //    {
        //        //        Name = hash
        //        //    };
        //        //}));

        //        // 느리지만 복사된 개체의 특성 유지 
        //        foreach (var hash in category.Value)
        //        {
        //            T item;

        //            if (memoization.ContainsKey(hash))
        //            {
        //                item = memoization[hash];
        //            }
        //            else
        //            {
        //                item = new T()
        //                {
        //                    Name = hash
        //                };

        //                memoization.Add(hash, item);
        //            }

        //            result.Add(category.Key, item);
        //        }
        //    }

        //    return result;
        //}
    }

    [Serializable]
    public class Status
    {
        [ShowInInspector]
        private int _ID;

        [ShowInInspector]
        private string _name;

        [ShowInInspector]
        private int _attack;
        [ShowInInspector]
        private int _shield;

        [ShowInInspector]
        private string _description;

        // Behavior

        public int ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public int Attack
        {
            get
            {
                return _attack;
            }

            set
            {
                _attack = value;
            }
        }

        public int Shield
        {
            get
            {
                return _shield;
            }

            set
            {
                _shield = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }
    }

    // 스킬 사용 순서
    // 스킬 이름 입력
    // 그 이름 갖다가 캐릭터 능력창에서 찾아서 효과 발동...?

    // 그 후 활성화 된 타일에서 타겟 찾아서
    // 효과 apply

    [Serializable]
    public abstract class Option
    {
        public string Target;

        public abstract Option Clone();
    }

    [Serializable]
    public class AttackOption : Option
    {
        public int Attack;

        public AttackOption()
        {

        }

        public AttackOption(int attack)
        {
            Attack = attack;
        }

        public override Option Clone()
        {
            return new AttackOption(Attack);
        }
    }

    [Serializable]
    public class DefenseOption : Option
    {
        public int Defense;

        public DefenseOption()
        {

        }

        public DefenseOption(int defense)
        {
            Defense = defense;
        }

        public override Option Clone()
        {
            return new DefenseOption(Defense);
        }
    }

    public delegate void AbilityEventHandler<TRuntimeData>(Entity target, TRuntimeData data);

    public abstract class Ability<TData> where TData : RuntimeData
    {
        public abstract void PrintInfo();
    }

    public class CakeAbility : Ability<CakeRuntimeData>
    {
        public override void PrintInfo()
        {
            "케이크 유닛의 어빌리티 효과입니다.".Print();
        }
    }

    public class GameComponent<TData> where TData : RuntimeData
    {
        public TData Data;

        public Library<string, Effect<TData>> Effect = new Library<string, Effect<TData>>();

        public Ability<TData> Ability;

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
        public Cake()
        {
            Ability = new CakeAbility();
        }
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
        public override void Apply(CakeRuntimeData data)
        {

        }
    }

    [Serializable]
    public class CakeShieldEffect : EffectData<CakeRuntimeData>
    {
        public override void Apply(CakeRuntimeData data)
        {

        }
    }
}

// ================================================================================

// ============================================================

// ========================================

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
