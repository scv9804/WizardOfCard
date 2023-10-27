using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

// ================================================================================ CardManager

//public class CardManager : Singleton<CardManager>
//{
//    // ================================================================================ Constance

//    // ============================================================ Card

//    public const string OWN = "OWN";
//    public const string DECK = "DECK";
//    public const string HAND = "HAND";
//    public const string DISCARD = "DISCARD";
//    public const string EXCLUDE = "EXCLUDE";

//    public const string SHOP = "SHOP";
//    public const string EVENT = "EVENT";
//    public const string REWARD = "REWARD";

//    public const string TEMPORARY = "TEMPORARY";

//    // ================================================================================ Field

//    // ============================================================ Card

//    [ShowInInspector, TitleGroup("카드")]
//    private Library<string, Card> _cards = new Library<string, Card>();

//    [ShowInInspector, TitleGroup("카드 데이터")]
//    private Dictionary<string, CardData> _cardData = new Dictionary<string, CardData>();

//    // ================================================================================ Property

//    // ============================================================ Card

//    public Library<string, Card> Cards
//    {
//        get
//        {
//            return _cards;
//        }
//    }

//    // ================================================================================ Method

//    // ============================================================ Singleton

//    protected override void Initialize()
//    {
//        base.Initialize();
//    }

//    private void Start()
//    {
//        //for (var i = 0; i < 10; i++)
//        //{
//        //    var instanceID = DataManager.Instance.Allocate();

//        //    //var card = new Card(instanceID, 0);
//        //    //card.Data += () => Bind(instanceID);

//        //    var card = Card.Create();
//        //    card.Data += () => Bind(instanceID);

//        //    var cardData = new CardData(instanceID, 0);

//        //    _cards.Add(DECK, card);
//        //    _cardData.Add(instanceID, cardData);

//        //    //Cards.Add(DECK, new Card(null, 0, GetDataOf));
//        //}
//    }

//    private void Update()
//    {
//        //if (Input.GetKeyDown(KeyCode.Space))
//        //{
//        //    foreach (var card in Cards[DECK])
//        //    {
//        //        card.GetDataOf().Name.Log();
//        //    }
//        //}
//    }

//    // ============================================================ Card

//    //private CardData Bind(string instanceID)
//    //{
//    //    return _cardData[instanceID];
//    //}

//    //public Card CreateModel(string instanceID, int serialID)
//    //{

//    //}

//    // ============================================================ CardData

//    //private CardData Attach(string instanceID, int serialID)
//    //{
//    //    if (_cardData.ContainsKey(instanceID))
//    //    {

//    //    }

//    //    var data =
//    //}
//}

// ================================================================================ CardManagerJSON

//public class CardManagerJSON
//{
//    // ================================================================================ Field

//    // ============================================================ Card


//}