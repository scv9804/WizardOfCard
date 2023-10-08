using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Card

//[Serializable]
public class Card : SerializedMonoBehaviour
{
    // ================================================================================ Constance

    // ============================================================ General

    public const int MAX_LEVEL = 2;

    // ================================================================================ Field

    // ============================================================ Data

    //[ShowInInspector]
    //private CardData _data;

    //public event Func<CardData> Data;

    // ================================================================================ Method

    // ============================================================ Constructor

    private Card()
    {

    }

    //public Card(CardData data)
    //{
    //    _data = data;
    //}

    public Card(string instanceID, int serialID)
    {
        instanceID = DataManager.Instance.Allocate(instanceID);

        //_data = data;
    }

    // ============================================================ Event

    public void OnDestroy()
    {
        Delete();
    }

    // ============================================================ Instance

    //public static Card Create()
    //{
    //    var card = new GameObject();

    //    return card.AddComponent<Card>();
    //}

    public void Delete()
    {

    }

    public void Initialize(string instanceID, int serialID)
    {

    }

    //

    //public CardData GetDataOf()
    //{
    //    return Data?.Invoke();
    //}
}