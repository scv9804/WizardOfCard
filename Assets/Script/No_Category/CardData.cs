using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ CardData

[Serializable]
public class CardData
{
    // ================================================================================ Field

    // ============================================================ Instance

    [ShowInInspector, TitleGroup("개체 데이터")]
    private string _instanceID;

    [ShowInInspector, TitleGroup("개체 데이터")]
    private int _serialID;

    // ============================================================ Card

    [ShowInInspector, TitleGroup("카드 데이터")]
    private string _name;

    [ShowInInspector, TitleGroup("카드 데이터")]
    private CardType _type;

    [ShowInInspector, TitleGroup("카드 데이터")]
    private int _level;

    [ShowInInspector, TitleGroup("카드 데이터")]
    private int _cost;

    [ShowInInspector, TitleGroup("카드 데이터")]
    private string _description;

    // ================================================================================ Property

    // ============================================================ Instance

    public string InstanceID
    {
        get => _instanceID;

        set => _instanceID = value;
    }

    public int SerialID
    {
        get => _serialID;

        set => _serialID = value;
    }

    // ============================================================ Card

    public string Name
    {
        get => _name;

        set => _name = value;
    }

    public CardType Type
    {
        get => _type;

        set => _type = value;
    }

    public int Level
    {
        get => _level;

        set => _level = value;
    }

    public int Cost
    {
        get => _cost;

        set => _cost = value;
    }

    public string Description
    {
        get => _description;

        set => _description = value;
    }

    // ================================================================================ Method

    // ============================================================ Constructor

    private CardData()
    {

    }

    public CardData(string instanceID, int serialID)
    {
        InstanceID = instanceID;
        SerialID = serialID;
    }

    // ============================================================ ????
}