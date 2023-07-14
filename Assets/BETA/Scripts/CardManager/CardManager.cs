using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

namespace BETA
{
    // ==================================================================================================== CardManager

    public sealed partial class CardManager : Card.ManagerBehaviour<CardManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Card

        [Header("카드")]
        [SerializeField] private Data<Card> _cards = new Data<Card>();

        // =========================================================================== CardObject

        [Header("카드 오브젝트")]
        [SerializeField] private Data<CardObject> _cardObjects = new Data<CardObject>();

        [Header("선택됨!")]
        [SerializeField] private CardObject _selected;

        // =========================================================================== Data

        [Header("데이터")]
        [SerializeField] private Data<string> _data = new Data<string>();

        // ==================================================================================================== Property

        // =========================================================================== Card

        public override List<Card> Owned
        {
            get
            {
                return _cards.Owned;
            }
        }

        public override List<Card> Deck
        {
            get
            {
                return _cards.Deck;
            }
        }

        public override List<Card> Hand
        {
            get
            {
                return _cards.Hand;
            }
        }

        public override List<Card> Discarded
        {
            get
            {
                return _cards.Discarded;
            }
        }

        public override List<Card> Exiled
        {
            get
            {
                return _cards.Exiled;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Event

        protected override void Awake()
        {
            base.Awake();

            Card.Original.Initialize();

            Card.ReadAllData();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Card.Original.Clear();

            Card.ReadAllData();
        }

        // =========================================================================== EventSystems

        public override void OnBeginDrag(PointerEventData eventData, CardObject cardObject)
        {

        }

        public override void OnDrag(PointerEventData eventData, CardObject cardObject)
        {
            cardObject.transform.position = eventData.position;
        }

        public override void OnEndDrag(PointerEventData eventData, CardObject cardObject)
        {

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

        // =========================================================================== Card (BETA)

        public override void Acquire(Card card)
        {
            Owned.Add(card);

            _data.Owned.Add(card.InstanceID);
        }

        // =========================================================================== Data

        protected override void Clear()
        {
            Owned.Clear();
        }

        // =========================================================================== BETA

        public override void OnCardManagerAwake()
        {
            Card card1 = Card.Create(0);
            Card card2 = Card.Create(1);
            Card card3 = Card.Create(2);

            Acquire(card1);
            Acquire(card2);
            Acquire(card3);

            card1.Upgrade();
            card1.Upgrade();

            card2.Upgrade();
            card2.Upgrade();

            card3.Upgrade();
            card3.Upgrade();

            foreach (var card in Owned)
            {
                CardObject.Create(card.InstanceID, true);
            }
        }
    }
}
