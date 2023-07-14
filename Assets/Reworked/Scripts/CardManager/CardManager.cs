using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Reworked
{
    // ==================================================================================================== CardManager

    public partial class CardManager : MonoSingleton<CardManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        private Dictionary<string, Card> _cards = new Dictionary<string, Card>();

        public List<CardDataConsole> Consoles = new List<CardDataConsole>();

        // =========================================================================== Data

        [Header("데이터")]
        [SerializeField] private Data _data = new Data();

        [Header("데이터베이스")]
        [SerializeField] private GameDatabase _database;

        // =========================================================================== Resource

        [Header("카드 프리팹")]
        [SerializeField] private GameObject _cardPrefab;

        // ==================================================================================================== Property

        // =========================================================================== Card

        public Dictionary<string, Card> Cards
        {
            get
            {
                return _cards;
            }

            private set
            {
                Cards = value;
            }
        }

        // =========================================================================== Card

        public List<string> Deck
        {
            get
            {
                return _data.Deck;
            }
        }

        public List<Card> Owned
        {
            get
            {
                var list = new List<Card>();

                foreach (var instanceID in _data.Owned)
                {
                    list.Add(Cards[instanceID]);
                }

                return list;
            }
        }

        // =========================================================================== Resource

        public GameObject CardPrefab
        {
            get
            {
                return _cardPrefab;
            }

            private set
            {
                _cardPrefab = value;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        protected override void Awake()
        {
            base.Awake();

            LoadResourcesData();

            //////////////////////////////////////////////////////////////////////////////////////////////////// BETA
            OnAwakeCardManager();
            //////////////////////////////////////////////////////////////////////////////////////////////////// BETA
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Card.Cache.Clear();
        }

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            bool isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Card Manager";

                DontDestroyOnLoad(gameObject);
            }

            return isEmpty;
        }

        // =========================================================================== Resource

        private void LoadResourcesData()
        {
            CardPrefab = Resources.Load<GameObject>("Prefabs/Card");
        }

        // =========================================================================== BETA

        // 카드 - 데이터 링크: 
        //  - 인스턴스 ID 생성
        //  - 카드 데이터 생성
        //  - 카드 생성
        //  - 카드 추가
        //  - 카드 데이터 할당
        private void OnAwakeCardManager()
        {
            // 인스턴스 ID 생성
            var instanceID1 = Game.Allocate(InstanceType.CARD);
            var instanceID2 = Game.Allocate(InstanceType.CARD);
            var instanceID3 = Game.Allocate(InstanceType.CARD);
            var instanceID4 = Game.Allocate(InstanceType.CARD);
            var instanceID5 = Game.Allocate(InstanceType.CARD);

            /// 인스턴스 ID 출력
            Debug.Log(instanceID1);
            Debug.Log(instanceID2);
            Debug.Log(instanceID3);
            Debug.Log(instanceID4);
            Debug.Log(instanceID5);

            Debug.Log("==================================================");

            // 카드 데이터 생성
            Card.Data.Create(instanceID1);
            Card.Data.Create(instanceID2);
            Card.Data.Create(instanceID3);
            Card.Data.Create(instanceID4);
            Card.Data.Create(instanceID5);

            // 카드 생성
            Cards.Add(instanceID1, Card.Create(instanceID1));
            Cards.Add(instanceID2, Card.Create(instanceID2));
            Cards.Add(instanceID3, Card.Create(instanceID3));
            Cards.Add(instanceID4, Card.Create(instanceID4));
            Cards.Add(instanceID5, Card.Create(instanceID5));

            // 카드 추가
            _data.Owned.Add(instanceID1);
            _data.Owned.Add(instanceID2);
            _data.Owned.Add(instanceID3);
            _data.Owned.Add(instanceID4);
            _data.Owned.Add(instanceID5);

            // 카드 데이터 할당
            Card.Cache.Data[instanceID1].Name = "마법구";
            Card.Cache.Data[instanceID1].Cost = 1;
            Card.Cache.Data[instanceID1].Description = "데미지를 6 줍니다.";

            Card.Cache.Data[instanceID2].Name = "보호막";
            Card.Cache.Data[instanceID2].Cost = 1;
            Card.Cache.Data[instanceID2].Description = "쉴드를 5 얻습니다.";

            Card.Cache.Data[instanceID3].Name = "화염구";
            Card.Cache.Data[instanceID3].Cost = 2;
            Card.Cache.Data[instanceID3].Description = "데미지를 5 주고, 화상을 3 부여합니다.";

            Card.Cache.Data[instanceID4].Name = "마력창";
            Card.Cache.Data[instanceID4].Cost = 3;
            Card.Cache.Data[instanceID4].Description = "데미지를 15 주고, 마나를 1 회복합니다.";

            Card.Cache.Data[instanceID5].Name = "불안정한 고서";
            Card.Cache.Data[instanceID5].Cost = 1;
            Card.Cache.Data[instanceID5].Description = "카드를 1장 드로우하고, [불안정] 효과를 부여합니다.";

            /// 카드 데이터 출력
            for (int i = 0; i < Owned.Count; i++)
            {
                Debug.Log($"data.name={Owned[i].Name}, data.cost={Owned[i].Cost}, data.description={Owned[i].Description}");
            }

            Debug.Log("==================================================");

            foreach (var keyValuePair in Card.Cache.Data)
            {
                var data = keyValuePair.Value;

                Debug.Log($"data.name={data.Name}, data.cost={data.Cost}, data.description={data.Description}");
            }

            foreach (var data in Card.Cache.Data)
            {
                Consoles.Add(new CardDataConsole()
                {
                    Key = data.Key,

                    Data = data.Value
                });
            }
        }
    }
}

// 카드 데이터 종류

// 1. 이름, 비용 등 Status 정보
// 2. 공격, 방어 등 효과 정보
// 3. 카드 테두리, 이미지 등 Sprite 정보
// 4. 타겟 정보

// 1번의 경우 [변동 빈도 잦음 / 다양한 값으로 변동 가능]
// 2번의 경우 [고정]
// 3번의 경우 [변동 빈도 드묾 / 할당된 값으로만 변동 가능]

// 1번: 개별 데이터베이스 관리
// 2번: ???
// 3번: 공통 데이터베이스 관리 / enum 값과 level 값 받아서 변동