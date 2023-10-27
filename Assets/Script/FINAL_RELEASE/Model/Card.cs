using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

using System;

// ================================================================================ Card

[Serializable]
public class Card : Unit
{
    // ================================================================================ Constant

    // ============================================================ Card

    public const int MAX_LEVEL = 2;

    public const int MAX_HAND_COUNT = 10;

    // ================================================================================ Field

    // ============================================================ Card

    [ShowInInspector, TitleGroup("ī�� ������")]
    private string _name;

    [ShowInInspector, TitleGroup("ī�� ������")]
    private int _level;

    [ShowInInspector, TitleGroup("ī�� ������")]
    private int _cost;

    [ShowInInspector, TitleGroup("ī�� ������")]
    private string _description;

    // ================================================================================ Property

    // ============================================================ Card

    public string Name
    {
        get => _name;

        set => _name = value;
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

    private Card() : base()
    {

    }

    public Card(string instanceID, int serialID) : base(instanceID, serialID)
    {

    }

    // ============================================================ Unit

    public override void Refresh()
    {
        Name = "������";
        Level = 0;
        Cost = 1;
        Description = "�������� 6 �ݴϴ�.";
    }
}
